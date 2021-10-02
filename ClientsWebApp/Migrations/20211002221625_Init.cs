using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientsWebApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Type = table.Column<bool>(type: "bit", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Founders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INN = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NameSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Founders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientFounder",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    FoundersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFounder", x => new { x.ClientsId, x.FoundersId });
                    table.ForeignKey(
                        name: "FK_ClientFounder_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFounder_Founders_FoundersId",
                        column: x => x.FoundersId,
                        principalTable: "Founders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFounder_FoundersId",
                table: "ClientFounder",
                column: "FoundersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFounder");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Founders");
        }
    }
}
