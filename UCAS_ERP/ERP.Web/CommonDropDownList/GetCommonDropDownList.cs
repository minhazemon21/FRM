﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Common.Data.CommonDataModel;
using Common.Service.StoredProcedure;
using ERP.Web.Models;

namespace ERP.Web.CommonDropDownList
{
    public class GetCommonDropDownList
    {
        public CommonDbContext dbC = new CommonDbContext();
        List<SelectListItem> dropDownFirstItem;
        private UltimateReportService spService = new UltimateReportService();
        public GetCommonDropDownList()
        {

            dropDownFirstItem = new List<SelectListItem>();
            dropDownFirstItem.Add(new SelectListItem() { Text = "Please Select", Value = "" });
        }
        public IEnumerable<SelectListItem> getOfficeCalenderYear()
        {
            for (int y = DateTime.Now.AddYears(-5).Year; y <= DateTime.Now.AddYears(20).Year; y++)
            {
                dropDownFirstItem.Add(new SelectListItem() { Text = y.ToString(), Value = y.ToString() });

            }
            return dropDownFirstItem;
        }

        public IEnumerable<SelectListItem> getEMPBranchList()
        {
            var list = dbC.OfficeBranches.ToList().Where(b => b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.BranchName
                });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<SelectListItem> getEMPDepartment()
        {
            var list = dbC.BrokerDepartments.ToList().Where(b => b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.DepartmentName
                });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<SelectListItem> getEmpJobType()
        {
            var list = dbC.HR_JOB_TYPE.ToList().Where(b => b.job_enable_flag == 1)
                .Select(b => new SelectListItem
                {
                    Value = b.job_id.ToString(),
                    Text = b.job_name
                });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<SelectListItem> getDesignation()
        {
            var list = dbC.LookupDesignations.ToList().Where(b => b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.DesignationName
                });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<SelectListItem> getBloodGroup()
        {
            var list = dbC.HR_BLOOD_GROUP.ToList()
                .Select(b => new SelectListItem
                {
                    Value = b.blood_group_id.ToString(),
                    Text = b.blood_group_name
                });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<SelectListItem> getEmpActivStatus()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Active", Value = "1" });
            list.Add(new SelectListItem { Text = "In-active", Value = "0" });
            dropDownFirstItem.AddRange(list);
            return dropDownFirstItem;
        }
        public IEnumerable<EmpShortInfoServiceModel> getEmpShortInfoByCodeAndName(string EmpOfficeCode = "", int? Emp_id = 0)
        {
            IEnumerable<EmpShortInfoServiceModel> model = new List<EmpShortInfoServiceModel>();
            try
            {
                string _EmpOfficeCode = string.IsNullOrEmpty(EmpOfficeCode) == true ? "" : EmpOfficeCode;
                var parm = new { EmpOfficeCode = _EmpOfficeCode, Emp_id = Emp_id };
                var list = spService.GetReportDataWithParameter(parm, "USP_GET_EmpShortInfoByEmpCode");
                model = list.Tables[0].AsEnumerable().Select(b => new EmpShortInfoServiceModel
                {
                    emp_id = b.Field<int>("emp_id"),
                    emp_name = b.Field<string>("emp_name"),
                    emp_office_code = b.Field<string>("emp_office_code"),
                    job_name = b.Field<string>("job_name"),
                    job_office_code = b.Field<string>("job_office_code"),
                    branch_name = b.Field<string>("branch_name"),
                    branch_short_name = b.Field<string>("branch_short_name"),
                    dept_name = b.Field<string>("dept_name"),
                    dept_short_name = b.Field<string>("dept_short_name"),
                    desg_name = b.Field<string>("desg_name"),
                    desg_short_name = b.Field<string>("desg_short_name"),

                    dept_id = b.Field<int?>("dept_id"),
                    branch_id = b.Field<int?>("branch_id"),
                    desg_id = b.Field<int?>("desg_id"),
                });
            }
            catch (Exception e)
            {

                model = new List<EmpShortInfoServiceModel>();
            }
            return model;
        }
    }
}