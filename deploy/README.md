# Развёртывание на Linux (прод)

Прод-сервер: `185.43.5.115`, пользователь `root`, каталог приложения
`/home/FlyStratch/ElasticClients`.

Стек: Ubuntu 24.04 → nginx (80/443) → Mono 6.8 + XSP4 (`127.0.0.1:8080`) →
ASP.NET MVC 5 → SQL Server 2022 в Docker (`127.0.0.1:1433`).

## Файлы в этом каталоге

| Файл | Куда на сервере |
|---|---|
| `flystretch.service` | `/etc/systemd/system/flystretch.service` |
| `nginx/flystretch.conf` | `/etc/nginx/sites-available/flystretch` |

## Автозапуск приложения (обязательно)

Без юнита приложение не поднимается после перезагрузки сервера: nginx стартует
сам, а XSP4 — нет, и сайт отдаёт **502 Bad Gateway**. Штатный
`mono-xsp4.service` для этого не годится — в его init-скрипте
`start_boot=false`, он принципиально не запускается при загрузке.

```bash
cp deploy/flystretch.service /etc/systemd/system/flystretch.service
systemctl daemon-reload
systemctl enable --now flystretch.service

systemctl is-enabled flystretch   # enabled
systemctl is-active  flystretch   # active
```

`Restart=always` перезапускает приложение и при падении процесса (проверено
через `kill -9`: поднимается за ~10 секунд).

## nginx

```bash
cp deploy/nginx/flystretch.conf /etc/nginx/sites-available/flystretch
ln -sf /etc/nginx/sites-available/flystretch /etc/nginx/sites-enabled/flystretch
nginx -t && systemctl reload nginx
```

Редирект на 80-м порту для запросов по IP (`$host = 185.43.5.115`) добавлен
поверх заглушки certbot: без него `http://185.43.5.115/` отдавал 404.

## Порядок обновления кода

```bash
cd /home/FlyStratch
git pull
cd ElasticClients
MONO_GAC_PREFIX=/usr/local:/usr xbuild ./ElasticaClients.csproj /p:Configuration=Debug
cd ../ElasticaClients.DAL
MONO_GAC_PREFIX=/usr/local:/usr xbuild ./ElasticaClients.DAL.csproj /p:Configuration=Debug
systemctl restart flystretch
curl -s -o /dev/null -w '%{http_code}\n' http://127.0.0.1:8080/
```

Каталог `packages/` нужен рядом с проектом (`/home/FlyStratch/packages`),
иначе xbuild падает на `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`.

## Диагностика

```bash
systemctl status flystretch
tail -50 /root/flystretch-xsp.log
tail -20 /var/log/nginx/error.log      # 502 здесь = приложение не слушает 8080
ss -tlnp | grep 8080
docker ps --filter name=elastica-mssql
```

Кэш скомпилированных представлений лежит в `/tmp/root-temp-aspnet-0/<hash>/`.
При странностях с представлениями его можно удалить и перезапустить сервис.

## Известные подводные камни

- **`await Task.Run(...)` в контроллере ломает запрос.** На Mono/XSP4 после
  такого `await` не восстанавливается `SynchronizationContext`, поэтому
  `HttpContext.Current` становится `null`. Симптомы: MVC не находит
  представление (`The view 'Index' or its master was not found` при живом
  `.cshtml`) либо падает `NullReferenceException` в коде, читающем
  `HttpContext.Current`. Инцидент с `/Account` (2026-11-12) — ровно этот случай,
  починено коммитом `42cfa7b` (синхронный `GetWithFilter`).
- **`MapPath`/`FileExists` ведут себя не так, как на IIS.** Не стоит
  диагностировать проблему с представлениями по `VirtualPathProvider` —
  надёжнее проверять `BuildManager.GetCompiledType` и atime файла.
- **`MyRoleProvider.IsUserInRole` всегда возвращает `true`**
  (`ElasticClients/Models/RoleProvider.cs`). Атрибуты `[Authorize(Roles = "...")]`
  фактически не ограничивают доступ — пускают любого авторизованного
  пользователя. Роли проверяются только там, где вызывается `User.IsInRole`
  напрямую. Не закрыто, требует отдельной задачи.
- **Память 3.8 ГБ.** Собирать проекты по одному: параллельная сборка упирается
  в OOM.
