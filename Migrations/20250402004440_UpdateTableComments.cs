using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIBLIOTECA_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UsuarioId",
                table: "Comments",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UsuarioId",
                table: "Comments",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UsuarioId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UsuarioId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Comments");
        }
    }
}
