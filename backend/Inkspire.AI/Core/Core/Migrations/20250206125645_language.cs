using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class language : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NativeLanguage",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TargetLanguage",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "NativeLanguage", "PasswordHash", "TargetLanguage" },
                values: new object[] { "12f9a75d-196c-433f-80f4-f46b0b0ce80f", "", "AQAAAAIAAYagAAAAEICSkyjGX8Zta66fHIWaXovBodVNGerJRlKbH1NMxDN08t+qI2jiHCoOi6PkSxj/BA==", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "NativeLanguage", "PasswordHash", "TargetLanguage" },
                values: new object[] { "dd15e632-1341-4cd1-80fc-c116545ed125", "", "AQAAAAIAAYagAAAAEOKWbRZR4EOQjYEQthsxrojS+ccCb7b5RSJuvJemmTocAnAI8+FgfOl4tcvepuawdQ==", "" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "NativeLanguage", "PasswordHash", "TargetLanguage" },
                values: new object[] { "aac7cde8-d969-4e06-bcf7-3dba585cbe5d", "", "AQAAAAIAAYagAAAAEL216MWHe3F7vAFwKLsi76L8PRZw9FIolwzgC08W3CgrYh6KjhImy5jUohkRo2K7Ww==", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NativeLanguage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TargetLanguage",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "16135f46-b24f-4544-911f-b3d7e1cafce8", "AQAAAAIAAYagAAAAELceV3iFTnvN28wOfFHcgr+sWCSnH/YNNoXm2IfhoaylmrNDMaPfEFJTtlWFHPBYeA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b2cd1e9c-c8c6-4da3-bca8-9e705b7971ac", "AQAAAAIAAYagAAAAEFOlgs3+76lwizWfTTET+mUg30AJeWQLqlgAYY2nssVOzFCpbsAMf8N9QhvNKICgpQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "78792375-39ea-4134-86e4-58c97db1f246", "AQAAAAIAAYagAAAAEA3nc9iHTjaQXIxKb0Sa0pFUvxFl1dtnpW1rpko+bTUqpUqlOhcHMS9G/KW/FgOixw==" });
        }
    }
}
