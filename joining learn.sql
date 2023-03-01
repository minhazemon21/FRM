SELECT SUB.Id, SUB.AccSubledgerMasterId, SUB.SubledgerDetailsName FROM AccSubledgerDetails AS SUB
 INNER JOIN AccSubledgerMaster AS SUBMS ON SUB.AccSubledgerMasterId = SUBMS.Id

 SELECT SUB.Id, SUB.AccSubledgerMasterId, SUB.SubledgerDetailsName FROM AccSubledgerDetails AS SUB
 LEFT JOIN AccSubledgerMaster AS SUBMS ON SUB.AccSubledgerMasterId = SUBMS.Id

 SELECT * FROM AccSubledger
Select * from AccSubledgerDetails
Select * from AccSubledgerMaster
EXEC USP_GET_SubledgerDetailsList

SELECT MST.SubledgerMasterName, DTL.SubledgerDetailsName, SL.SubledgerName
FROM ((AccSubledgerDetails AS DTL INNER JOIN AccSubledgerMaster AS MST ON
DTL.AccSubledgerMasterId = MST.Id)
INNER JOIN AccSubledger AS SL ON
MST.AccSubledgerId = SL.Id)

SELECT MST.SubledgerMasterName, DTL.SubledgerDetailsName
FROM AccSubledgerDetails AS DTL RIGHT JOIN AccSubledgerMaster AS MST ON
DTL.AccSubledgerMasterId = MST.Id

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

select 
o.Name
, o.Income
--, m.Id
--, s.Id
, s.Expense
, sum(m.Amount) as MnthlyExAmount
, s.Expense + sum(m.Amount) as TotalExp
, o.Income - (s.Expense + sum(m.Amount)) as CurrentBalance
from OBroInfo o
left join MonthlyExp m on o.ID = m.Id
inner join Status s on o.Status = s.Id
group by m.Id, o.ID, o.Name, o.Income, s.Id, s.Expense