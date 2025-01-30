using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class smthsmth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Tests",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "Tests",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Tests",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Tests",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1dacf5e-5625-4fbe-a78e-3ac472db842c", "AQAAAAIAAYagAAAAELJDN/5I4PsAOvCjZ1OrRdKRUD/SIO/w1olOMTm9+TKwv4cUMY3DXZTkb6a1vzLIFw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f4eb921d-77b5-4d9d-a4bb-710e19ae2ade", "AQAAAAIAAYagAAAAEJd943OMwx4RuZClRhYzVW5H2cT50pvoO8eigM3PXPOQ9oi2jTF1MkiCPri4kzcvVQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9dd3f10c-fef0-4afd-b914-396ef7f95aa4", "AQAAAAIAAYagAAAAELYKWgT7jNvNR0iC09oiOcRDi9L2bXWadlvkTLilWYBr2vhfwGbha3DL4bn97/1SsQ==" });
        }
    }
}
