using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class version12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "Beneficiarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "Beneficiarios");
        }
    }
}
