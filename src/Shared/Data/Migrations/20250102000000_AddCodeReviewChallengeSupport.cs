using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Data.Migrations
{
    /// <summary>
    /// Migration to add code review challenge support
    /// Validates: Requirement 9.2 - Bug Fixing exercises where users identify and fix errors
    /// </summary>
    public partial class AddCodeReviewChallengeSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCodeReviewChallenge",
                table: "Challenges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CodeToReview",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedIssues",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCodeReviewChallenge",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "CodeToReview",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "ExpectedIssues",
                table: "Challenges");
        }
    }
}
