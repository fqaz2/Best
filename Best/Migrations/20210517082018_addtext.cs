using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addtext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleImg",
                table: "Campaign");

            migrationBuilder.AddColumn<string>(
                name: "shortText",
                table: "Campaign",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "Campaign",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shortText",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "text",
                table: "Campaign");

            migrationBuilder.AddColumn<string>(
                name: "TitleImg",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
