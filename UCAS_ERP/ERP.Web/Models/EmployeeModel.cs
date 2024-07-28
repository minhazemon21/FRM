using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Web.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public HttpPostedFileBase PhotographMsg { get; set; }
        public HttpPostedFileBase SignatureMsg { get; set; }
    }
}