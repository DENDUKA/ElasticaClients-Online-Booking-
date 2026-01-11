using Microsoft.Extensions.DependencyInjection;
using ElasticaClients.Logic;

namespace ElasticaClients.Infrastructure
{
    public static class LogicServiceConfiguration
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            // Регистрация всех Logic классов как Singleton
            services.AddSingleton<AccountB>();
            services.AddSingleton<BranchB>();
            services.AddSingleton<GymB>();
            services.AddSingleton<RoleB>();
            services.AddSingleton<IncomeB>();
            services.AddSingleton<SubscriptionB>();
            services.AddSingleton<TrainingB>();
            services.AddSingleton<TrainingItemB>();
            services.AddSingleton<LogB>();
            services.AddSingleton<ExcelGenerator>();
            services.AddSingleton<ExcelHelper>();
            services.AddSingleton<Batches>();

            return services;
        }
    }
}
