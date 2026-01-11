using ElasticaClients.DAL.Entities;
using System;
using System.Collections.Generic;

namespace ElasticaClients.DAL.Data.Interfaces
{
    public interface ITrainingDAL
    {
        List<Training> GetAllForGym(int gymId);
        List<Training> GetAllForGym(int gymId, DateTime start, DateTime end);
        List<Training> GetAllForTrainer(int trainerId, DateTime start, DateTime end);
        void Update(Training t);
        int Add(Training training);
        Training Get(int id);
        Training GetBase(int id);
        void Delete(int id);
        Training GetByYclintsId(int id);
        Training GetIndivid(DateTime date, string yclientId);
    }
}
