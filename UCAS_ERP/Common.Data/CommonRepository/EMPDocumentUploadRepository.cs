using Common.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.CommonDataModel;

namespace Common.Data.CommonRepository
{
    public interface IEMPDocumentUploadRepository : IRepository<EMP_Document_Upload>
    {

    }
    public class EMPDocumentUploadRepository : RepositoryBaseCodeFirst<EMP_Document_Upload, CommonDbContext>, IEMPDocumentUploadRepository
    {
        public EMPDocumentUploadRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
