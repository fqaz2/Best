using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addCampaignRatingV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Campaign");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Campaign",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
