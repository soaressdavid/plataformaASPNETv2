CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Challenges" (
        "Id" uuid NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Description" text NOT NULL,
        "Difficulty" integer NOT NULL,
        "StarterCode" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Challenges" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Courses" (
        "Id" uuid NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Description" text NOT NULL,
        "Level" integer NOT NULL,
        "OrderIndex" integer NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Courses" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Email" character varying(255) NOT NULL,
        "PasswordHash" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "TestCases" (
        "Id" uuid NOT NULL,
        "ChallengeId" uuid NOT NULL,
        "Input" text NOT NULL,
        "ExpectedOutput" text NOT NULL,
        "OrderIndex" integer NOT NULL,
        "IsHidden" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_TestCases" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_TestCases_Challenges_ChallengeId" FOREIGN KEY ("ChallengeId") REFERENCES "Challenges" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Lessons" (
        "Id" uuid NOT NULL,
        "CourseId" uuid NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Content" text NOT NULL,
        "OrderIndex" integer NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Lessons" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Lessons_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Enrollments" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "CourseId" uuid NOT NULL,
        "EnrolledAt" timestamp with time zone NOT NULL,
        "LastAccessedAt" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Enrollments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Enrollments_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Enrollments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Progresses" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "TotalXP" integer NOT NULL,
        "CurrentLevel" integer NOT NULL,
        "LearningStreak" integer NOT NULL,
        "LastActivityAt" timestamp with time zone NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Progresses" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Progresses_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "Submissions" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "ChallengeId" uuid NOT NULL,
        "Code" text NOT NULL,
        "Passed" boolean NOT NULL,
        "Result" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Submissions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Submissions_Challenges_ChallengeId" FOREIGN KEY ("ChallengeId") REFERENCES "Challenges" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Submissions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE TABLE "LessonCompletions" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "LessonId" uuid NOT NULL,
        "CompletedAt" timestamp with time zone NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_LessonCompletions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_LessonCompletions_Lessons_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Lessons" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_LessonCompletions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_Enrollments_CourseId" ON "Enrollments" ("CourseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Enrollments_UserId_CourseId" ON "Enrollments" ("UserId", "CourseId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_LessonCompletions_LessonId" ON "LessonCompletions" ("LessonId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_LessonCompletions_UserId_LessonId" ON "LessonCompletions" ("UserId", "LessonId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_Lessons_CourseId_OrderIndex" ON "Lessons" ("CourseId", "OrderIndex");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Progresses_UserId" ON "Progresses" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_Submissions_ChallengeId" ON "Submissions" ("ChallengeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_Submissions_UserId_ChallengeId" ON "Submissions" ("UserId", "ChallengeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE INDEX "IX_TestCases_ChallengeId_OrderIndex" ON "TestCases" ("ChallengeId", "OrderIndex");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260306173833_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260306173833_InitialCreate', '10.0.3');
    END IF;
END $EF$;
COMMIT;

