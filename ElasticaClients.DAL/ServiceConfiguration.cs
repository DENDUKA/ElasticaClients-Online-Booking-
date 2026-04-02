using Microsoft.Extensions.DependencyInjection;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Data.Mocks;

namespace ElasticaClients.DAL
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            // Регистрация Mock сервисов (Singleton для сохранения состояния в памяти)
            services.AddSingleton<IAccountDAL, MockAccountDAL>();
            services.AddSingleton<IBranchDAL, MockBranchDAL>();
            services.AddSingleton<IGymDAL, MockGymDAL>();
            services.AddSingleton<IIncomeDAL, MockIncomeDAL>();
            services.AddSingleton<IRoleDAL, MockRoleDAL>();
            services.AddSingleton<ISubscriptionDAL, MockSubscriptionDAL>();
            services.AddSingleton<ITrainingDAL, MockTrainingDAL>();
            services.AddSingleton<ITrainingItemDAL, MockTrainingItemDAL>();

            return services;
        }
    }
}