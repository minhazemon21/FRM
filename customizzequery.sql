select C.TotalCollection from SalaryLoanDisburse L
LEFT JOIN( SELECT C.LoanDisbuseId,C.EmployeeId,C.TotCollectionPrincipal,C.TotCollectionInterest,C.TotalCollection,C.PaidInstallment 
                 FROM UFN_Get_LoanwiseCollectionAndInstallment(0,0,0,0) AS C) AS C ON C.LoanDisbuseId = L.Id


SELECT sal.LoanDisburseAmount
from SalaryLoanDisburse as sal 
where sal.EmployeeId = 2266 and sal.LoanStatus = 2 and sal.IsActive = 1

select (sum(sc.CollectionAmountPrincipal)) from 
SalaryLoanCollection sc

select (sum(sc.CollectionAmountPrincipal) - sal.LoanDisburseAmount) from
SalaryLoanDisburse sal
inner join SalaryLoanCollection sc on
sal.Id = sc.LoanDisbuseId
group by sc.LoanDisbuseId

SELECT SUM(CollectionAmountPrincipal) from SalaryLoanCollection as salc where salc.EmployeeId = 2266

SELECT SUM(InstallmentAmount)as Total from SalaryLoanDisburse as sal where sal.EmployeeId = 2266 and sal.LoanStatus = 2 and sal.IsActive = 1

SELECT ISNULL((sal.LoanDisburseAmount - sum(sc.CollectionAmountPrincipal)), 0) as CollectionAmount from SalaryLoanDisburse as sal 
inner join SalaryLoanCollection as sc on sal.Id = sc.LoanDisbuseId
where sal.LoanStatus = 2 and sal.IsActive = 1 and sal.EmployeeId = 2266
group by sal.LoanDisburseAmount

SELECT (ISNULL(SUM(CollectionAmountPrincipal),0))  as PaidAmount from SalaryLoanCollection as salc where salc.EmployeeId = 47 

SELECT (ISNULL((sal.LoanDisburseAmount - sum(sc.CollectionAmountPrincipal)),0)) as DueAmount from SalaryLoanDisburse as sal inner join SalaryLoanCollection as sc on sal.Id = sc.LoanDisbuseId where sal.LoanStatus = 2 and sal.IsActive = 1 and sal.EmployeeId = 47
 group by sal.LoanDisburseAmount

 select sum(CollectionAmountPrincipal) as c from SalaryLoanCollection where EmployeeId = 3

 select SD.*,LC.* from EMP_PROFILE E
  left join( SELECT ld.EmployeeId,ld.Id LoanDisburseId,ld.LoanDisburseAmount, ld.InstallmentAmount,ld.PrincipalInstallmentAmount,ld.LoanDurationMonth,ld.InterestRate FROM SalaryLoanDisburse ld WHERE ld.IsActive = 1 AND ld.LoanStatus = 2 AND ld.EmployeeId = 2266 ) SD on E.emp_id = SD.EmployeeId
 left join (
             SELECT LC.LoanDisbuseId,SUM(LC.CollectionAmountPrincipal) tCollectionAmountPrincipal FROM SalaryLoanCollection LC 
			 WHERE LC.IsActive = 1 AND  LC.LoanDisbuseId = 17
			 GROUP BY LC.LoanDisbuseId
 ) LC on LC.LoanDisbuseId = SD.LoanDisburseId