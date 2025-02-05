using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class nullusetest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Tests",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c6c06ce7-0fd2-4717-8269-354911a47be2", "AQAAAAIAAYagAAAAEHH+18HLpj4i56T1VW73NSQFnkUSASEQ8jmT5ijFk/tF29SVvNEFqL4pjD+ktRM1Ag==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a8fa023a-95b6-4cf8-ab1d-bc2a53576efd", "AQAAAAIAAYagAAAAEGVqmVkfBfx9bXEC+PCUc2tnn+GV6lmheneb1+q0vFW0ft05moNAt5WF9nemVOMv9w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "245abcbb-7a6c-4ded-8ebe-e8240335bae7", "AQAAAAIAAYagAAAAENIQCDLvkbEaHY9GmPojN9shoTFgI9yTEa/d2PylyFFpBQYi5LWjlH8BuvQDsp6a0g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Tests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

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
    }
}
