using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Cache;
using System.Web;

namespace ADO_Example.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string MotherName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public decimal? Salary { get; set; }
        public int  Age { get; set; }
        //public DateTime CreateDate { get; set; }
        //public bool IsActive { get; set; }


    }
}