using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class rooomid1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4ab6f032-4c40-456d-b2b3-ab758bd07608", "AQAAAAIAAYagAAAAEEgL2rxVCFlkpa8ZZe01GVnZFCZzJNOtAHKWeWRhI9sy+PHdII7N+ub/M7E+HBl7hg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "57972f35-deb9-46cd-9c51-d735e7f9d503", "AQAAAAIAAYagAAAAEJnV2k4lbov9vHW5QKfFO4nprYXhYMp28/So3V+2KDPEGXiYqw6qWcEohJzlPtrwWw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b23eba8b-066d-4016-b871-b2f9b7ccb0f1", "AQAAAAIAAYagAAAAEKkQRDq65ERKZBJZb/ksFxIRN29uluRMDFN7AwGEyO1IrqPbejxzVrEarY70ZRryBw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1337f0a6-3cd8-45e3-9579-a02577bfa0f7", "AQAAAAIAAYagAAAAEN8sY9Q5mMhhGLjS4iK4wX2VPJy24V9lYuEBKi9cn/5cOaSuutrAVDLaPWYfTaQtlA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "341b907b-74ba-412c-a497-f25829683e12", "AQAAAAIAAYagAAAAEJEQBkEeXqGe+f91YkceXxar6jTgGhG7QZfgVHyzV1eSAuZ5ZhlF/s3YVRJQj443AQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "12ad2d97-193b-419c-bfd5-b06af60f1d6e", "AQAAAAIAAYagAAAAEO1MF1nn1aiCGO9Ah/kkZI1110UCz665f9DnW+OJaV0zm9suUHDJIwbv9BBuH3xXig==" });
        }
    }
}
