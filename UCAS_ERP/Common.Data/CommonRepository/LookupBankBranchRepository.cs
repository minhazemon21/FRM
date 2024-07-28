using Common.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.CommonDataModel;

namespace Common.Data.CommonRepository
{
    public interface ILookupBankBranchRepository : IRepository<LookupBankBranch>
    {

    }
    public class LookupBankBranchRepository : RepositoryBaseCodeFirst<LookupBankBranch,CommonDbContext>, ILookupBankBranchRepository
    {
        public LookupBankBranchRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
