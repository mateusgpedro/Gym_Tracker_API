using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedFollowProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FollowUser_Id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUser",
                table: "FollowUser");

            migrationBuilder.RenameTable(
                name: "FollowUser",
                newName: "FollowUsers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FollowUsers_Id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers");

            migrationBuilder.RenameTable(
                name: "FollowUsers",
                newName: "FollowUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowUser",
                table: "FollowUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FollowUser_Id",
                table: "AspNetUsers",
                column: "Id",
                principalTable: "FollowUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
