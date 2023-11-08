using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class Collaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorName",
                table: "Collaborators");

            migrationBuilder.AddColumn<string>(
                name: "CollaboratorEmail",
                table: "Collaborators",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollaboratorEmail",
                table: "Collaborators");

            migrationBuilder.AddColumn<string>(
                name: "CollaboratorName",
                table: "Collaborators",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
