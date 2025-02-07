using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class words111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExamplesJson",
                table: "Words",
                newName: "Examples");

            migrationBuilder.AddColumn<string>(
                name: "ExampleDescriptions",
                table: "Words",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9d67e670-39fb-4658-a4d7-bef8ab0c9b67", "AQAAAAIAAYagAAAAEC4Dfbn4MwzV57WDLniBc8K55nqnEI8Zxmg1clfSUQqIk9WMzj0H00U9bTFa/mU5pA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1e067475-17a2-401e-85bd-0e34691c1c06", "AQAAAAIAAYagAAAAEFfp3DK6ACdeNh5PNgIXaRwIfid9swiwtO2jviQrcUyyJyx1Pe8UIy/Iixqc7dZzow==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "06194eb7-3820-436b-ae3e-8f5cf4cb49c5", "AQAAAAIAAYagAAAAEExSEkRoe9wA7vVpWGZqlQrSsHZk0htBYCP+FT0ncuKGRWemXDduzHj/vMtFL8EgHQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExampleDescriptions",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "Examples",
                table: "Words",
                newName: "ExamplesJson");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ef56fce9-f2e1-4fc1-90f0-986b6da8eb1e", "AQAAAAIAAYagAAAAEDRmXZs0gNf2fPMNjVF1R61X4gCnuxdaB+3WA36X4uCLB1qqGaidpp6PLU9mFi9sJQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95b7d3da-6e4c-4e72-af3e-7e1b485d3088", "AQAAAAIAAYagAAAAEHkPeOA+DjAXzdwkl9RIgVYytDTftjcHnrNhPvYne5F2FUC3fx3Q0fV2ZeH5Q8+wgw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7e5a16d1-131e-4e61-ac67-92cd3c504454", "AQAAAAIAAYagAAAAEDzPGK7efLoZ62FP9aeiD6JvT8uBF1r3CHkRLYPauAhbvXljMEKAAwFF7lJUS/c+ew==" });
        }
    }
}
