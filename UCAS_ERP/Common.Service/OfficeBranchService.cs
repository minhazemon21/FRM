using Common.Data.CommonDataModel;
using Common.Data.Infrastructure;
using Common.Data.CommonRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public interface IOfficeBranchService : IServiceBase<OfficeBranch>
    {

    }
    public class OfficeBranchService : IOfficeBranchService
    {
         private readonly IOfficeBranchRepository repository;

         public OfficeBranchService(IOfficeBranchRepository repository)
        {
            this.repository = repository;
        }
        public IEnumerable<OfficeBranch> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true);
            return entities;
        }
        public OfficeBranch GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public void Save()
        {
            repository.Commit();
        }
        public OfficeBranch Create(OfficeBranch objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OfficeBranch objectToUpdate)
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
