-- Verification script for database migration
-- Checks that all tables have required audit fields and data integrity

USE aspnet_learning_platform;
GO

PRINT '=== Database Migration Verification ===';
PRINT '';

-- Check 1: Verify all tables have audit fields
PRINT '1. Checking audit fields (CreatedAt, UpdatedAt, IsDeleted)...';
GO

DECLARE @MissingFields TABLE (TableName NVARCHAR(128), MissingField NVARCHAR(50));

-- Check for CreatedAt
INSERT INTO @MissingFields
SELECT t.name, 'CreatedAt'
FROM sys.tables t
WHERE t.name NOT IN ('__EFMigrationsHistory')
AND NOT EXISTS (
    SELECT 1 FROM sys.columns c 
    WHERE c.object_id = t.object_id AND c.name = 'CreatedAt'
);

-- Check for UpdatedAt
INSERT INTO @MissingFields
SELECT t.name, 'UpdatedAt'
FROM sys.tables t
WHERE t.name NOT IN ('__EFMigrationsHistory')
AND NOT EXISTS (
    SELECT 1 FROM sys.columns c 
    WHERE c.object_id = t.object_id AND c.name = 'UpdatedAt'
);

-- Check for IsDeleted
INSERT INTO @MissingFields
SELECT t.name, 'IsDeleted'
FROM sys.tables t
WHERE t.name NOT IN ('__EFMigrationsHistory')
AND NOT EXISTS (
    SELECT 1 FROM sys.columns c 
    WHERE c.object_id = t.object_id AND c.name = 'IsDeleted'
);

IF EXISTS (SELECT 1 FROM @MissingFields)
BEGIN
    PRINT '  ✗ Some tables are missing audit fields:';
    SELECT '    ' + TableName + ' missing ' + MissingField FROM @MissingFields;
END
ELSE
BEGIN
    PRINT '  ✓ All tables have required audit fields';
END
GO

-- Check 2: Verify row counts
PRINT '';
PRINT '2. Checking table row counts...';
GO

SELECT 
    t.name AS TableName,
    SUM(p.rows) AS [RowCount]
FROM sys.tables t
JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
AND t.name NOT IN ('__EFMigrationsHistory')
GROUP BY t.name
ORDER BY t.name;
GO

-- Check 3: Verify no soft-deleted records exist (all should be IsDeleted = 0 after migration)
PRINT '';
PRINT '3. Checking for soft-deleted records...';
GO

DECLARE @SoftDeletedCount INT = 0;

-- Check each table
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Users WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Courses WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Lessons WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Challenges WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM TestCases WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Submissions WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Enrollments WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Progresses WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM LessonCompletions WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM Projects WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM ProjectProgresses WHERE IsDeleted = 1);
SELECT @SoftDeletedCount = @SoftDeletedCount + (SELECT COUNT(*) FROM CurriculumLevels WHERE IsDeleted = 1);

IF @SoftDeletedCount = 0
    PRINT '  ✓ No soft-deleted records found (expected after migration)';
ELSE
    PRINT '  ⚠ Found ' + CAST(@SoftDeletedCount AS NVARCHAR) + ' soft-deleted records';
GO

-- Check 4: Verify indexes exist
PRINT '';
PRINT '4. Checking indexes...';
GO

SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    CASE WHEN i.has_filter = 1 THEN 'Yes' ELSE 'No' END AS HasFilter
FROM sys.indexes i
JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.name NOT IN ('__EFMigrationsHistory')
AND i.name IS NOT NULL
ORDER BY t.name, i.name;
GO

-- Check 5: Verify data types
PRINT '';
PRINT '5. Checking audit field data types...';
GO

SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE COLUMN_NAME IN ('CreatedAt', 'UpdatedAt', 'IsDeleted')
AND TABLE_NAME NOT IN ('__EFMigrationsHistory')
ORDER BY TABLE_NAME, COLUMN_NAME;
GO

PRINT '';
PRINT '=== Verification Complete ===';
GO
