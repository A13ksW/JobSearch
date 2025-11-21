using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearch.Migrations
{
    /// <inheritdoc />
    public partial class CreateCVSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfileCVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    EducationStatus = table.Column<int>(type: "int", nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreferredCategories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredDepartments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredLocations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteWork = table.Column<int>(type: "int", nullable: false),
                    Relocation = table.Column<int>(type: "int", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CvFileUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileCVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileCVs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileCVId = table.Column<int>(type: "int", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LanguageLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVLanguages_UserProfileCVs_UserProfileCVId",
                        column: x => x.UserProfileCVId,
                        principalTable: "UserProfileCVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileCVId = table.Column<int>(type: "int", nullable: false),
                    SkillName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVSkills_UserProfileCVs_UserProfileCVId",
                        column: x => x.UserProfileCVId,
                        principalTable: "UserProfileCVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CVLanguages_UserProfileCVId",
                table: "CVLanguages",
                column: "UserProfileCVId");

            migrationBuilder.CreateIndex(
                name: "IX_CVSkills_UserProfileCVId",
                table: "CVSkills",
                column: "UserProfileCVId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileCVs_UserId",
                table: "UserProfileCVs",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVLanguages");

            migrationBuilder.DropTable(
                name: "CVSkills");

            migrationBuilder.DropTable(
                name: "UserProfileCVs");
        }
    }
}
