using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFieldsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowerId",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "FollowUsers",
                newName: "FollowedId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowedId",
                table: "FollowUsers",
                column: "FollowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowedId",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "FollowUsers",
                newName: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowerId",
                table: "FollowUsers",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
