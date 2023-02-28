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

