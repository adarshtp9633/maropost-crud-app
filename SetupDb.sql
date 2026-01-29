USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeeDb')
BEGIN
    CREATE DATABASE EmployeeDb;
END
GO

USE EmployeeDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employee')
BEGIN
    CREATE TABLE Employee (
        EmployeeId NVARCHAR(50) PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Designation NVARCHAR(100) NOT NULL,
        DateOfJoining NVARCHAR(50) NOT NULL, -- Storing as string based on C# model, ideally should be DATE
        Gender NVARCHAR(20) NOT NULL,
        Qualification NVARCHAR(100) NOT NULL,
        State NVARCHAR(50) NOT NULL
    );
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_Employee')
BEGIN
    DROP PROCEDURE sp_Employee;
END
GO

CREATE PROCEDURE sp_Employee
    @Action NVARCHAR(20),
    @EmployeeId NVARCHAR(50) = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @Designation NVARCHAR(100) = NULL,
    @DateOfJoining NVARCHAR(50) = NULL,
    @Gender NVARCHAR(20) = NULL,
    @Qualification NVARCHAR(100) = NULL,
    @State NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO Employee (EmployeeId, FirstName, LastName, Designation, DateOfJoining, Gender, Qualification, State)
        VALUES (@EmployeeId, @FirstName, @LastName, @Designation, @DateOfJoining, @Gender, @Qualification, @State);
    END
    ELSE IF @Action = 'GetEmp'
    BEGIN
        SELECT * FROM Employee WHERE EmployeeId = @EmployeeId;
    END
    ELSE IF @Action = 'GetAllEmp'
    BEGIN
        SELECT * FROM Employee;
    END
    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE Employee
        SET FirstName = @FirstName,
            LastName = @LastName,
            Designation = @Designation,
            DateOfJoining = @DateOfJoining,
            Gender = @Gender,
            Qualification = @Qualification,
            State = @State
        WHERE EmployeeId = @EmployeeId;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        DELETE FROM Employee WHERE EmployeeId = @EmployeeId;
    END
END
GO
