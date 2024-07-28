﻿using System.ComponentModel.DataAnnotations;

namespace ERP.Web.ViewModels
{
    public class AspNetRoleViewModel: BaseModel
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name="Home Page")]
        [StringLength(500)]
        //[RegularExpression(@"^[a-zA-Z]+$")]
        public string DefaultLinkURL { get; set; }
    }
}