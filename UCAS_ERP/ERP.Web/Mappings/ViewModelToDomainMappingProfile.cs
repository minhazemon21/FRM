//using UcasPortfolio.Data.DBDetailModels;
using AutoMapper;
using Common.Data.CommonDataModel;
using ERP.Web.ViewModels;

namespace ERP.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : AutomapperProfile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<LookupRelationViewModel, LookupRelation>();
            CreateMap<BrokerBranchViewModel, OfficeBranch>();
            CreateMap<AspNetRoleViewModel, AspNetRole>();
            CreateMap<UserLoginViewModel, UserLogin>();
        }
    }
}