using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addUsersToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7926), new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7920) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7932), new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7930) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7938), new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7936) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7944), new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7941) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7950), new DateTime(2024, 1, 8, 22, 7, 55, 243, DateTimeKind.Local).AddTicks(7947) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9906), new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9901) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9910), new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9908) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9914), new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9912) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9918), new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9916) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9921), new DateTime(2023, 12, 14, 22, 1, 35, 235, DateTimeKind.Local).AddTicks(9920) });
        }
    }
}
