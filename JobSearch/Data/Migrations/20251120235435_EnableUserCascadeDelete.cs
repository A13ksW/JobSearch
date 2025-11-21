using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearch.Migrations
{
    /// <inheritdoc />
    public partial class EnableUserCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffer_AspNetUsers_CreatedByUserId",
                table: "JobOffer",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
