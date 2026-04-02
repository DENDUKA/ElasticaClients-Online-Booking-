using Microsoft.Extensions.DependencyInjection;
using ElasticaClients.DAL.Data.Interfaces;
using ElasticaClients.DAL.Data.Mocks;
using ElasticaClients.DAL.Data;

namespace ElasticaClients.DAL
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            // Регистрация Mock сервисов (Singleton для сохранения состояния в памяти)
            services.AddSingleton<IAccountDAL, AccountDAL>();
            services.AddSingleton<IBranchDAL, BranchDAL>();
            services.AddSingleton<IGymDAL, GymDAL>();
            services.AddSingleton<IIncomeDAL, IncomeDAL>();
            services.AddSingleton<IRoleDAL, RoleDAL>();
            services.AddSingleton<ISubscriptionDAL, SubscriptionDAL>();
            services.AddSingleton<ITrainingDAL, TrainingDAL>();
            services.AddSingleton<ITrainingItemDAL, TrainingItemDAL>();

            return services;
        }
    }
}