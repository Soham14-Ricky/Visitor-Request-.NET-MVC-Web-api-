CREATE DATABASE VisitorRequestDB;
GO

USE VisitorRequestDB;
GO

------  table


CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY(1,1),

    Username NVARCHAR(50) NOT NULL UNIQUE,

    PasswordHash NVARCHAR(200) NOT NULL,

    FullName NVARCHAR(100) NOT NULL,

    Role NVARCHAR(20) NOT NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO




CREATE TABLE VisitorRequests
(
    RequestId INT PRIMARY KEY IDENTITY(1,1),

    VisitorName NVARCHAR(100) NOT NULL,

    MobileNumber NVARCHAR(15) NOT NULL,

    CompanyName NVARCHAR(100) NULL,

    PersonToMeet NVARCHAR(100) NOT NULL,

    PurposeOfVisit NVARCHAR(500) NULL,

    VisitDate DATETIME NOT NULL,

    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',

    Remarks NVARCHAR(500) NULL,

    CreatedBy INT NOT NULL,

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    ModifiedDate DATETIME NULL,

    CONSTRAINT FK_VisitorRequests_Users
    FOREIGN KEY (CreatedBy)
    REFERENCES Users(UserId)
);
GO





--======================================================================================
-- Error_Log_Visitor // Stores Error Logs Occured during the Process ()Sp and calls
--======================================================================================
Create table Error_Log_Visitor(
Error_log_id Bigint identity(1,1) primary key not null,
ErrorNumber varchar(1000) null,
ErrorSeverity varchar(1000) null,
ErrorState varchar(1000) null,
ErrorProcedure varchar(1000) null,
ErrorLine varchar(1000) null,
ErrorMessage varchar(4000) null,
Created_Date smalldatetime null
) 


--   insert in user table
INSERT INTO Users
(
    Username,
    PasswordHash,
    FullName,
    Role
)
VALUES
(
    'employee',
    '123',
    'Employee User',
    'Employee'
),
(
    'admin',
    '123',
    'Admin User',
    'Admin'
);
GO

select * from users




-- =====    SP create

CREATE OR ALTER PROCEDURE sp_GetUserPasswordHash
(
    @Username NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT PasswordHash
        FROM Users
        WHERE Username = @Username
          AND IsActive = 1;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO

--=================================================================================

CREATE OR ALTER PROCEDURE sp_GetUserDetails
(
    @Username NVARCHAR(50)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            UserId,
            Username,
            FullName,
            Role
        FROM Users
        WHERE Username = @Username
          AND IsActive = 1;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--==================================================================================
CREATE OR ALTER PROCEDURE sp_LoginUser
(
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            UserId,
            Username,
            FullName,
            Role
        FROM Users
        WHERE Username = @Username
          AND PasswordHash = @PasswordHash
          AND IsActive = 1;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--================================================================

CREATE OR ALTER PROCEDURE sp_CreateVisitorRequest
(
    @VisitorName NVARCHAR(100),
    @MobileNumber NVARCHAR(15),
    @CompanyName NVARCHAR(100),
    @PersonToMeet NVARCHAR(100),
    @PurposeOfVisit NVARCHAR(250),
    @VisitDate DATETIME,
    @CreatedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        INSERT INTO VisitorRequests
        (
            VisitorName,
            MobileNumber,
            CompanyName,
            PersonToMeet,
            PurposeOfVisit,
            VisitDate,
            Status,
            CreatedBy
        )
        VALUES
        (
            @VisitorName,
            @MobileNumber,
            @CompanyName,
            @PersonToMeet,
            @PurposeOfVisit,
            @VisitDate,
            'Pending',
            @CreatedBy
        );

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO

--============================================================
CREATE OR ALTER PROCEDURE sp_GetMyRequests
(
    @CreatedBy INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT *
        FROM VisitorRequests
        WHERE CreatedBy = @CreatedBy
        ORDER BY RequestId DESC;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO

--======================================================

CREATE OR ALTER PROCEDURE sp_GetPendingRequests
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT *
        FROM VisitorRequests
        WHERE Status = 'Pending'
        ORDER BY RequestId DESC;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--=============================================================
CREATE OR ALTER PROCEDURE sp_UpdateVisitorRequest
(
    @RequestId INT,
    @VisitorName NVARCHAR(100),
    @MobileNumber NVARCHAR(15),
    @CompanyName NVARCHAR(100),
    @PersonToMeet NVARCHAR(100),
    @PurposeOfVisit NVARCHAR(250),
    @VisitDate DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE VisitorRequests
        SET
            VisitorName = @VisitorName,
            MobileNumber = @MobileNumber,
            CompanyName = @CompanyName,
            PersonToMeet = @PersonToMeet,
            PurposeOfVisit = @PurposeOfVisit,
            VisitDate = @VisitDate
        WHERE
            RequestId = @RequestId
            AND LTRIM(RTRIM(Status)) = 'Pending';

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--====================================================



CREATE OR ALTER PROCEDURE sp_DeleteVisitorRequest
(
    @RequestId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DELETE FROM VisitorRequests
        WHERE RequestId = @RequestId
          AND Status = 'Pending';

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO


--=======================================================

CREATE OR ALTER PROCEDURE sp_ApproveRequest
(
    @RequestId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE VisitorRequests
        SET
            Status = 'Approved',
            ModifiedDate = GETDATE()
        WHERE RequestId = @RequestId;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--===========================================================================



CREATE OR ALTER PROCEDURE sp_RejectRequest
(
    @RequestId INT,
    @Remarks NVARCHAR(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE VisitorRequests
        SET
            Status = 'Rejected',
            Remarks = @Remarks,
            ModifiedDate = GETDATE()
        WHERE RequestId = @RequestId;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--==================================================================================================
sp_help 'VisitorRequests'

select * from VisitorRequests

EXEC sp_CreateVisitorRequest
    @VisitorName = 'Rahul',
    @MobileNumber = '9876543210',
    @CompanyName = 'Infosys',
    @PersonToMeet = 'Manager',
    @PurposeOfVisit = 'Meeting',
    @VisitDate = '2026-05-25',
    @CreatedBy = 1



    sp_helptext 'sp_UpdateVisitorRequest'

    EXEC sp_UpdateVisitorRequest
    @RequestId = 4,
    @VisitorName = 'SOHAM CHATTERJEE',
    @MobileNumber = '9999999999',
    @CompanyName = 'TCS',
    @PersonToMeet = 'Director',
    @PurposeOfVisit = 'Updated',
    @VisitDate = '2026-05-26'

--======================================================================================
CREATE OR ALTER PROCEDURE sp_GetVisitorRequestById
(
    @RequestId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            RequestId,
            VisitorName,
            MobileNumber,
            CompanyName,
            PersonToMeet,
            PurposeOfVisit,
            VisitDate,
            Status,
            Remarks,
            CreatedBy
        FROM VisitorRequests
        WHERE RequestId = @RequestId;

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--====================================================================================


CREATE OR ALTER PROCEDURE sp_DeleteVisitorRequest
(
    @RequestId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DELETE FROM VisitorRequests
        WHERE RequestId = @RequestId
          AND Status = 'Pending';

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--==================================================================================================

CREATE OR ALTER PROCEDURE sp_GetPendingVisitorRequests
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            RequestId,
            VisitorName,
            MobileNumber,
            CompanyName,
            PersonToMeet,
            PurposeOfVisit,
            VisitDate,
            Status,
            Remarks,
            CreatedBy
        FROM VisitorRequests
        WHERE Status = 'Pending';

    END TRY
    BEGIN CATCH

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO
--=============================================================================

CREATE OR ALTER PROCEDURE sp_ApproveVisitorRequest
(
    @RequestId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE VisitorRequests
        SET
            Status = 'Approved',
            Remarks = NULL
        WHERE RequestId = @RequestId
          AND Status = 'Pending';

            SELECT CAST(1 AS BIT) AS IsSuccess;
     END TRY
    BEGIN CATCH
                SELECT CAST(0 AS BIT) AS IsSuccess;

        INSERT INTO Error_Log_Visitor
        (
            ErrorNumber,
            ErrorSeverity,
            ErrorState,
            ErrorProcedure,
            ErrorLine,
            ErrorMessage,
            Created_Date
        )
        VALUES
        (
            ERROR_NUMBER(),
            ERROR_SEVERITY(),
            ERROR_STATE(),
            ERROR_PROCEDURE(),
            ERROR_LINE(),
            ERROR_MESSAGE(),
            GETDATE()
        );

        THROW;

    END CATCH
END
GO