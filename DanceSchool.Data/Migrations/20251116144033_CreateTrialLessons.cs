using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanceSchool.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrialLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrialLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstructorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CoordinationScore = table.Column<int>(type: "INTEGER", nullable: true),
                    MusicScore = table.Column<int>(type: "INTEGER", nullable: true),
                    TechniqueScore = table.Column<int>(type: "INTEGER", nullable: true),
                    RecommendedGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrialLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrialLessons_Groups_RecommendedGroupId",
                        column: x => x.RecommendedGroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrialLessons_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrialLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrialLessons_InstructorId",
                table: "TrialLessons",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_TrialLessons_RecommendedGroupId",
                table: "TrialLessons",
                column: "RecommendedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrialLessons_StudentId",
                table: "TrialLessons",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrialLessons");
        }
    }
}
