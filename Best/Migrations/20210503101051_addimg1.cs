using Microsoft.EntityFrameworkCore.Migrations;

namespace Best.Migrations
{
    public partial class addimg1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaingCarousel");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Campaing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BestUserImg",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    BestUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestUserImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BestUserImg_AspNetUsers_BestUserId",
                        column: x => x.BestUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaingImg",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    CampaingId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaingImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaingImg_Campaing_CampaingId",
                        column: x => x.CampaingId,
                        principalTable: "Campaing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostImg",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    PostId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImg_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BestUserImg_BestUserId",
                table: "BestUserImg",
                column: "BestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaingImg_CampaingId",
                table: "CampaingImg",
                column: "CampaingId");

            migrationBuilder.CreateIndex(
                name: "IX_PostImg_PostId",
                table: "PostImg",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestUserImg");

            migrationBuilder.DropTable(
                name: "CampaingImg");

            migrationBuilder.DropTable(
                name: "PostImg");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "Campaing");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "CampaingCarousel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Alt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampaingId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaingCarousel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaingCarousel_Campaing_CampaingId",
                        column: x => x.CampaingId,
                        principalTable: "Campaing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaingCarousel_CampaingId",
                table: "CampaingCarousel",
                column: "CampaingId");
        }
    }
}
