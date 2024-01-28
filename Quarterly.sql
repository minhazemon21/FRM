--EXEC USP_RPT_Quarterly_transctions_and_activities_of_the_Branch_offices_of_DSE 10,5,'1 Jan 2024','30 Jun 2024'
ALTER PROC [dbo].[USP_RPT_Quarterly_transctions_and_activities_of_the_Branch_offices_of_DSE]
@AuthorizedRepra int = 0,
@CustomerComplain int = 0,
@QuaterDateFrom DATE='1 Oct 2023',
@QuarterDateTo DATE = '31 Dec 2023',
@QuarterId VARCHAR(5) = 'Q1',
@QuarterName varchar(50) = 'January to March',
@Year varchar(4) = '2023'
AS
BEGIN
DECLARE @HeadOfficeId INT,@MarketId INT, @BranchId INT

SELECT @MarketId=m.id From MarketInformation m WHere m.MarketShortName = 'DSE'
SELECT @HeadOfficeId=B.Id FROM BrokerBranch B WHERE B.BrokerBranchName LIKE '%Head Office%'
SELECT @BranchId = BT.Id FROM BrokerBranchType BT WHERE BT.BranchTypeName LIKE '%Branch%'
--------------
SELECT AC.InvestorId,AC.AccountTypeId,MAX(Case When AC.ValidTo IS NULL Then AC.ValidFrom ELSE AC.ValidTo END) ValidTo,AC.CreateDate 
INTO #MarginList
FROM InvestorAccountTypeChangeHistory AC
	WHERE AC.IsActive =1 AND (Case When AC.ValidTo IS NULL Then AC.ValidFrom ELSE AC.ValidTo END) Between @QuaterDateFrom AND @QuarterDateTo 
	Group BY AC.InvestorId ,AC.AccountTypeId	,AC.CreateDate 
	ORDER BY AC.InvestorId

Select a.InvestorId,a.AccountTypeId, a.ValidTo,a.CreateDate
INTO #MarginListExtended
from #MarginList a Inner Join (Select a.InvestorId,Max(a.ValidTo) ValidTo from #MarginList a Group By a.InvestorId) b on b.InvestorId =a.InvestorId AND b.ValidTo =a.ValidTo 

--------------
	SELECT  
		@QuarterName  QuarterName,
		REPLACE(CONVERT(VARCHAR,@QuarterDateTo,106),' ','-')  AsOnDate
		,a.Trec TREC
		,a.NameofTrecHolderCompany TRECHolderCompany
		,a.BrokerBranchName BranchLocation,
		@AuthorizedRepra NoOfAuthorizedRepresentative
		--,isnull(a.NumberofWorkstation,0) NoOfWorkstation
		,SUM(isnull(d.TotalTransaction,0)) TotalTransctionBuySale
		,isnull(b.NewACOpened,0) NewACOpened
		,isnull(c.BOClosed,0) NoOfACClosed
		,SUM(isnull(e.CollectionAmount,0)) TotalMoneyReceived
		,@CustomerComplain NoOfCustomersComplaint
		,'' Remarks
		,@QuarterId QuarterId, @Year QYear
		,SUM(isnull(f.PaymentAmount,0)) TotalMoneyPaid
		,Count(g.InvestorId) NoOfInvestorMarginExtended
		,SUM(isnull(e.CollectionAmount,0)) TotalMoneyDepositedToBank
		,SUM(isnull(a1.NumberofWorkstation,0)) NoOfWorkstation
		--=========
		,isnull(TraderCodesList,'') AnyChangesInTheQuarter
		
		FROM
		(
		Select 
		O.DSEMemberCode AS Trec
		, O.OrganizationName AS NameofTrecHolderCompany
		, BB.BrokerBranchName
		, BB.Id AS BrokerBranchId
		FROM  BrokerBranch BB 
		CROSS JOIN Organization O
		WHERE BB.BoothBranchExtensionTypeId = @BranchId
		--AND TC.CreateDate Between @QuaterDateFrom AND @QuarterDateTo
		AND BB.IsActive = 1
		GROUP BY O.DSEMemberCode, O.OrganizationName, BB.BrokerBranchName, BB.Id
		) a 
		LEFT JOIN
		(
		SELECT BB.Id AS BrokerBranchId
		,Count(TC.TraderCode) NumberofWorkstation   
		FROM TraderCodes TC 
		INNER JOIN BrokerBranch BB ON BB.Id = TC.BrokerBranchId
		WHERE BB.BoothBranchExtensionTypeId = @BranchId
		AND TC.CreateDate < @QuarterDateTo
		AND BB.IsActive = 1
		GROUP BY BB.Id
		)a1 ON a1.BrokerBranchId = a.BrokerBranchId
		LEFT JOIN
		(
		SELECT a.BrokerBranchId
		, Count(C.BOCode) AS NewACOpened  
		FROM InvestorAccountOpeningFromCDBL C 
		INNER JOIN InvestorAccount a  ON  a.BOID = C.BOCode 
		INNER JOIN BrokerBranch BB ON BB.Id = C.BrokerBranchId
		WHERE C.OpeningDate  Between @QuaterDateFrom AND @QuarterDateTo
		AND BB.BoothBranchExtensionTypeId = @BranchId
		AND a.IsActive = 1 AND C.IsActive = 1
		GROUP BY a.BrokerBranchId 
		) b on b.BrokerBranchId =a.BrokerBranchId
		LEFT JOIN
		(
		SELECT c.BrokerBranchId
		, Count(C.BOID) AS BOClosed  
		FROM InvestorBOClosing C 
		Inner Join InvestorAccount A  ON  A.BOID = C.BOID  
		INNER JOIN BrokerBranch BB ON BB.Id = A.BrokerBranchId
		WHere C.ClosureDate  Between @QuaterDateFrom AND @QuarterDateTo
		AND BB.BoothBranchExtensionTypeId = @BranchId
		AND C.IsActive = 1
		GROUP BY C.BrokerBranchId 
		) c on c.BrokerBranchId =a.BrokerBranchId
		LEFT JOIN 
		(
		SELECT 
		BB.Id as BrokerBranchId
		, SUM(ISNULL(TH.TotalSharePrice,0)) AS  TotalTransaction
		FROM TradeDetailsHistory TH 
		Inner Join InvestorAccount IA on IA.InvestorId = TH.InvestorId 
		INNER JOIN BrokerBranch BB ON BB.Id = IA.BrokerBranchId --InvestorBranchId  
		--INNER JOIN BrokerBranch BB ON BB.Id =TH.InvestorBranchId
		WHere TH.TransactionDate BETWEEN @QuaterDateFrom AND @QuarterDateTo
				AND TH.MarketId = @MarketId
				AND BB.BoothBranchExtensionTypeId = @BranchId
				AND TH.IsActive = 1
		GROUP BY BB.Id 
		) d ON d.BrokerBranchId= a.BrokerBranchId

		LEFT JOIN
		(
		SELECT C.BrokerBranchId
		,SUM(C.Amount) CollectionAmount 
		FROM AccCollection C
		INNER JOIN InvestorAccount IA ON IA.InvestorId = C.InvestorId
		INNER JOIN BrokerBranch BB ON BB.Id = IA.BankBranchId
		WHERE C.IsActive = 1 
			 AND C.TransactionDate BETWEEN @QuaterDateFrom AND @QuarterDateTo
			 AND BB.BoothBranchExtensionTypeId = @BranchId
			 GROUP BY C.BrokerBranchId
		)e ON e.BrokerBranchId = a.BrokerBranchId

		LEFT JOIN (
		SELECT P.BrokerBranchId
		,SUM(P.Amount) PaymentAmount 
		FROM AccPayment  P 
		INNER JOIN InvestorAccount IA ON IA.InvestorId = P.InvestorId
		INNER JOIN BrokerBranch BB ON BB.Id = IA.BrokerBranchId
		WHERE P.IsActive = 1 
		AND P.TransactionDate BETWEEN @QuaterDateFrom AND @QuarterDateTo
		AND BB.BoothBranchExtensionTypeId = @BranchId
		GROUP BY P.BrokerBranchId
		) f on f.BrokerBranchId = a.BrokerBranchId

------------------------------------------------------------------------------------
		LEFT JOIN (
		SELECT count(a.InvestorId) InvestorId, IA.BrokerBranchId   --,a.AccountTypeId, a.ValidTo,  
		FROM #MarginListExtended a Inner Join  
		(Select a.InvestorId,Max(a.CreateDate) CreateDate from #MarginListExtended a Group By a.InvestorId) b on b.InvestorId =a.InvestorId AND b.CreateDate =a.CreateDate 
		INNER JOIN InvestorAccount IA ON IA.InvestorId = a.InvestorId 
		INNER JOIN BrokerBranch BB ON BB.Id = IA.BrokerBranchId
		WHERE a.AccountTypeId =1 
		AND BB.BoothBranchExtensionTypeId = @BranchId
		GROUP BY IA.BrokerBranchId

		) g on g.BrokerBranchId =A.BrokerBranchId

		LEFT JOIN
		(
		SELECT TC.BrokerBranchId
		--,Count(TC.TraderCode) NumberofCurrentWorkstation 
		,STRING_AGG(TC.TraderCode, ', ') AS TraderCodesList
		FROM TraderCodes TC
		INNER JOIN BrokerBranch BB ON BB.Id = TC.BrokerBranchId
		WHERE BB.BoothBranchExtensionTypeId = @BranchId
		AND TC.CreateDate BETWEEN @QuaterDateFrom AND @QuarterDateTo
		AND TC.IsActive = 1
		GROUP BY TC.BrokerBranchId
		) i on i.BrokerBranchId = a.BrokerBranchId
------------------------------------------------------------------------------------

		GROUP BY a.Trec
		,a.NameofTrecHolderCompany
		,a.BrokerBranchName
		--,a.NumberofWorkstation
		,b.NewACOpened
		,c.BOClosed
		,i.TraderCodesList
DROP Table #MarginList
DROP Table #MarginListExtended

END


