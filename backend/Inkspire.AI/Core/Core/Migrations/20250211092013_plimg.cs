using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class plimg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl" },
                values: new object[] { "72343990-2959-4774-942e-5bd38fc8368c", "AQAAAAIAAYagAAAAECXY3GMJlnvzeU7qPAovcXDxmTpWaC3Qg/y5DV5hodqmB5jXi4IUXgumRf9a9oj13w==", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl" },
                values: new object[] { "026b2a1e-4e98-4aee-b477-7769fbc75fad", "AQAAAAIAAYagAAAAEEn6bPO+k0IJ80jYJuQ5HW8sTz3TNiyFNxH9hN6n8v/jDJL13siZLufn1SZNe7rgmA==", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfileImageUrl" },
                values: new object[] { "0e4e6029-c6c6-470a-9aaa-02d107a6b0f2", "AQAAAAIAAYagAAAAEEig02YxuMGNwxytew207lSQTIp9uF3VmgiR6BaHitm3g2mDxXHFvGRiRjEZ6KbJZQ==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f66b6f67-f48d-4bf8-8d8f-d9453ea45bb8", "AQAAAAIAAYagAAAAEBlP2N6l6zVUqYW4WZE4oSy7fsUMQY6fAeLdZZOgvhJUZ7EIp9ttYniYV2ZuWk4/Vw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9356c3f1-968a-4819-a94f-3a40f095c704", "AQAAAAIAAYagAAAAEDN+EbDPbqaNQND9aIRLN0zQIjdi8KKG6WJ+Ua9vCZ0vw13l0+VHFWI0Ni7UQ1XgJw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7dd1aee9-8e36-4781-8566-745394cb033e", "AQAAAAIAAYagAAAAEA5ig8aypdK4EIjDORI1nmm/NprsYNtAAIKDVOC8tr4vVqHn98z71pOEBw/2WWAGyw==" });
        }
    }
}
