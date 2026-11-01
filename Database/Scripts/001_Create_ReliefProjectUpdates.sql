/*
    Gift of the Givers Foundation
    Database deployment: 001_Create_ReliefProjectUpdates.sql

    Purpose:
    Create a table for recording relief project progress updates.
*/

SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('dbo.ReliefProjectUpdates', 'U') IS NULL
    BEGIN
        CREATE TABLE dbo.ReliefProjectUpdates
        (
            UpdateId INT IDENTITY(1,1) NOT NULL,
            ProjectId INT NOT NULL,
            UpdateTitle NVARCHAR(150) NOT NULL,
            UpdateDescription NVARCHAR(1000) NOT NULL,

            UpdateDate DATETIME2 NOT NULL
                CONSTRAINT DF_ReliefProjectUpdates_UpdateDate
                DEFAULT SYSUTCDATETIME(),

            UpdatedBy NVARCHAR(100) NULL,

            CONSTRAINT PK_ReliefProjectUpdates
                PRIMARY KEY (UpdateId)
        );

        PRINT 'ReliefProjectUpdates table created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'ReliefProjectUpdates table already exists. No changes required.';
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;