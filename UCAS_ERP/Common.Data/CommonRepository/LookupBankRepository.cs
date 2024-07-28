using Common.Data.Infrastructure;
using Common.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.CommonRepository
{
    public interface ILookupBankRepository : IRepository<LookupBank>
    {

    }
    public class LookupBankRepository : RepositoryBaseCodeFirst<LookupBank,CommonDbContext>, ILookupBankRepository
    {
        public LookupBankRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
