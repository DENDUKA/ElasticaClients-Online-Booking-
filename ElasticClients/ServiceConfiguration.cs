using ElasticaClients.Helpers;
using ElasticaClients.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticaClients.DAL
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<AccountB>();
            services.AddScoped<Batches>();
            services.AddScoped<BranchB>();
            services.AddScoped<ExcelGenerator>();
            services.AddScoped<ExcelHelper>();
            services.AddScoped<GymB>();
            services.AddScoped<IncomeB>();
            services.AddScoped<LogB>();
            services.AddScoped<RoleB>();
            services.AddScoped<SubscriptionB>();
            services.AddScoped<TrainingB>();
            services.AddScoped<TrainingItemB>();
            services.AddScoped<NavigationHelper>();

            return services;
        }

    }
}
