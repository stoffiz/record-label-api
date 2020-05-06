using Microsoft.EntityFrameworkCore.Migrations;

namespace RecordLabelApi.Migrations
{
    public partial class AddFrontCoverToRelease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FrontCover",
                table: "Releases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontCover",
                table: "Releases");
        }
    }
}
