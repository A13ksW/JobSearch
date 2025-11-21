using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearch.Migrations
{
    /// <inheritdoc />
    public partial class UserOffersAndApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "JobOffer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobOfferId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_JobOffer_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffer_CreatedByUserId",
                table: "JobOffer",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobOfferId",
                table: "JobApplications",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobOffer_CreatedByUserId",
                table: "JobOffer");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "JobOffer");
        }
    }
}
