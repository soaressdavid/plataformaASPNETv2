-- Add IsDeleted column to all tables for soft delete support
-- This script adds the IsDeleted column to existing tables that don't have it

USE aspnet_learning_platform;
GO

PRINT 'Adding IsDeleted column to tables...';
GO

-- Users table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Users ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Users';
END
ELSE
    PRINT '  Users already has IsDeleted';
GO

-- Courses table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Courses' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Courses ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Courses';
END
ELSE
    PRINT '  Courses already has IsDeleted';
GO

-- Lessons table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Lessons' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Lessons ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Lessons';
END
ELSE
    PRINT '  Lessons already has IsDeleted';
GO

-- Challenges table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Challenges' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Challenges ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Challenges';
END
ELSE
    PRINT '  Challenges already has IsDeleted';
GO

-- TestCases table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TestCases' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE TestCases ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to TestCases';
END
ELSE
    PRINT '  TestCases already has IsDeleted';
GO

-- Submissions table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Submissions' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Submissions ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Submissions';
END
ELSE
    PRINT '  Submissions already has IsDeleted';
GO

-- Enrollments table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Enrollments' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Enrollments ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Enrollments';
END
ELSE
    PRINT '  Enrollments already has IsDeleted';
GO

-- Progresses table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Progresses' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Progresses ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Progresses';
END
ELSE
    PRINT '  Progresses already has IsDeleted';
GO

-- LessonCompletions table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LessonCompletions' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE LessonCompletions ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to LessonCompletions';
END
ELSE
    PRINT '  LessonCompletions already has IsDeleted';
GO

-- Projects table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Projects' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE Projects ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to Projects';
END
ELSE
    PRINT '  Projects already has IsDeleted';
GO

-- ProjectProgresses table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProjectProgresses' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE ProjectProgresses ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to ProjectProgresses';
END
ELSE
    PRINT '  ProjectProgresses already has IsDeleted';
GO

-- CurriculumLevels table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CurriculumLevels' AND COLUMN_NAME = 'IsDeleted')
BEGIN
    ALTER TABLE CurriculumLevels ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT '✓ Added IsDeleted to CurriculumLevels';
END
ELSE
    PRINT '  CurriculumLevels already has IsDeleted';
GO

-- Update indexes to include IsDeleted filter
PRINT '';
PRINT 'Updating indexes to filter out soft-deleted records...';
GO

-- Users Email index
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email' AND object_id = OBJECT_ID('Users'))
BEGIN
    DROP INDEX IX_Users_Email ON Users;
    CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Email ON Users(Email) WHERE IsDeleted = 0;
    PRINT '✓ Updated IX_Users_Email with IsDeleted filter';
END
GO

-- Users Name index
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Name' AND object_id = OBJECT_ID('Users'))
BEGIN
    DROP INDEX IX_Users_Name ON Users;
    CREATE NONCLUSTERED INDEX IX_Users_Name ON Users(Name) WHERE IsDeleted = 0;
    PRINT '✓ Updated IX_Users_Name with IsDeleted filter';
END
GO

-- Lessons CreatedAt index
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Lessons_CreatedAt' AND object_id = OBJECT_ID('Lessons'))
BEGIN
    DROP INDEX IX_Lessons_CreatedAt ON Lessons;
    CREATE NONCLUSTERED INDEX IX_Lessons_CreatedAt ON Lessons(CreatedAt) WHERE IsDeleted = 0;
    PRINT '✓ Updated IX_Lessons_CreatedAt with IsDeleted filter';
END
GO

-- Challenges Difficulty index
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Challenges_Difficulty' AND object_id = OBJECT_ID('Challenges'))
BEGIN
    DROP INDEX IX_Challenges_Difficulty ON Challenges;
    CREATE NONCLUSTERED INDEX IX_Challenges_Difficulty ON Challenges(Difficulty) WHERE IsDeleted = 0;
    PRINT '✓ Updated IX_Challenges_Difficulty with IsDeleted filter';
END
GO

PRINT '';
PRINT '✓ Migration completed successfully!';
PRINT 'All tables now have IsDeleted column for soft delete support.';
GO
