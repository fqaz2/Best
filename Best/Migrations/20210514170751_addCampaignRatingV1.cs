using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addCampaignRatingV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignRating",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    rating = table.Column<int>(nullable: false),
                    CampaignId = table.Column<string>(nullable: true),
                    BestUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignRating_AspNetUsers_BestUserId",
                        column: x => x.BestUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignRating_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRating_BestUserId",
                table: "CampaignRating",
                column: "BestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRating_CampaignId",
                table: "CampaignRating",
                column: "CampaignId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignRating");
        }
    }
}
