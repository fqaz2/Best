using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addTopicDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Topic",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Topic");
        }
    }
}
