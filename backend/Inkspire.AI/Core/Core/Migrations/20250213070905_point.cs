using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class point : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Point" },
                values: new object[] { "4f1155e5-9852-4d77-9834-1431d0def7f2", "AQAAAAIAAYagAAAAEIHXUI9uz2eabPLHnYMl2cm+21s1eoQiB7uPUEOp+o3UdA109DHLZIKIlS6IsRn6sw==", 0 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Point" },
                values: new object[] { "c87be8fa-5228-4d98-b1fe-a6755618ebfd", "AQAAAAIAAYagAAAAEA09jzidRKo7EyiTzWut5mwE7ZrNkn/khNgn0duJqtZOsM64MspjwDFFiTG95OgGNQ==", 0 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Point" },
                values: new object[] { "06d898ec-26bb-4f03-9e12-9bc611741c6a", "AQAAAAIAAYagAAAAENFUZD+T7XtuC/uGsgCHtNTL3RDMYJQCHulWtf5kPOjMoWocn9HTdUGa1+NkFchdwg==", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "72343990-2959-4774-942e-5bd38fc8368c", "AQAAAAIAAYagAAAAECXY3GMJlnvzeU7qPAovcXDxmTpWaC3Qg/y5DV5hodqmB5jXi4IUXgumRf9a9oj13w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "026b2a1e-4e98-4aee-b477-7769fbc75fad", "AQAAAAIAAYagAAAAEEn6bPO+k0IJ80jYJuQ5HW8sTz3TNiyFNxH9hN6n8v/jDJL13siZLufn1SZNe7rgmA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0e4e6029-c6c6-470a-9aaa-02d107a6b0f2", "AQAAAAIAAYagAAAAEEig02YxuMGNwxytew207lSQTIp9uF3VmgiR6BaHitm3g2mDxXHFvGRiRjEZ6KbJZQ==" });
        }
    }
}
