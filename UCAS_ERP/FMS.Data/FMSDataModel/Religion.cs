namespace FMS.Data.FMSDataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Religion
    {
      

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public string ReligionName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ReportOrder { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdatedBy { get; set; }

       public bool IsActive {  get; set; }
    }
}
