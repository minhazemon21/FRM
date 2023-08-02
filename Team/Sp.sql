ALTER PROCEDURE [dbo].[USP_Insert_Team](
@teamId INT,
@teamName VARCHAR(200),
@teamStName VARCHAR(50),
@teamLeaderId INT,
@remarks VARCHAR(200),
@userId INT

)
AS
BEGIN
	IF EXISTS (SELECT T.TeamLeaderId FROM CRMTeam T WHERE T.IsActive = 1 AND T.TeamLeaderId = @teamLeaderId AND T.Id != @teamId)  
	BEGIN
	SELECT 1 [Response],  O.ExecutiveName +' - '+ O.ExecutiveCode +' is a Team Leader of '+ C.TeamName +', Please Remove From Existing Team' [Message] FROM CRMTeam C INNER JOIN OfficeExecutive O ON C.TeamLeaderId = O.Id WHERE C.TeamLeaderId = @teamLeaderId AND C.IsActive = 1
	END

	ELSE IF NOT EXISTS (SELECT A.UserId FROM AspNetUsers A WHERE A.UserId = @teamLeaderId AND A.IsActive = 1)
	BEGIN
	SELECT 2 [Response], 'This Executive Have no User Account' [Message]
	END

	ELSE
	BEGIN
	IF @teamId = 0
		BEGIN
		DECLARE @crmTeamId INT = 0
		SELECT @crmTeamId = ISNULL(MAX(T.Id),0) + 1 FROM CRMTeam T

		INSERT INTO CRMTeam(TeamName, TeamShortName, TeamLeaderId, Remarks, EntryDate, EntryUserId, IsActive)
		VALUES(@teamName,@teamStName,@teamLeaderId,@remarks,getdate(),@userId, 1)

		INSERT INTO CRMTeamHistory(CRMTeamId, TeamName, TeamShortName, TeamLeaderId, Remarks, EntryDate, EntryUserId, IsActive, ValidFrom)
		VALUES(@crmTeamId, @teamName, @teamStName, @teamLeaderId, @remarks, getdate(), @userId, 1, getdate())

		UPDATE AspNetUsers 
		SET RoleId = 14
		WHERE UserId = @teamLeaderId
	

		-- UPDATE EXECUTIVE TEAM
		UPDATE OfficeExecutive 
		SET CRMTeamId = @crmTeamId
		WHERE Id = @teamLeaderId 

		---- EXECUTIVE HISTORY

		INSERT INTO OfficeExecutiveHistory([OfficeExecutiveId], [ExecutiveName], [ExecutiveCode], [OrganizationId], [DesignationId], [DepartmentId], [BranchId], 
		 [JoiningDate], [FatherName], [MotherName], [GenderId], [PresentAddress], [PresentThanaId], [PermanentAddress], [PermanentThanaId], [CountryId], [Mobile],
		 [Email], [Photograph], [Signature], [EntryDate], [EntryUserId], [UpdateDate], [UpdateUserId], [IsActive], [VaildFrom], [ValidTo], [CRMTeamId],IsCRMTeamChanged,CRMTeamVaildFrom,CRMTeamValidTo)
		 SELECT [Id]
			  ,O.[ExecutiveName]
			  ,O.[ExecutiveCode]
			  ,O.[OrganizationId]
			  ,O.[DesignationId]
			  ,O.[DepartmentId]
			  ,O.[BranchId]
			  ,O.[JoiningDate]
			  ,O.[FatherName]
			  ,O.[MotherName]
			  ,O.[GenderId]
			  ,O.[PresentAddress]
			  ,O.[PresentThanaId]
			  ,O.[PermanentAddress]
			  ,O.[PermanentThanaId]
			  ,O.[CountryId]
			  ,O.[Mobile]
			  ,O.[Email]
			  ,O.[Photograph]
			  ,O.[Signature]
			  ,O.[EntryDate]
			  ,O.[EntryUserId]
			  ,O.[UpdateDate]
			  ,O.[UpdateUserId]
			  ,O.[IsActive]
			  ,ISNULL(O.UpdateDate,O.EntryDate)
			  ,GETDATE()
			  ,@crmTeamId
			  ,0
			  ,GETDATE()
			  ,NULL
		  FROM [OfficeExecutive] O
		  WHERE O.Id = @teamLeaderId


		SELECT 3 [Response], 'Save Successfully' [Message]
		END
	ELSE
	BEGIN

	 
		 UPDATE CRMTeamHistory
		SET ValidTo = getdate(),
			UpdateDate = getdate(),
			UpdateUserId = @userId
		WHERE Id = (SELECT TOP 1 H.Id FROM CRMTeamHistory H WHERE H.CRMTeamId = @teamId ORDER BY ID DESC)
		--AND IsLeaderChange = 0 


		If @teamLeaderId = (select t.TeamLeaderId from CRMTeam t where t.Id = @teamId and t.IsActive = 1)
			BEGIN
			--UPDATE AspNetUsers 
			--	SET RoleId = 14
			--	WHERE UserId = @teamLeaderId

				INSERT INTO CRMTeamHistory(CRMTeamId, TeamName, TeamShortName, TeamLeaderId, Remarks, EntryDate, EntryUserId, IsActive, ValidFrom)
				VALUES(@teamId, @teamName, @teamStName, @teamLeaderId, @remarks, getdate(), @userId, 1, getdate())

				INSERT INTO OfficeExecutiveHistory([OfficeExecutiveId], [ExecutiveName], [ExecutiveCode], [OrganizationId], [DesignationId], [DepartmentId], [BranchId], 
							 [JoiningDate], [FatherName], [MotherName], [GenderId], [PresentAddress], [PresentThanaId], [PermanentAddress], [PermanentThanaId], [CountryId], [Mobile],
							 [Email], [Photograph], [Signature], [EntryDate], [EntryUserId], [UpdateDate], [UpdateUserId], [IsActive], [VaildFrom], [ValidTo], [CRMTeamId],IsCRMTeamChanged,CRMTeamVaildFrom,CRMTeamValidTo)
							 SELECT [Id]
							  ,O.[ExecutiveName]
							  ,O.[ExecutiveCode]
							  ,O.[OrganizationId]
							  ,O.[DesignationId]
							  ,O.[DepartmentId]
							  ,O.[BranchId]
							  ,O.[JoiningDate]
							  ,O.[FatherName]
							  ,O.[MotherName]
							  ,O.[GenderId]
							  ,O.[PresentAddress]
							  ,O.[PresentThanaId]
							  ,O.[PermanentAddress]
							  ,O.[PermanentThanaId]
							  ,O.[CountryId]
							  ,O.[Mobile]
							  ,O.[Email]
							  ,O.[Photograph]
							  ,O.[Signature]
							  ,O.[EntryDate]
							  ,O.[EntryUserId]
							  ,O.[UpdateDate]
							  ,O.[UpdateUserId]
							  ,O.[IsActive]
							  ,ISNULL(O.UpdateDate,O.EntryDate)
							  ,GETDATE()
							  ,@teamId
							  ,0
							  ,GETDATE()
							  ,NULL
						  FROM [OfficeExecutive] O
						  WHERE O.Id = @teamLeaderId

			END
		ELSE
			BEGIN
			
				UPDATE AspNetUsers 
				SET RoleId = 10
				WHERE UserId = (SELECT TOP 1 H.TeamLeaderId FROM CRMTeamHistory H WHERE H.CRMTeamId = @teamId ORDER BY ID DESC)

				UPDATE AspNetUsers 
				SET RoleId = 14
				WHERE UserId = @teamLeaderId

				UPDATE CRMTeamHistory 
				SET IsLeaderChange = 1
				WHERE Id = (SELECT TOP 1 H.Id FROM CRMTeamHistory H WHERE H.CRMTeamId = @teamId ORDER BY ID DESC)

				INSERT INTO CRMTeamHistory(CRMTeamId, TeamName, TeamShortName, TeamLeaderId, Remarks, EntryDate, EntryUserId, IsActive, ValidFrom, IsLeaderChange)
				VALUES(@teamId, @teamName, @teamStName, @teamLeaderId, @remarks, getdate(), @userId, 1, getdate(), 0)

				UPDATE OfficeExecutive
				SET CRMTeamId = @teamId
				WHERE Id = @teamLeaderId

				UPDATE OfficeExecutiveHistory
				SET CRMTeamValidTo = GETDATE(),
					IsCRMTeamChanged = 1
				
				WHERE Id = (SELECT TOP 1 H.Id FROM OfficeExecutiveHistory H WHERE H.CRMTeamId = @teamId ORDER BY ID DESC)

				INSERT INTO OfficeExecutiveHistory([OfficeExecutiveId], [ExecutiveName], [ExecutiveCode], [OrganizationId], [DesignationId], [DepartmentId], [BranchId], 
							 [JoiningDate], [FatherName], [MotherName], [GenderId], [PresentAddress], [PresentThanaId], [PermanentAddress], [PermanentThanaId], [CountryId], [Mobile],
							 [Email], [Photograph], [Signature], [EntryDate], [EntryUserId], [UpdateDate], [UpdateUserId], [IsActive], [VaildFrom], [ValidTo], [CRMTeamId],IsCRMTeamChanged,CRMTeamVaildFrom,CRMTeamValidTo)
							 SELECT [Id]
							  ,O.[ExecutiveName]
							  ,O.[ExecutiveCode]
							  ,O.[OrganizationId]
							  ,O.[DesignationId]
							  ,O.[DepartmentId]
							  ,O.[BranchId]
							  ,O.[JoiningDate]
							  ,O.[FatherName]
							  ,O.[MotherName]
							  ,O.[GenderId]
							  ,O.[PresentAddress]
							  ,O.[PresentThanaId]
							  ,O.[PermanentAddress]
							  ,O.[PermanentThanaId]
							  ,O.[CountryId]
							  ,O.[Mobile]
							  ,O.[Email]
							  ,O.[Photograph]
							  ,O.[Signature]
							  ,O.[EntryDate]
							  ,O.[EntryUserId]
							  ,O.[UpdateDate]
							  ,O.[UpdateUserId]
							  ,O.[IsActive]
							  ,ISNULL(O.UpdateDate,O.EntryDate)
							  ,GETDATE()
							  ,@teamId
							  ,0
							  ,GETDATE()
							  ,NULL
						  FROM [OfficeExecutive] O
						  WHERE O.Id = @teamLeaderId

			END
	
	


		 UPDATE CRMTeam 
		 SET TeamName = @teamName,
			 TeamShortName = @teamStName,
			 TeamLeaderId = @teamLeaderId,
			 Remarks = @remarks,
			 UpdateDate = getdate(),
			 UpdateUserId = @userId
		 WHERE Id = @teamId

	 

	
	 SELECT 4 [Response], 'Update Successfully' [Message]

	END
	END
END

