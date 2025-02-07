using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class nullusetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0a76b436-00a3-44dd-92fc-737a2af77cbf", "AQAAAAIAAYagAAAAEL2nxnKCWiTKbBK5NsV9oKaw2HOTHmqxk+T0ws6JAo5JnDIfh9flv98OxtQiyZ3GnA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bba5a758-b926-4e5e-86a9-304743387d9e", "AQAAAAIAAYagAAAAEEQAAuEUIF/nYakmgWnt9NjzrJOYH5d1/taEnod/CKl9i7PlEjYaspvMwjQzOR8E7A==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7ad2b80e-2735-4e01-9ae7-8df3f6a78dca", "AQAAAAIAAYagAAAAECbLdTUl80kC/DGgUbVigvxu6+cpduHW/UJEcKiXgfVvQoesGkmw1sFr3aJ+x0MlIQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3e449d79-9635-4757-a1b5-b75503be64dd", "AQAAAAIAAYagAAAAEGunIJyQEgk4CqAOGJuPHgapMTzTwd77u/8c8pZFNsYd/bVzCF4ddoSPsLsYIa5gAg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f6c0db5a-92fb-4d55-94cc-cdf31ffec3b5", "AQAAAAIAAYagAAAAEL4gxMluaTeI+8jRbZOuSyJs9Mua7f05OWdjuEaQqe99IieHetBwd0ithUbgram4aA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b9169384-614e-4c91-8313-08436617f4bf", "AQAAAAIAAYagAAAAEEVkUQ3XQUDPecusoErRQ4Zy441KpVii4iCflmcIN6S8VFLZV+UJUl+WckBw3XNdUA==" });
        }
    }
}
