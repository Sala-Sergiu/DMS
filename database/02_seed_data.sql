-- =============================================================
-- DMS - Seed Data
-- Idempotent: inserts only if rows do not already exist
-- Passwords hashed with BCrypt (cost factor 11)
--   Admin/Manager password : Admin123!
--   Employee password      : Employee123!
-- =============================================================

-- ---------------------------------------------------------------
-- Users
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'admin@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'a1000000-0000-0000-0000-000000000001',
        'Sergiu Sala',
        'admin@dms.com',
        '$2a$11$KoMCRVBmkFp2F/7KS0r4O.pV1yUKVVsOk5CmM4fQwHHqvKj6Hm7oS',
        3,  -- Admin
        'Bucharest',
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'User admin@dms.com inserted.';
END
ELSE
    PRINT 'User admin@dms.com already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'manager@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'a1000000-0000-0000-0000-000000000002',
        'Andrei Popescu',
        'manager@dms.com',
        '$2a$11$KoMCRVBmkFp2F/7KS0r4O.pV1yUKVVsOk5CmM4fQwHHqvKj6Hm7oS',
        2,  -- Manager
        'Cluj-Napoca',
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'User manager@dms.com inserted.';
END
ELSE
    PRINT 'User manager@dms.com already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'ion.ionescu@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'a1000000-0000-0000-0000-000000000003',
        'Ion Ionescu',
        'ion.ionescu@dms.com',
        '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi',
        1,  -- Employee
        'Bucharest',
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'User ion.ionescu@dms.com inserted.';
END
ELSE
    PRINT 'User ion.ionescu@dms.com already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'maria.stan@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'a1000000-0000-0000-0000-000000000004',
        'Maria Stan',
        'maria.stan@dms.com',
        '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi',
        1,  -- Employee
        'Timisoara',
        1,
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'User maria.stan@dms.com inserted.';
END
ELSE
    PRINT 'User maria.stan@dms.com already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- Devices
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-DELL-001')
BEGIN
    INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'b2000000-0000-0000-0000-000000000001',
        'Dell Latitude 5540',
        'SN-DELL-001',
        'AT-DELL-001',
        'Dell',
        'Latitude 5540',
        1,  -- Laptop
        2,  -- InUse
        '2023-03-15T00:00:00Z',
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'Device SN-DELL-001 inserted.';
END
ELSE
    PRINT 'Device SN-DELL-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APPLE-001')
BEGIN
    INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'b2000000-0000-0000-0000-000000000002',
        'MacBook Pro 14',
        'SN-APPLE-001',
        'AT-APPLE-001',
        'Apple',
        'MacBook Pro 14 M3',
        1,  -- Laptop
        1,  -- Available
        '2024-01-10T00:00:00Z',
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'Device SN-APPLE-001 inserted.';
END
ELSE
    PRINT 'Device SN-APPLE-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-001')
BEGIN
    INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'b2000000-0000-0000-0000-000000000003',
        'Samsung Galaxy S24',
        'SN-SAM-001',
        'AT-SAM-001',
        'Samsung',
        'Galaxy S24',
        4,  -- Smartphone
        2,  -- InUse
        '2024-02-20T00:00:00Z',
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'Device SN-SAM-001 inserted.';
END
ELSE
    PRINT 'Device SN-SAM-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-LG-001')
BEGIN
    INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'b2000000-0000-0000-0000-000000000004',
        'LG UltraWide 34"',
        'SN-LG-001',
        'AT-LG-001',
        'LG',
        'UltraWide 34WN80C',
        5,  -- Monitor
        1,  -- Available
        '2022-11-05T00:00:00Z',
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'Device SN-LG-001 inserted.';
END
ELSE
    PRINT 'Device SN-LG-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-HP-001')
BEGIN
    INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES (
        'b2000000-0000-0000-0000-000000000005',
        'HP EliteDesk 800',
        'SN-HP-001',
        'AT-HP-001',
        'HP',
        'EliteDesk 800 G6',
        2,  -- Desktop
        3,  -- UnderMaintenance
        '2021-06-01T00:00:00Z',
        GETUTCDATE(),
        GETUTCDATE()
    );
    PRINT 'Device SN-HP-001 inserted.';
END
ELSE
    PRINT 'Device SN-HP-001 already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- DeviceAssignments
-- Active assignments (ReturnedAt = NULL) must match device Status=InUse
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000001')
BEGIN
    INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES (
        'c3000000-0000-0000-0000-000000000001',
        'b2000000-0000-0000-0000-000000000001',  -- Dell Latitude 5540
        'a1000000-0000-0000-0000-000000000003',  -- Ion Ionescu
        '2024-04-01T08:00:00Z',
        NULL,
        'Primary work laptop.'
    );
    PRINT 'Assignment Dell -> Ion Ionescu inserted.';
END
ELSE
    PRINT 'Assignment c3000000-...0001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000002')
BEGIN
    INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES (
        'c3000000-0000-0000-0000-000000000002',
        'b2000000-0000-0000-0000-000000000003',  -- Samsung Galaxy S24
        'a1000000-0000-0000-0000-000000000004',  -- Maria Stan
        '2024-05-10T09:00:00Z',
        NULL,
        'Corporate mobile device.'
    );
    PRINT 'Assignment Samsung -> Maria Stan inserted.';
END
ELSE
    PRINT 'Assignment c3000000-...0002 already exists — skipped.';
GO

-- Historical assignment (returned)
IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000003')
BEGIN
    INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES (
        'c3000000-0000-0000-0000-000000000003',
        'b2000000-0000-0000-0000-000000000002',  -- MacBook Pro 14
        'a1000000-0000-0000-0000-000000000003',  -- Ion Ionescu
        '2024-01-15T08:00:00Z',
        '2024-03-20T17:00:00Z',
        'Returned after project completion.'
    );
    PRINT 'Historical assignment MacBook -> Ion Ionescu inserted.';
END
ELSE
    PRINT 'Assignment c3000000-...0003 already exists — skipped.';
GO