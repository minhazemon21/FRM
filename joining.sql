select * from EMP_PROFILE 
select 
p.emp_name
, p.OfficeEmail
, o.BranchName
, p.MobileNoConvence
, d.DesignationName
, dpt.DepartmentName
, c.CountryName
, ci.CompanyName
, cu.CurrencyName
, isnull(s.PayableAmount,0) PayableAmount
from EMP_PROFILE p
inner join LookupDesignation d on p.emp_desg_id = d.Id
inner join BrokerDepartment dpt on p.emp_dept_id = dpt.Id
inner join LookupCountry c on dpt.CompanyId = c.Id
inner join CompanyInformation ci on p.CompanyId = ci.Id
inner join LookupCurrency cu on p.currency_id = cu.Id
inner join SalaryMaster s on p.emp_id = s.Id
inner join OfficeBranch o on p.emp_branch_id = o.Id
