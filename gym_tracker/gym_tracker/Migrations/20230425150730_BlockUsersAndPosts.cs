using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class BlockUsersAndPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockUser_AspNetUsers_BlockerId",
                table: "BlockUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockUser_AspNetUsers_BlockingId",
                table: "BlockUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockUser",
                table: "BlockUser");

            migrationBuilder.RenameTable(
                name: "BlockUser",
                newName: "BlockUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BlockUser_BlockingId",
                table: "BlockUsers",
                newName: "IX_BlockUsers_BlockingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockUsers",
                table: "BlockUsers",
                columns: new[] { "BlockerId", "BlockingId" });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Tag = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BlockUsers_AspNetUsers_BlockerId",
                table: "BlockUsers",
                column: "BlockerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockUsers_AspNetUsers_BlockingId",
                table: "BlockUsers",
                column: "BlockingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockUsers_AspNetUsers_BlockerId",
                table: "BlockUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockUsers_AspNetUsers_BlockingId",
                table: "BlockUsers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlockUsers",
                table: "BlockUsers");

            migrationBuilder.RenameTable(
                name: "BlockUsers",
                newName: "BlockUser");

            migrationBuilder.RenameIndex(
                name: "IX_BlockUsers_BlockingId",
                table: "BlockUser",
                newName: "IX_BlockUser_BlockingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlockUser",
                table: "BlockUser",
                columns: new[] { "BlockerId", "BlockingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlockUser_AspNetUsers_BlockerId",
                table: "BlockUser",
                column: "BlockerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockUser_AspNetUsers_BlockingId",
                table: "BlockUser",
                column: "BlockingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
