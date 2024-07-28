﻿using System.Data.Entity;
using Common.Data.CommonDataModel;
using Common.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.CommonRepository
{
    public interface IApplicationLogRepository : IRepository<ApplicationLog>
    {
        IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
    }
    public class ApplicationLogRepository : RepositoryBaseCodeFirst<ApplicationLog, CommonDbContext>, IApplicationLogRepository
    {
        public ApplicationLogRepository(IDatabaseFactoryCodeFirst<CommonDbContext> databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            IQueryable<ApplicationLog> results = null;
            if (!string.IsNullOrWhiteSpace(organizationId))
                results = DataContext.ApplicationLogs.Where(x => x.OrganizationId == organizationId);
            else
                results = DataContext.ApplicationLogs;

            if (!string.IsNullOrWhiteSpace(filterColumnName))
            {
                if (filterColumnName == "User")
                    results = results.Where(w => w.RequestUser == filterValue);
                else if (filterColumnName == "CreateDate")
                {
                    DateTime filterDate = DateTime.Parse(filterValue);
                    DateTime filterDate1 = DateTime.Parse(filterValue).AddDays(1);
                    results = results.Where(w => w.LogDate.Value >= filterDate && w.LogDate < filterDate1);
                }
                else if (filterColumnName == "ControllerName")
                    results = results.Where(w => w.ControllerName == filterValue);
                else if (filterColumnName == "IP")
                    results = results.Where(w => w.ClientIP == filterValue);
                //IP
            }

            totalCount = results.LongCount();
            var items = results.OrderByDescending(w => w.ApplicationLogId).Skip(startRowIndex).Take(pageSize);
            return items;
        }
    }
}