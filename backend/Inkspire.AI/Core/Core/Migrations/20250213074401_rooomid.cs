using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class rooomid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveRoomId",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ActiveRoomId", "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { null, "1337f0a6-3cd8-45e3-9579-a02577bfa0f7", "AQAAAAIAAYagAAAAEN8sY9Q5mMhhGLjS4iK4wX2VPJy24V9lYuEBKi9cn/5cOaSuutrAVDLaPWYfTaQtlA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ActiveRoomId", "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { null, "341b907b-74ba-412c-a497-f25829683e12", "AQAAAAIAAYagAAAAEJEQBkEeXqGe+f91YkceXxar6jTgGhG7QZfgVHyzV1eSAuZ5ZhlF/s3YVRJQj443AQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ActiveRoomId", "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { null, "12ad2d97-193b-419c-bfd5-b06af60f1d6e", "AQAAAAIAAYagAAAAEO1MF1nn1aiCGO9Ah/kkZI1110UCz665f9DnW+OJaV0zm9suUHDJIwbv9BBuH3xXig==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveRoomId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4f1155e5-9852-4d77-9834-1431d0def7f2", "AQAAAAIAAYagAAAAEIHXUI9uz2eabPLHnYMl2cm+21s1eoQiB7uPUEOp+o3UdA109DHLZIKIlS6IsRn6sw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c87be8fa-5228-4d98-b1fe-a6755618ebfd", "AQAAAAIAAYagAAAAEA09jzidRKo7EyiTzWut5mwE7ZrNkn/khNgn0duJqtZOsM64MspjwDFFiTG95OgGNQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "06d898ec-26bb-4f03-9e12-9bc611741c6a", "AQAAAAIAAYagAAAAENFUZD+T7XtuC/uGsgCHtNTL3RDMYJQCHulWtf5kPOjMoWocn9HTdUGa1+NkFchdwg==" });
        }
    }
}
