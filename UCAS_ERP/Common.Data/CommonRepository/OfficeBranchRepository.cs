using Common.Data.CommonDataModel;
using Common.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.CommonRepository
{
    public interface IOfficeBranchRepository : IRepository<OfficeBranch>
    {

    }
    public class OfficeBranchRepository : RepositoryBaseCodeFirst<OfficeBranch, CommonDbContext>, IOfficeBranchRepository
    {
        public OfficeBranchRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
