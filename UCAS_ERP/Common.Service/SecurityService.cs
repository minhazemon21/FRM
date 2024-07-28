using Common.Data;
using Common.Data.CommonDataModel;
using Common.Data.Infrastructure;
using Common.Data.CommonRepository;
using Common.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Service.ExtentionMethod;

using System.Data;
using Common.Service.StoredProcedure;


namespace Common.Service
{

    public interface ISecurityService : IServiceBase<AspNetSecurityModule>
    {
        IEnumerable<UcasSoftware_Projects> GetAllProject(int roleId, string ProjectShortName);
        IEnumerable<UcasSoftware_Projects> UserwiseGetAllProject(int UserId);
        IEnumerable<AspNetSecurityModule> GetAllPrentModule(string ProjectName);
        IEnumerable<AspNetSecurityModule> GetSecondPrentModule(string ProjectName);
        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId);
        void CreateSecurityRole(List<AspNetRoleModule> roleModules);
        IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId, string ProjectName);
        IEnumerable<ReportInformation> GetReportModules(int UserId, string ProjectName);
    }
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository repository;
        private readonly IAspNetRoleModuleSPService aspNetRoleModuleSPService;
        private readonly ISPService spService;

        public SecurityService(ISecurityRepository repository, IAspNetRoleModuleSPService aspNetRoleModuleSPService, ISPService spService)
        {
            this.repository = repository;
            this.aspNetRoleModuleSPService = aspNetRoleModuleSPService;
            this.spService = spService;
        }

        public IEnumerable<AspNetSecurityModule> GetAllPrentModule(string ProjectName)
        {
            return repository.GetAllPrentModule(ProjectName);
        }
        public IEnumerable<AspNetSecurityModule> GetSecondPrentModule(string ProjectName)
        {
            return repository.GetSecondPrentModule(ProjectName);
        }
        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId)
        {
            return repository.GetAllModulesForParent(parentModuleId, roleId);
        }

        public IEnumerable<AspNetSecurityModule> GetAll()
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule Create(AspNetSecurityModule objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(AspNetSecurityModule objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            repository.Commit();
        }


        public void CreateSecurityRole(List<AspNetRoleModule> roleModules)
        {
            repository.CreateSecurityRole(roleModules);
            Save();
        }
        public IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId, string ProjectName)
        {
            var param = new { RoleId = roleId, ProjectName = ProjectName };

            var childMenu = aspNetRoleModuleSPService.GetAspNetRoleModule(param);
            var RoleModuleList = new List<AspNetSecurityModule>();

            foreach (DataRow dr in childMenu.Tables[0].Rows)
            {
                RoleModuleList.Add(new AspNetSecurityModule
                {
                    AspNetSecurityModuleId = Convert.ToInt32(dr["AspNetSecurityModuleId"]),
                    SecurityModuleCode = dr["SecurityModuleCode"].ToString(),
                    LinkText = dr["LinkText"].ToString(),
                    ControllerName = dr["ControllerName"].ToString(),
                    ActionName = dr["ActionName"].ToString(),
                    ParentModuleId = dr["ParentModuleId"] as int?,
                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                    IsMenuItem =
                        Convert.ToBoolean(string.IsNullOrEmpty(dr["IsMenuItem"].ToString())
                            ? "true"
                            : dr["IsMenuItem"].ToString()),
                    MenuLevel = Convert.ToInt32(dr["MenuLevel"])
                    ,
                    ProjectShortName = dr["ProjectShortName"].ToString(),
                    AreaName = dr["AreaName"].ToString(),
                    DisplayOrder = Convert.ToInt32(dr["DisplayOrder"].ToString()),
                    IsShownAfterDayEnd = (dr["IsShownAfterDayEnd"].ToString()).ToNullableInt()
                });
            }
            var roleModules = RoleModuleList.ToList();

            return roleModules;
        }
        public IEnumerable<UcasSoftware_Projects> GetAllProject(int roleId, string ProjectShortName)
        {
            var param = new { RoleId = roleId, ProjectShortName = ProjectShortName };
            var Projects = spService.GetDataWithParameter(param, "GetRole_wise_AllUcasProject");// spService.GetDataBySqlCommand("SELECT DISTINCT M.ProjectShortName FROM AspNetRoleModule AS R INNER JOIN AspNetSecurityModule AS M ON M.AspNetSecurityModuleId = R.ModuleId WHERE R.IsActive = 1 AND R.RoleId = " + roleId + "");//.Tables[0].AsEnumerable().Select(row => new { ProjectShortName = row.Field<string>("ProjectShortName") }).ToList();

            var ProjectList = new List<UcasSoftware_Projects>();

            foreach (DataRow p in Projects.Tables[0].Rows)
            {
                ProjectList.Add(new UcasSoftware_Projects { ProjectName = p["ProjectName"].ToString(), ProjectShortName = p["ProjectShortName"].ToString(), Style_Css = p["Style_Css"].ToString(), ProjectHomePage = p["ProjectHomePage"].ToString() });
            }
            var UserProject = ProjectList.ToList();

            return UserProject;
        }
        public IEnumerable<UcasSoftware_Projects> UserwiseGetAllProject(int UserId)
        {
            var param = new { UserId = UserId };
            var Projects = spService.GetDataWithParameter(param, "GetRole_wise_AllUcasParentProject");// spService.GetDataBySqlCommand("SELECT DISTINCT M.ProjectShortName FROM AspNetRoleModule AS R INNER JOIN AspNetSecurityModule AS M ON M.AspNetSecurityModuleId = R.ModuleId WHERE R.IsActive = 1 AND R.RoleId = " + roleId + "");//.Tables[0].AsEnumerable().Select(row => new { ProjectShortName = row.Field<string>("ProjectShortName") }).ToList();

            var ProjectList = new List<UcasSoftware_Projects>();

            foreach (DataRow p in Projects.Tables[0].Rows)
            {
                ProjectList.Add(new UcasSoftware_Projects { ProjectName = p["ProjectName"].ToString(), ProjectShortName = p["ProjectShortName"].ToString(), Style_Css = p["Style_Css"].ToString(), ProjectHomePage = p["ProjectHomePage"].ToString(), ImageLink = p["ImageLink"].ToString(), ProjectLink = p["ProjectLink"].ToString() });
            }
            var UserProject = ProjectList.ToList();

            return UserProject;
        }

        public IEnumerable<ReportInformation> GetReportModules(int UserId, string ProjectName)
        {
            var param = new { UserId = UserId, ProjectName = ProjectName };

            var RptMod = aspNetRoleModuleSPService.GetReportPermissionModule(param);

            var ReportModuleList = new List<ReportInformation>();

            foreach (DataRow dr in RptMod.Tables[0].Rows)
            {
                ReportModuleList.Add(new ReportInformation { ControllerName = dr["ControllerName"].ToString(), ActionName = dr["ActionName"].ToString(), ProjectShortName = dr["ProjectShortName"].ToString(), SerialNo = Convert.ToInt32(dr["SerialNo"]), Id = Convert.ToInt32(dr["ReportId"]) });
            }
            var roleModules = ReportModuleList.ToList();

            return roleModules;
        }
    }
}
