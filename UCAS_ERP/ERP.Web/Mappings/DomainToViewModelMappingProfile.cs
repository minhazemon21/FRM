//using UcasPortfolio.Data.DBDetailModels;
using AutoMapper;
using Common.Data.CommonDataModel;
using ERP.Web.ViewModels;

namespace ERP.Web.Mappings
{
    public class DomainToViewModelMappingProfile : AutomapperProfile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<LookupRelation, LookupRelationViewModel>();
            CreateMap<OfficeBranch, BrokerBranchViewModel>();

            CreateMap<AspNetRole, AspNetRoleViewModel>();

            CreateMap<UserLogin, UserLoginViewModel>();
        }
       
    }
}