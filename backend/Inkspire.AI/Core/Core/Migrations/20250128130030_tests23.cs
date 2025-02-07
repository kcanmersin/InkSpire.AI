using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class tests23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Choices",
                table: "Questions",
                type: "json",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choices",
                table: "Questions");

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
        }
    }
}
