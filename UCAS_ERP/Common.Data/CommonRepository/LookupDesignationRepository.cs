﻿using Common.Data.CommonDataModel;
using Common.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.CommonRepository
{
    public interface ILookupDesignationRepository : IRepository<LookupDesignation>
    {

    }
    public class LookupDesignationRepository : RepositoryBaseCodeFirst<LookupDesignation, CommonDbContext>, ILookupDesignationRepository
    {
        public LookupDesignationRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {


        }
    }
}
