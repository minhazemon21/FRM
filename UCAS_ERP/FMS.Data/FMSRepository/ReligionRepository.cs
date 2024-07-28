using Common.Data.Infrastructure;
using FMS.Data.FMSDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FMS.Data.FMSRepository
{
    public interface IReligionRepository : IRepository<Religion>
    {

    }
    public class ReligionRepository : RepositoryBaseCodeFirst<Religion, FMSDbContext>, IReligionRepository
    {
        public ReligionRepository(IDatabaseFactoryCodeFirst<FMSDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
