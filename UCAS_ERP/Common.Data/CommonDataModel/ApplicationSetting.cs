namespace Common.Data.CommonDataModel
{
    //using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApplicationSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ApplicationSettingsID { get; set; }

        public int? OfficeId { get; set; }

        public short OrganizationID { get; set; }

        [StringLength(50)]
        public string OrganizationName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MonthClosingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? YearClosingDate { get; set; }

        [StringLength(10)]
        public string CashBook { get; set; }

        [StringLength(50)]
        public string PLAccount { get; set; }

        [StringLength(50)]
        public string BankAccount { get; set; }

        [StringLength(50)]
        public string OrganizationAddress { get; set; }

        [StringLength(25)]
        public string PhoneNo { get; set; }

        [StringLength(25)]
        public string CellNo { get; set; }

        [StringLength(35)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OperationStartDate { get; set; }

        [StringLength(50)]
        public string LicenseNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LicenseStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LicenseEndDate { get; set; }

        [StringLength(50)]
        public string ProcessType { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public int OrgId { get; set; }
       // public virtual Organization Organization { get; set; }
        //public virtual Office Office { get; set; }
    }
}
