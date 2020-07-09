using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskPublisher.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "packets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    message = table.Column<string>(nullable: true),
                    sendDate = table.Column<DateTime>(nullable: false),
                    hash = table.Column<string>(nullable: true),
                    sended = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packets", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "packets",
                columns: new[] { "id", "hash", "message", "sendDate", "sended" },
                values: new object[] { 1, null, "message 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "packets",
                columns: new[] { "id", "hash", "message", "sendDate", "sended" },
                values: new object[] { 2, null, "message 2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "packets",
                columns: new[] { "id", "hash", "message", "sendDate", "sended" },
                values: new object[] { 3, null, "message 3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "packets",
                columns: new[] { "id", "hash", "message", "sendDate", "sended" },
                values: new object[] { 4, null, "message 4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "packets");
        }
    }
}
