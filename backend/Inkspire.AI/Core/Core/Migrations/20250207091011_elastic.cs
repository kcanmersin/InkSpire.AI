using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class elastic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
