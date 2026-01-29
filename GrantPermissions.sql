USE [EmployeeDb]
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'LP-BLR-SDH\CLOPS_Sudarshan')
BEGIN
    CREATE USER [LP-BLR-SDH\CLOPS_Sudarshan] FOR LOGIN [LP-BLR-SDH\CLOPS_Sudarshan]
END
GO

ALTER ROLE [db_owner] ADD MEMBER [LP-BLR-SDH\CLOPS_Sudarshan]
GO
