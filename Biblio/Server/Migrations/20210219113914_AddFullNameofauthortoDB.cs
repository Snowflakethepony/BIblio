using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblio.Server.Migrations
{
    public partial class AddFullNameofauthortoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Authors");
        }
    }
}
