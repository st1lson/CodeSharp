using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSharp.Samples.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompilationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompiledSuccessfully = table.Column<bool>(type: "bit", nullable: false),
                    ExecutedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    CompilationDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    ExecutionDuration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeReport = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompilationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompiledSuccessfully = table.Column<bool>(type: "bit", nullable: false),
                    TestedSuccessfully = table.Column<bool>(type: "bit", nullable: false),
                    CompilationDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    TestingDuration = table.Column<TimeSpan>(type: "time", nullable: true),
                    TestResults = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CodeReport = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tests = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompilationLogs");

            migrationBuilder.DropTable(
                name: "TestLogs");

            migrationBuilder.DropTable(
                name: "Tests");
        }
    }
}
