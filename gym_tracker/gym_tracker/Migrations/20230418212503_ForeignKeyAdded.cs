using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FollowUsers_Id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FollowUsers",
                newName: "FollowingId");

            migrationBuilder.AddColumn<string>(
                name: "FollowerId",
                table: "FollowUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers",
                columns: new[] { "FollowerId", "FollowingId" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUsers_FollowingId",
                table: "FollowUsers",
                column: "FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowerId",
                table: "FollowUsers",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowingId",
                table: "FollowUsers",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowerId",
                table: "FollowUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowingId",
                table: "FollowUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers");

            migrationBuilder.DropIndex(
                name: "IX_FollowUsers_FollowingId",
                table: "FollowUsers");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "FollowUsers",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FollowUsers_Id",
                table: "AspNetUsers",
                column: "Id",
                principalTable: "FollowUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
