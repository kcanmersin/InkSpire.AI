using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class feedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralFeedback",
                table: "Tests",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Questions",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "837e414a-abce-4191-ac64-37a6a7cbccf6", "AQAAAAIAAYagAAAAECvFPpBDtnGytGp7eF+YeKUa7i1uKJTvwDnyCSFPbsfWfxX6X6IRalgnMwEaWFJzTA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "577780df-e842-423d-9ec9-b53ee56c6925", "AQAAAAIAAYagAAAAEKyUA8hEiKB8Y2nvFSfFSz/gCh7fLdWxyuBRn851tS3GQcFtGFD0gAzGk7gQrnNc7w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "73ff23cd-fe32-4431-aa89-e8ef3c2ccf6c", "AQAAAAIAAYagAAAAEJvKuAaID6GzNE2PcqLGm+gbNKbyDmWROQ05y7ucNOQ3Tn6fahD67eV4qEKGtrjmOQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralFeedback",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Questions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d413f725-23f1-48a0-936e-f0fac891bd20", "AQAAAAIAAYagAAAAEAcN/CU+kTVOuNlmInm+h0NXoSp7rk5PkMXDpnAFYyfZMHD+NaPY7Yx8ecWKNuK3KA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0fbed38f-599e-40af-a174-5c4c2e3fcb22", "AQAAAAIAAYagAAAAEFqCRNEi1OwFxOOs03zMcR//w3cuW6OmZU1E0RcIr8i2tTRUgxpKtTnFk6lx+hyq8w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c0b17ef1-ddbb-4be9-9970-eb0cdf18a4fd", "AQAAAAIAAYagAAAAEPyO6mufLv8BTog6VHW+rbRsKiLP/IHv7yO8w0QdQ3Y/qbmWrx4mprldQ4S/g4CnuA==" });
        }
    }
}
