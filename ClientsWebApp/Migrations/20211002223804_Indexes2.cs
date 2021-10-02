using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientsWebApp.Migrations
{
    public partial class Indexes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Founders_INN",
                table: "Founders",
                column: "INN");

            migrationBuilder.CreateIndex(
                name: "IX_Founders_NameSurname",
                table: "Founders",
                column: "NameSurname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Founders_INN",
                table: "Founders");

            migrationBuilder.DropIndex(
                name: "IX_Founders_NameSurname",
                table: "Founders");
        }
    }
}
