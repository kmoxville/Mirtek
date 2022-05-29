using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssFeedAggregator.DAL.Migrations
{
    public partial class AddIndexFeedSourceEntityUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "news_sources",
                type: "varchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2048)",
                oldMaxLength: 2048)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "original_url",
                table: "news_sources",
                type: "varchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2048)",
                oldMaxLength: 2048)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_news_sources_url",
                table: "news_sources",
                column: "url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_news_sources_url",
                table: "news_sources");

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "news_sources",
                type: "varchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(512)",
                oldMaxLength: 512)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "original_url",
                table: "news_sources",
                type: "varchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(512)",
                oldMaxLength: 512)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
