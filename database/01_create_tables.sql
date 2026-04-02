-- =============================================================
-- DMS - Create Tables
-- Idempotent: safe to run multiple times
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.Users (
        Id              UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),
        FullName        NVARCHAR(200)       NOT NULL,
        Email           NVARCHAR(256)       NOT NULL,
        PasswordHash    NVARCHAR(512)       NOT NULL,
        Role            INT                 NOT NULL,   -- 1=Employee, 2=Manager, 3=Admin
        Location        NVARCHAR(200)       NULL,
        IsActive        BIT                 NOT NULL DEFAULT 1,
        CreatedAtUtc    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAtUtc    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_Users PRIMARY KEY (Id),
        CONSTRAINT UQ_Users_Email UNIQUE (Email)
    );

    PRINT 'Table Users created.';
END
ELSE
    PRINT 'Table Users already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Devices' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.Devices (
        Id              UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),
        Name            NVARCHAR(200)       NOT NULL,
        SerialNumber    NVARCHAR(100)       NOT NULL,
        AssetTag        NVARCHAR(100)       NOT NULL,
        Brand           NVARCHAR(100)       NOT NULL,
        Model           NVARCHAR(100)       NOT NULL,
        Type            INT                 NOT NULL,   -- 1=Laptop, 2=Desktop, 3=Tablet, 4=Smartphone, 5=Monitor, 6=Peripheral, 99=Other
        Status          INT                 NOT NULL,   -- 1=Available, 2=InUse, 3=UnderMaintenance, 4=Retired
        PurchasedAtUtc  DATETIME2           NULL,
        CreatedAtUtc    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAtUtc    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_Devices PRIMARY KEY (Id),
        CONSTRAINT UQ_Devices_SerialNumber UNIQUE (SerialNumber),
        CONSTRAINT UQ_Devices_AssetTag UNIQUE (AssetTag)
    );

    PRINT 'Table Devices created.';
END
ELSE
    PRINT 'Table Devices already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DeviceAssignments' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.DeviceAssignments (
        Id              UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),
        DeviceId        UNIQUEIDENTIFIER    NOT NULL,
        UserId          UNIQUEIDENTIFIER    NOT NULL,
        AssignedAtUtc   DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        ReturnedAt      DATETIME2           NULL,
        Notes           NVARCHAR(1000)      NULL,

        CONSTRAINT PK_DeviceAssignments PRIMARY KEY (Id),
        CONSTRAINT FK_DeviceAssignments_Devices FOREIGN KEY (DeviceId)
            REFERENCES dbo.Devices(Id) ON DELETE NO ACTION,
        CONSTRAINT FK_DeviceAssignments_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users(Id) ON DELETE NO ACTION
    );

    PRINT 'Table DeviceAssignments created.';
END
ELSE
    PRINT 'Table DeviceAssignments already exists — skipped.';
GO