using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class correctionoffielderrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mintext",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Post",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "createData",
                table: "Post",
                newName: "CreateData");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "CampaignRating",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Campaign",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "shortText",
                table: "Campaign",
                newName: "ShortText");

            migrationBuilder.RenameColumn(
                name: "createData",
                table: "Campaign",
                newName: "CreateData");

            migrationBuilder.AddColumn<string>(
                name: "ShortText",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortText",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Post",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Post",
                newName: "createData");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "CampaignRating",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Campaign",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "ShortText",
                table: "Campaign",
                newName: "shortText");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Campaign",
                newName: "createData");

            migrationBuilder.AddColumn<string>(
                name: "mintext",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
