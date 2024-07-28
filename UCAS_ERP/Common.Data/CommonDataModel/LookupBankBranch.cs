namespace Common.Data.CommonDataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LookupBankBranch")]
    public partial class LookupBankBranch
    {
        public int Id { get; set; }

        public int? ThanaId { get; set; }

        public int BankId { get; set; }

        [Required]
        [StringLength(300)]
        public string BranchName { get; set; }

        public string Address { get; set; }

        public string RoutingNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public int CreatedUserId { get; set; }

        public int? UpdateUserId { get; set; }

        public bool IsActive { get; set; }

    }
}
