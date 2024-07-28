using Common.Data.Infrastructure;
using Common.Service;
using  FMS.Data.FMSDataModel;
using FMS.Data.FMSRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Service
{
    public interface IReligionService : IServiceBase<Religion>
    {

    }
    public class ReligionService : IReligionService
    {
        private readonly IReligionRepository repository;
        

        public ReligionService(IReligionRepository repository)
        {
            this.repository = repository;
            
        }
        public IEnumerable<Religion> GetAll()
        {
            var entities = repository.GetAll().Where(m=>m.IsActive == true);
            return entities;
        }
        public Religion GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

       
        public void Save()
        {
            repository.Commit();
        }
        public Religion Create(Religion objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Religion objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }
    }
}
