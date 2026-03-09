-- Migration: 20260308000000_AddPerformanceIndexes
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260308000000_AddPerformanceIndexes') THEN
        -- Create CurriculumLevels table
        CREATE TABLE "CurriculumLevels" (
            "Id" uuid NOT NULL,
            "Number" integer NOT NULL,
            "Title" character varying(200) NOT NULL,
            "Description" text NOT NULL,
            "RequiredXP" integer NOT NULL,
            "CreatedAt" timestamp with time zone NOT NULL,
            "UpdatedAt" timestamp with time zone NOT NULL,
            CONSTRAINT "PK_CurriculumLevels" PRIMARY KEY ("Id")
        );

        -- Add new columns to Courses table
        ALTER TABLE "Courses" ADD COLUMN "LevelId" uuid NULL;
        ALTER TABLE "Courses" ADD COLUMN "Duration" character varying(50) NOT NULL DEFAULT '';
        ALTER TABLE "Courses" ADD COLUMN "LessonCount" integer NOT NULL DEFAULT 0;
        ALTER TABLE "Courses" ADD COLUMN "Topics" text NOT NULL DEFAULT '[]';

        -- Add new columns to Lessons table
        ALTER TABLE "Lessons" ADD COLUMN "StructuredContent" text NULL;
        ALTER TABLE "Lessons" ADD COLUMN "Duration" character varying(50) NOT NULL DEFAULT '';
        ALTER TABLE "Lessons" ADD COLUMN "Difficulty" character varying(50) NOT NULL DEFAULT 'Iniciante';
        ALTER TABLE "Lessons" ADD COLUMN "EstimatedMinutes" integer NOT NULL DEFAULT 0;
        ALTER TABLE "Lessons" ADD COLUMN "Prerequisites" text NOT NULL DEFAULT '[]';
        ALTER TABLE "Lessons" ADD COLUMN "Version" integer NOT NULL DEFAULT 1;

        -- Add foreign key and indexes
        CREATE INDEX "IX_Courses_LevelId" ON "Courses" ("LevelId");
        ALTER TABLE "Courses" ADD CONSTRAINT "FK_Courses_CurriculumLevels_LevelId" 
            FOREIGN KEY ("LevelId") REFERENCES "CurriculumLevels" ("Id") ON DELETE SET NULL;

        CREATE INDEX "IX_Lessons_CourseId" ON "Lessons" ("CourseId");
        CREATE INDEX "IX_CurriculumLevels_Number" ON "CurriculumLevels" ("Number");

        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20260308000000_AddPerformanceIndexes', '10.0.3');
    END IF;
END $EF$;
