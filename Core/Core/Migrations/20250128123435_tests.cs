using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class tests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TestId",
                table: "Books",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuestionText = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TestId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9153c394-fef9-4db6-a3bd-9028aa5edc94", "AQAAAAIAAYagAAAAEFE15g+iLr6mO/hLZhdLv7x2DeQ+28zuJFIAj0RVJh8zJRam+3uXuZVeoIBD5nvlDQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "986849ba-ab2d-4969-b6c2-9500aaf4a5d3", "AQAAAAIAAYagAAAAEDlOpoKbBRbp/C4uLV/i6MSi1fk1MkZtMLyzG5qrGsHHVFZy4qhIowa5oc94/FGPkw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bfaf981e-0698-4805-b05e-79c5e2b32bf0", "AQAAAAIAAYagAAAAEGv6pDran9HuB9/zsTTfj6C6qaRXUUKBjwMSns5sTOpJzChi2cVlhMPCUg64Qq23kA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_TestId",
                table: "Books",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Tests_TestId",
                table: "Books",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Tests_TestId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Books_TestId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a95d8d07-3aee-43a6-b438-8e1fd7da914b", "AQAAAAIAAYagAAAAEJA9E5J3aiIT2pLU+ZWgmDDylwqGotZ3hPVHstpQWPuL9b60odChbHlOddRcHU9Rig==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "96492f22-05a3-4bb2-901c-85c447b0d386", "AQAAAAIAAYagAAAAEE54xj0FWyo0yjz7unyGJdqGLCIHoXshIQMBqEsWqms0ZVzVTqKB8Ioo3dCVYnyq+w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "90d8811c-c6d2-4fb0-ad69-f26f940a76be", "AQAAAAIAAYagAAAAECF1I+11G3JhbB9IGGQJ4bhEJaH2ssBH4av8CX+EkxZ1utCMhu8z7O+F0ezndRokFw==" });
        }
    }
}
