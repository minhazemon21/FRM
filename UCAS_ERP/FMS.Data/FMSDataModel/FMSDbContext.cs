using Common.Data.CommonDataModel;

namespace FMS.Data.FMSDataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FMS.Data.FMSDataModel;
    using System.Collections.Generic;
    using System.Data;


    public partial class FMSDbContext : DbContext
    {
        public FMSDbContext()
            : base("name=ERPDbContext")
        {
        }
       
        public virtual DbSet<Religion> Religion { get; set; }
    }
}
