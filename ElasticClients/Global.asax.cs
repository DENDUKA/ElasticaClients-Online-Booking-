using System.Web.Mvc;
using System.Linq;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using ElasticaClients.DAL;
using ElasticaClients.Infrastructure;

namespace ElasticaClients
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Настройка DI контейнера
            var services = new ServiceCollection();

            // Регистрация DAL сервисов
            services.AddDataAccessServices();

            // Регистрация Business Logic сервисов
            services.AddBusinessLogicServices();

            // Регистрация контроллеров
            var controllerTypes = typeof(MvcApplication).Assembly.GetExportedTypes()
                .Where(t => typeof(IController).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic);

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            // Построить ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Установить DependencyResolver для MVC
            DependencyResolver.SetResolver(new ServiceProviderDependencyResolver(serviceProvider));
        }
    }
}
