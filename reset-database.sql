-- Reset database script
-- This will drop and recreate the database

USE master;
GO

-- Drop the database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'aspnet_learning_platform')
BEGIN
    ALTER DATABASE aspnet_learning_platform SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE aspnet_learning_platform;
    PRINT 'Database dropped successfully.';
END
ELSE
BEGIN
    PRINT 'Database does not exist.';
END
GO

-- Create the database
CREATE DATABASE aspnet_learning_platform;
GO

PRINT 'Database created successfully. EF Core will create tables on next run.';
GO
