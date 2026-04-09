-- =============================================================
-- DMS - Seed Data
-- Idempotent: inserts only if rows do not already exist
-- Type: 1=Smartphone, 2=Tablet, 3=Laptop, 99=Other
-- Status: 1=Available, 2=InUse, 3=UnderMaintenance, 4=Retired
-- Password for all users: Employee123!  (BCrypt cost 11)
-- =============================================================

USE DMS;
GO

-- ---------------------------------------------------------------
-- Users
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'ion.ionescu@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('a1000000-0000-0000-0000-000000000001', 'Ion Ionescu', 'ion.ionescu@dms.com',
            '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1, 'Bucharest', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'User ion.ionescu already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'maria.stan@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('a1000000-0000-0000-0000-000000000002', 'Maria Stan', 'maria.stan@dms.com',
            '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1, 'Timisoara', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'User maria.stan already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'andrei.pop@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('a1000000-0000-0000-0000-000000000003', 'Andrei Pop', 'andrei.pop@dms.com',
            '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1, 'Cluj-Napoca', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'User andrei.pop already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'elena.dumitrescu@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('a1000000-0000-0000-0000-000000000004', 'Elena Dumitrescu', 'elena.dumitrescu@dms.com',
            '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1, 'Iasi', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'User elena.dumitrescu already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = 'mihai.georgescu@dms.com')
BEGIN
    INSERT INTO dbo.Users (Id, FullName, Email, PasswordHash, Role, Location, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('a1000000-0000-0000-0000-000000000005', 'Mihai Georgescu', 'mihai.georgescu@dms.com',
            '$2a$11$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 1, 'Brasov', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'User mihai.georgescu already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- Devices — Smartphones (Type = 1)
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IP15P-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000001', 'iPhone 15 Pro', 'SN-APL-IP15P-001', 'AT-APL-001', 'Apple', 'iPhone 15 Pro', 1, 2, 'iOS', '17.0', 'A17 Pro', 8, '2023-10-01', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IP15P-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IP14-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000002', 'iPhone 14', 'SN-APL-IP14-001', 'AT-APL-002', 'Apple', 'iPhone 14', 1, 1, 'iOS', '16.0', 'A15 Bionic', 6, '2022-09-15', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IP14-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IP13-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000003', 'iPhone 13', 'SN-APL-IP13-001', 'AT-APL-003', 'Apple', 'iPhone 13', 1, 4, 'iOS', '15.0', 'A15 Bionic', 4, '2021-09-24', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IP13-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-S24U-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000004', 'Samsung Galaxy S24 Ultra', 'SN-SAM-S24U-001', 'AT-SAM-001', 'Samsung', 'Galaxy S24 Ultra', 1, 2, 'Android', '14', 'Snapdragon 8 Gen 3', 12, '2024-01-17', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-S24U-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-S24-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000005', 'Samsung Galaxy S24', 'SN-SAM-S24-001', 'AT-SAM-002', 'Samsung', 'Galaxy S24', 1, 2, 'Android', '14', 'Exynos 2400', 8, '2024-01-17', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-S24-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-S23-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000006', 'Samsung Galaxy S23', 'SN-SAM-S23-001', 'AT-SAM-003', 'Samsung', 'Galaxy S23', 1, 1, 'Android', '13', 'Snapdragon 8 Gen 2', 8, '2023-02-17', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-S23-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-A54-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000007', 'Samsung Galaxy A54', 'SN-SAM-A54-001', 'AT-SAM-004', 'Samsung', 'Galaxy A54', 1, 1, 'Android', '13', 'Exynos 1380', 8, '2023-04-14', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-A54-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-GOO-P8P-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000008', 'Google Pixel 8 Pro', 'SN-GOO-P8P-001', 'AT-GOO-001', 'Google', 'Pixel 8 Pro', 1, 2, 'Android', '14', 'Google Tensor G3', 12, '2023-10-04', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-GOO-P8P-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-GOO-P7-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000009', 'Google Pixel 7', 'SN-GOO-P7-001', 'AT-GOO-002', 'Google', 'Pixel 7', 1, 3, 'Android', '13', 'Google Tensor G2', 8, '2022-10-13', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-GOO-P7-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-XIA-14P-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000010', 'Xiaomi 14 Pro', 'SN-XIA-14P-001', 'AT-XIA-001', 'Xiaomi', 'Xiaomi 14 Pro', 1, 1, 'Android', '14', 'Snapdragon 8 Gen 3', 12, '2024-02-22', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-XIA-14P-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-ONE-12P-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000011', 'OnePlus 12 Pro', 'SN-ONE-12P-001', 'AT-ONE-001', 'OnePlus', 'OnePlus 12 Pro', 1, 1, 'Android', '14', 'Snapdragon 8 Gen 3', 16, '2024-01-23', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-ONE-12P-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-HUA-P60-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000012', 'Huawei P60 Pro', 'SN-HUA-P60-001', 'AT-HUA-001', 'Huawei', 'P60 Pro', 1, 4, 'Android', '13', 'Snapdragon 8+ Gen 1', 8, '2023-04-06', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-HUA-P60-001 already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- Devices — Tablets (Type = 2)
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IPADP-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000013', 'iPad Pro 12.9 M2', 'SN-APL-IPADP-001', 'AT-APL-004', 'Apple', 'iPad Pro 12.9 M2', 2, 2, 'iPadOS', '16.1', 'Apple M2', 8, '2022-10-18', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IPADP-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IPADA-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000014', 'iPad Air 5', 'SN-APL-IPADA-001', 'AT-APL-005', 'Apple', 'iPad Air 5', 2, 1, 'iPadOS', '15.4', 'Apple M1', 8, '2022-03-08', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IPADA-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-IPAD10-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000015', 'iPad 10th Gen', 'SN-APL-IPAD10-001', 'AT-APL-006', 'Apple', 'iPad 10th Generation', 2, 1, 'iPadOS', '16.1', 'Apple A14 Bionic', 4, '2022-10-26', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-IPAD10-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-TABS9-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000016', 'Samsung Galaxy Tab S9', 'SN-SAM-TABS9-001', 'AT-SAM-005', 'Samsung', 'Galaxy Tab S9', 2, 2, 'Android', '13', 'Snapdragon 8 Gen 2', 8, '2023-08-11', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-TABS9-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-TABS8-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000017', 'Samsung Galaxy Tab S8', 'SN-SAM-TABS8-001', 'AT-SAM-006', 'Samsung', 'Galaxy Tab S8', 2, 1, 'Android', '12', 'Snapdragon 8 Gen 1', 8, '2022-02-25', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-TABS8-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SAM-TABA9-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000018', 'Samsung Galaxy Tab A9', 'SN-SAM-TABA9-001', 'AT-SAM-007', 'Samsung', 'Galaxy Tab A9', 2, 3, 'Android', '13', 'Helio G99', 4, '2023-10-27', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SAM-TABA9-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-LEN-P12P-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000019', 'Lenovo Tab P12 Pro', 'SN-LEN-P12P-001', 'AT-LEN-001', 'Lenovo', 'Tab P12 Pro', 2, 1, 'Android', '12', 'Snapdragon 870', 8, '2023-01-10', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-LEN-P12P-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-HUA-MATEP-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000020', 'Huawei MatePad Pro 13', 'SN-HUA-MATEP-001', 'AT-HUA-002', 'Huawei', 'MatePad Pro 13', 2, 1, 'HarmonyOS', '4.0', 'Kirin 9000S', 12, '2023-06-05', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-HUA-MATEP-001 already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- Devices — Laptops (Type = 3)
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-MBP14-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000021', 'MacBook Pro 14 M3', 'SN-APL-MBP14-001', 'AT-APL-007', 'Apple', 'MacBook Pro 14 M3', 3, 2, 'macOS', '14 Sonoma', 'Apple M3 Pro', 18, '2024-01-10', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-MBP14-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-APL-MBA15-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000022', 'MacBook Air 15 M2', 'SN-APL-MBA15-001', 'AT-APL-008', 'Apple', 'MacBook Air 15 M2', 3, 1, 'macOS', '13 Ventura', 'Apple M2', 8, '2023-06-05', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-APL-MBA15-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-DEL-LAT5540-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000023', 'Dell Latitude 5540', 'SN-DEL-LAT5540-001', 'AT-DEL-001', 'Dell', 'Latitude 5540', 3, 2, 'Windows', '11 Pro', 'Intel Core i7-1365U', 16, '2023-03-15', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-DEL-LAT5540-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-DEL-XPS15-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000024', 'Dell XPS 15', 'SN-DEL-XPS15-001', 'AT-DEL-002', 'Dell', 'XPS 15 9530', 3, 1, 'Windows', '11 Pro', 'Intel Core i7-13700H', 32, '2023-07-20', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-DEL-XPS15-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-LEN-T14S-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000025', 'Lenovo ThinkPad T14s', 'SN-LEN-T14S-001', 'AT-LEN-002', 'Lenovo', 'ThinkPad T14s Gen 4', 3, 2, 'Windows', '11 Pro', 'Intel Core i7-1360P', 16, '2023-05-12', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-LEN-T14S-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-LEN-X1C-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000026', 'Lenovo ThinkPad X1 Carbon', 'SN-LEN-X1C-001', 'AT-LEN-003', 'Lenovo', 'ThinkPad X1 Carbon Gen 11', 3, 1, 'Windows', '11 Pro', 'Intel Core i7-1365U', 16, '2023-02-08', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-LEN-X1C-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-HP-ELB840-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000027', 'HP EliteBook 840 G10', 'SN-HP-ELB840-001', 'AT-HP-001', 'HP', 'EliteBook 840 G10', 3, 3, 'Windows', '11 Pro', 'Intel Core i7-1365U', 16, '2023-09-01', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-HP-ELB840-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-MSI-MOD14-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000028', 'MSI Modern 14', 'SN-MSI-MOD14-001', 'AT-MSI-001', 'MSI', 'Modern 14 C13M', 3, 1, 'Windows', '11 Home', 'Intel Core i7-1355U', 16, '2023-11-15', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-MSI-MOD14-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-ASU-ZB14-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000029', 'ASUS ZenBook 14', 'SN-ASU-ZB14-001', 'AT-ASU-001', 'ASUS', 'ZenBook 14 UX3402', 3, 1, 'Windows', '11 Home', 'Intel Core i5-1240P', 16, '2023-04-22', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-ASU-ZB14-001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE SerialNumber = 'SN-SUR-PRO9-001')
BEGIN INSERT INTO dbo.Devices (Id, Name, SerialNumber, AssetTag, Brand, Model, Type, Status, OperatingSystem, OsVersion, Processor, RamGb, PurchasedAtUtc, CreatedAtUtc, UpdatedAtUtc)
    VALUES ('b2000000-0000-0000-0000-000000000030', 'Microsoft Surface Pro 9', 'SN-SUR-PRO9-001', 'AT-SUR-001', 'Microsoft', 'Surface Pro 9', 3, 1, 'Windows', '11 Pro', 'Intel Core i7-1265U', 16, '2022-10-12', GETUTCDATE(), GETUTCDATE());
END ELSE PRINT 'Device SN-SUR-PRO9-001 already exists — skipped.';
GO

-- ---------------------------------------------------------------
-- DeviceAssignments
-- ---------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000001')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000001', 'b2000000-0000-0000-0000-000000000001', 'a1000000-0000-0000-0000-000000000001', '2024-04-01 08:00:00', NULL, 'Primary work phone.');
END ELSE PRINT 'Assignment c3...001 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000002')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000002', 'b2000000-0000-0000-0000-000000000004', 'a1000000-0000-0000-0000-000000000002', '2024-05-10 09:00:00', NULL, 'Corporate mobile device.');
END ELSE PRINT 'Assignment c3...002 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000003')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000003', 'b2000000-0000-0000-0000-000000000013', 'a1000000-0000-0000-0000-000000000003', '2024-03-01 09:00:00', NULL, 'Field work tablet.');
END ELSE PRINT 'Assignment c3...003 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000004')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000004', 'b2000000-0000-0000-0000-000000000008', 'a1000000-0000-0000-0000-000000000004', '2024-02-14 08:00:00', NULL, 'Work phone.');
END ELSE PRINT 'Assignment c3...004 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000005')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000005', 'b2000000-0000-0000-0000-000000000021', 'a1000000-0000-0000-0000-000000000005', '2024-01-20 08:00:00', NULL, 'Primary work laptop.');
END ELSE PRINT 'Assignment c3...005 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000006')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000006', 'b2000000-0000-0000-0000-000000000016', 'a1000000-0000-0000-0000-000000000002', '2024-06-01 09:00:00', NULL, 'Presentation tablet.');
END ELSE PRINT 'Assignment c3...006 already exists — skipped.';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.DeviceAssignments WHERE Id = 'c3000000-0000-0000-0000-000000000007')
BEGIN INSERT INTO dbo.DeviceAssignments (Id, DeviceId, UserId, AssignedAtUtc, ReturnedAt, Notes)
    VALUES ('c3000000-0000-0000-0000-000000000007', 'b2000000-0000-0000-0000-000000000003', 'a1000000-0000-0000-0000-000000000001', '2021-10-01 08:00:00', '2023-09-30 17:00:00', 'Returned — replaced by iPhone 15 Pro.');
END ELSE PRINT 'Assignment c3...007 already exists — skipped.';
GO