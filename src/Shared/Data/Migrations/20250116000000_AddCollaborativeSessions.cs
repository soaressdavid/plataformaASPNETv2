using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Data.Migrations
{
    /// <summary>
    /// Migration to add collaborative coding session tables
    /// Validates: Requirements 32.1, 32.2, 32.6, 32.8, 32.9
    /// </summary>
    public partial class AddCollaborativeSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create CollaborativeSessions table
            migrationBuilder.CreateTable(
                name: "CollaborativeSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborativeSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborativeSessions_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollaborativeSessions_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create CollaborativeSessionParticipants table
            migrationBuilder.CreateTable(
                name: "CollaborativeSessionParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    XPEarned = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaborativeSessionParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollaborativeSessionParticipants_CollaborativeSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "CollaborativeSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaborativeSessionParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create indexes for performance
            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessions_CreatedByUserId",
                table: "CollaborativeSessions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessions_ChallengeId",
                table: "CollaborativeSessions",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessions_Status_CreatedAt",
                table: "CollaborativeSessions",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessionParticipants_SessionId",
                table: "CollaborativeSessionParticipants",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessionParticipants_UserId",
                table: "CollaborativeSessionParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessionParticipants_SessionId_UserId",
                table: "CollaborativeSessionParticipants",
                columns: new[] { "SessionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollaborativeSessionParticipants_IsActive",
                table: "CollaborativeSessionParticipants",
                column: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaborativeSessionParticipants");

            migrationBuilder.DropTable(
                name: "CollaborativeSessions");
        }
    }
}
