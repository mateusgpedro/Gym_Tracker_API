using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangedFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowingId",
                table: "FollowUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers");

            migrationBuilder.DropIndex(
                name: "IX_FollowUsers_FollowingId",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "FollowUsers",
                newName: "FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers",
                columns: new[] { "FollowerId", "FollowedId" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUsers_FollowedId",
                table: "FollowUsers",
                column: "FollowedId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowerId",
                table: "FollowUsers",
                column: "FollowerId",
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers");

            migrationBuilder.DropIndex(
                name: "IX_FollowUsers_FollowedId",
                table: "FollowUsers");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "FollowUsers",
                newName: "FollowingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowUsers",
                table: "FollowUsers",
                columns: new[] { "FollowedId", "FollowingId" });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUsers_FollowingId",
                table: "FollowUsers",
                column: "FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUsers_AspNetUsers_FollowingId",
                table: "FollowUsers",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
