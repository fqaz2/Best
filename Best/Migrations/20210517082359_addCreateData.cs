using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addCreateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createData",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "createData",
                table: "Campaign",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createData",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "createData",
                table: "Campaign");
        }
    }
}
