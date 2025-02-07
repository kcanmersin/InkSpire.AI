using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class words11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Words",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Words");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "496c58e9-c4d4-4428-b95d-fdb37598c5d9", "AQAAAAIAAYagAAAAECdkN3VW39d5RbDIzxTkEqlhwyMr7jNBwyVn7hk0b8LXfcy0OBPEWoEcHK843sdqRw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cb901746-f667-4423-b094-40980ea8412d", "AQAAAAIAAYagAAAAEBUo6q1p+lmS+23Dh2H6cumv9jkFL0m8XFemqJ+7HjlqHGSuiSBjUZ4t2lIM/DMduA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1f26fed3-a1d0-4c78-a7b7-16272507e82c", "AQAAAAIAAYagAAAAEHsmH10X8eSxxV3CxFHdQvoZA0pomRwxbFjTv79HnChD/7mjF++aFBUYLS5axdigvQ==" });
        }
    }
}
