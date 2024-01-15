using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatedImageToLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Likes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Likes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Images_ImageId",
                table: "Likes",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
