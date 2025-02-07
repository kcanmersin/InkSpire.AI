using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class nullusetest13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GeneralFeedback",
                table: "Tests",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "Questions",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tests",
                keyColumn: "GeneralFeedback",
                keyValue: null,
                column: "GeneralFeedback",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "GeneralFeedback",
                table: "Tests",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Feedback",
                keyValue: null,
                column: "Feedback",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "Questions",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c6c06ce7-0fd2-4717-8269-354911a47be2", "AQAAAAIAAYagAAAAEHH+18HLpj4i56T1VW73NSQFnkUSASEQ8jmT5ijFk/tF29SVvNEFqL4pjD+ktRM1Ag==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a8fa023a-95b6-4cf8-ab1d-bc2a53576efd", "AQAAAAIAAYagAAAAEGVqmVkfBfx9bXEC+PCUc2tnn+GV6lmheneb1+q0vFW0ft05moNAt5WF9nemVOMv9w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "245abcbb-7a6c-4ded-8ebe-e8240335bae7", "AQAAAAIAAYagAAAAENIQCDLvkbEaHY9GmPojN9shoTFgI9yTEa/d2PylyFFpBQYi5LWjlH8BuvQDsp6a0g==" });
        }
    }
}
