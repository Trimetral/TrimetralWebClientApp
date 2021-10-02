using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientsWebApp.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientName",
                table: "Clients",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_INN",
                table: "Clients",
                column: "INN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_ClientName",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_INN",
                table: "Clients");
        }
    }
}
