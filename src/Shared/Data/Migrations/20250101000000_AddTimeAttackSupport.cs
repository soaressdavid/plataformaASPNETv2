using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Data.Migrations
{
    /// <summary>
    /// Migration to add Time Attack mode support to Challenges and Submissions
    /// Validates: Requirements 31.1, 31.2, 31.3, 31.6, 31.7
    /// </summary>
    public partial class AddTimeAttackSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Time Attack fields to Challenges table
            migrationBuilder.AddColumn<bool>(
                name: "SupportsTimeAttack",
                table: "Challenges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeAttackLimitSeconds",
                table: "Challenges",
                type: "int",
                nullable: false,
                defaultValue: 900); // 15 minutes

            // Add Time Attack fields to Submissions table
            migrationBuilder.AddColumn<bool>(
                name: "IsTimeAttack",
                table: "Submissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompletionTimeSeconds",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeAttackBonusXP",
                table: "Submissions",
                type: "int",
                nullable: true);

            // Create index for Time Attack leaderboard queries
            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ChallengeId_IsTimeAttack_CompletionTimeSeconds",
                table: "Submissions",
                columns: new[] { "ChallengeId", "IsTimeAttack", "CompletionTimeSeconds" },
                filter: "IsTimeAttack = 1 AND Passed = 1 AND IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop index
            migrationBuilder.DropIndex(
                name: "IX_Submissions_ChallengeId_IsTimeAttack_CompletionTimeSeconds",
                table: "Submissions");

            // Remove Time Attack fields from Submissions
            migrationBuilder.DropColumn(
                name: "IsTimeAttack",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CompletionTimeSeconds",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "TimeAttackBonusXP",
                table: "Submissions");

            // Remove Time Attack fields from Challenges
            migrationBuilder.DropColumn(
                name: "SupportsTimeAttack",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "TimeAttackLimitSeconds",
                table: "Challenges");
        }
    }
}
