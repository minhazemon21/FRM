DECLARE BRANCH CURSOR FOR
SELECT B.Id
FROM BrokerBranch B
WHERE B.Id<>1

DECLARE @BRANCH INT

OPEN BRANCH
FETCH NEXT FROM BRANCH INTO @BRANCH
WHILE(@@FETCH_STATUS=0)
      BEGIN
            INSERT INTO TradeTransactionWithAccountingMapping_New([BrokerBranchId], [OtherBranchId], [MarketId], [TransactionMode]
            , [TransactionTypeId], [TransactionTypeName], [AccIdDr], [AccMasterIdDr], [AccCodeDr], [NarrationDr]
            , [AccIdCr], [AccMasterIdCr], [AccCodeCr], [NarrationCr], [IsAutoVoucher], [IsValid], [ValidFrom], [ValidTo]
            , [GroupableDr], [GroupableCr])
            SELECT @BRANCH,T.OtherBranchId,T.MarketId,T.TransactionMode
                  ,T.TransactionTypeId,T.TransactionTypeName,ISNULL(CHDR.AccID,T.AccIdDr),ISNULL(CHDR.AccMasterId,T.AccMasterIdDr)
                  ,T.AccCodeDr,T.NarrationDr
                  ,ISNULL(CHCR.AccID,T.AccIdCr),ISNULL(CHCR.AccMasterId,T.AccMasterIdCr),T.AccCodeCr,T.NarrationCr,T.IsAutoVoucher,T.IsValid,T.ValidFrom,T.ValidTo
                  ,T.GroupableDr,T.GroupableCr
            FROM TradeTransactionWithAccountingMapping_New T
            LEFT JOIN AccChart CHDR ON CHDR.AccCode=T.AccCodeDr AND CHDR.BrokerBranchId=@BRANCH
            LEFT JOIN AccChart CHCR ON CHCR.AccCode=T.AccCodeDr AND CHCR.BrokerBranchId=@BRANCH
            WHERE T.BrokerBranchId=1

            FETCH NEXT FROM BRANCH INTO @BRANCH
      END
CLOSE BRANCH
DEALLOCATE BRANCH