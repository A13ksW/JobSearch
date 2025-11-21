using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearch.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienceAndSpecialRequirementsToJobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnlineRecruitment",
                table: "JobOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresDisabilityCertificate",
                table: "JobOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresExperience",
                table: "JobOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresSanitaryBook",
                table: "JobOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresStudentStatus",
                table: "JobOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnlineRecruitment",
                table: "JobOffer");

            migrationBuilder.DropColumn(
                name: "RequiresDisabilityCertificate",
                table: "JobOffer");

            migrationBuilder.DropColumn(
                name: "RequiresExperience",
                table: "JobOffer");

            migrationBuilder.DropColumn(
                name: "RequiresSanitaryBook",
                table: "JobOffer");

            migrationBuilder.DropColumn(
                name: "RequiresStudentStatus",
                table: "JobOffer");
        }
    }
}
