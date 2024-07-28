﻿namespace Common.Data.CommonDataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Common.Data.CommonDataModel;
    using BasicDataAccess;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public partial class CommonDbContext : DbContext
    {        
        public CommonDbContext()
            : base("name=ERPDbContext")
        {
        }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<AspNetRoleModule> AspNetRoleModules { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetSecurityLevel> AspNetSecurityLevels { get; set; }
        public virtual DbSet<AspNetSecurityModule> AspNetSecurityModules { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<OfficeBranch> OfficeBranches { get; set; }
        public virtual DbSet<BrokerDepartment> BrokerDepartments { get; set; }
        public virtual DbSet<LookupCountry> LookupCountries { get; set; }
        public virtual DbSet<LookupDesignation> LookupDesignations { get; set; }
        public virtual DbSet<LookupDistrict> LookupDistricts { get; set; }
        public virtual DbSet<LookupDivision> LookupDivisions { get; set; }
        public virtual DbSet<LookupOccupation> LookupOccupations { get; set; }
        public virtual DbSet<LookupRelation> LookupRelations { get; set; }
        public virtual DbSet<LookupThana> LookupThanas { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<EMP_PROFILE> EMP_PROFILEs { get; set; }
        public virtual DbSet<EMP_PERSONAL_DETAILS_PROFILE> EMP_PERSONAL_DETAILS_PROFILE { get; set; }

        public virtual DbSet<HR_BLOOD_GROUP> HR_BLOOD_GROUP { get; set; }
        public virtual DbSet<HR_DESK> HR_DESK { get; set; }
        public virtual DbSet<HR_JOB_TYPE> HR_JOB_TYPE { get; set; }
        public virtual DbSet<LookupBankBranch> LookupBankBranch { get; set; }
        public virtual DbSet<LookupBank> LookupBank { get; set; }
        public virtual DbSet<EMP_Document_Upload> EMP_Document_Upload { get; set; }

       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ActionURL)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ClientIP)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestUser)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.QueryStringParams)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ErrorDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.UserAgent)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.HttpMethod)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.SessionId)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.OrganizationId)
                .IsUnicode(false);
            modelBuilder.Entity<AspNetRole>()
                .Property(e => e.DefaultLinkURL)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.SecurityModuleCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.LinkText)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetUser>()
                .Property(e => e.PersonType)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.ContactMobile)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeBranch>()
                .Property(e => e.ContactFax)
                .IsUnicode(false);

            modelBuilder.Entity<BrokerDepartment>()
                .Property(e => e.DepartmentShortName)
                .IsUnicode(false);

            modelBuilder.Entity<BrokerDepartment>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<LookupCountry>()
                .Property(e => e.CountryName)
                .IsUnicode(false);


            modelBuilder.Entity<LookupDesignation>()
                .Property(e => e.DesignationShortName)
                .IsUnicode(false);

            modelBuilder.Entity<LookupDesignation>()
                .Property(e => e.DesignationName)
                .IsUnicode(false);

            modelBuilder.Entity<LookupDistrict>()
                .Property(e => e.DistrictName)
                .IsUnicode(false);

            modelBuilder.Entity<LookupDivision>()
                .Property(e => e.DivisionName)
                .IsUnicode(false);

            modelBuilder.Entity<LookupOccupation>()
                .Property(e => e.Occupation)
                .IsUnicode(false);


            modelBuilder.Entity<LookupRelation>()
                .Property(e => e.Relation)
                .IsUnicode(false);


            modelBuilder.Entity<LookupThana>()
                .Property(e => e.ThanaName)
                .IsUnicode(false);



            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FirstLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.SecondLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.ThirdLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FourthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.PostCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);


            modelBuilder.Entity<UserLogin>()
                .Property(e => e.PersonType)
                .IsUnicode(false);

            modelBuilder.Entity<ReportInformation>().ToTable("ReportInformation");
            modelBuilder.Entity<ReportAccess>().ToTable("ReportAccess");
            modelBuilder.Entity<AspNetSecurityModule>().ToTable("AspNetSecurityModule");
        }
    }
}

