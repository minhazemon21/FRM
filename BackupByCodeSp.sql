ALTER PROCEDURE [dbo].[USP_GENERATE_DATABASE_BACKUP]
	@DATABASE_NAME VARCHAR(500)='UPMS_Blank',@BACK_UP_PATH VARCHAR(500)='E:\Backup\',@FILE_NAME VARCHAR(500)=''
AS
BEGIN
SET NOCOUNT ON;
	EXEC master.dbo.xp_create_subdir @BACK_UP_PATH

	EXEC SP_configure 'show advanced options', 1

	RECONFIGURE

	EXEC SP_configure 'xp_cmdshell', 1

	RECONFIGURE

	DECLARE @DELETE_FILE VARCHAR(500)

	SET @DELETE_FILE='del '+LOWER(LEFT(@BACK_UP_PATH,1))+RIGHT(@BACK_UP_PATH,LEN(@BACK_UP_PATH)-1)+@FILE_NAME+'.bak'

	EXEC xp_cmdshell @DELETE_FILE

	DECLARE @SQL_QUERY VARCHAR(500)
	SET @SQL_QUERY='BACKUP DATABASE ' + @DATABASE_NAME + ' TO DISK = ''' + @BACK_UP_PATH+@FILE_NAME+'.bak' + ''''

	EXEC(@SQL_QUERY)
	SET NOCOUNT OFF;
END