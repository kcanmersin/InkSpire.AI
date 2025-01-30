using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class _2fa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorEnabled",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorCode",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactorExpiryTime",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "IsTwoFactorEnabled", "PasswordHash", "TwoFactorCode", "TwoFactorExpiryTime" },
                values: new object[] { "3e449d79-9635-4757-a1b5-b75503be64dd", false, "AQAAAAIAAYagAAAAEGunIJyQEgk4CqAOGJuPHgapMTzTwd77u/8c8pZFNsYd/bVzCF4ddoSPsLsYIa5gAg==", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "IsTwoFactorEnabled", "PasswordHash", "TwoFactorCode", "TwoFactorExpiryTime" },
                values: new object[] { "f6c0db5a-92fb-4d55-94cc-cdf31ffec3b5", false, "AQAAAAIAAYagAAAAEL4gxMluaTeI+8jRbZOuSyJs9Mua7f05OWdjuEaQqe99IieHetBwd0ithUbgram4aA==", "", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "IsTwoFactorEnabled", "PasswordHash", "TwoFactorCode", "TwoFactorExpiryTime" },
                values: new object[] { "b9169384-614e-4c91-8313-08436617f4bf", false, "AQAAAAIAAYagAAAAEEVkUQ3XQUDPecusoErRQ4Zy441KpVii4iCflmcIN6S8VFLZV+UJUl+WckBw3XNdUA==", "", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorExpiryTime",
                table: "AspNetUsers");

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
    }
}
