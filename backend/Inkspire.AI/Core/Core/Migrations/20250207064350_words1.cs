using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class words1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d7d7d7d7-d7d7-d7d7-d7d7-d7d7d7d7d7d7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c260ce31-4905-464e-a7f0-8b2d001afafc", "AQAAAAIAAYagAAAAEFt4ud4HqrMcenHyQ+yUPNldN2rypslFcS3ADuKwZMudq5JaTp4dEsIV38nHVvZ0Eg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e7e7e7e7-e7e7-e7e7-e7e7-e7e7e7e7e7e7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bc7e9632-e839-43a8-8776-9d93e52844ad", "AQAAAAIAAYagAAAAEAiHMAcERR3KIaatFp4+SKm977Ek9AIuvtVHjJ5yh29kN0Cb5n+P1wmaIf9S9YJPog==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "11c5c98d-4747-49a3-82b6-bd42b7fe5fe1", "AQAAAAIAAYagAAAAEFsS6vHJCFyO2xm1KDdVebDeiJF1z6YUWyqsswWSh3ko+J6OEsIiPicHCCgUuvgR4g==" });
        }
    }
}
