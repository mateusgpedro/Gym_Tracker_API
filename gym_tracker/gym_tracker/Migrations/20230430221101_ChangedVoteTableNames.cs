using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangedVoteTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote<Comment>_AspNetUsers_UserId",
                table: "Vote<Comment>");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote<Comment>_Comments_ItemId",
                table: "Vote<Comment>");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote<Post>_AspNetUsers_UserId",
                table: "Vote<Post>");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote<Post>_Posts_ItemId",
                table: "Vote<Post>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote<Post>",
                table: "Vote<Post>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote<Comment>",
                table: "Vote<Comment>");

            migrationBuilder.RenameTable(
                name: "Vote<Post>",
                newName: "PostVotes");

            migrationBuilder.RenameTable(
                name: "Vote<Comment>",
                newName: "CommentVotes");

            migrationBuilder.RenameIndex(
                name: "IX_Vote<Post>_UserId",
                table: "PostVotes",
                newName: "IX_PostVotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote<Post>_ItemId",
                table: "PostVotes",
                newName: "IX_PostVotes_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote<Comment>_UserId",
                table: "CommentVotes",
                newName: "IX_CommentVotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote<Comment>_ItemId",
                table: "CommentVotes",
                newName: "IX_CommentVotes_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostVotes",
                table: "PostVotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentVotes",
                table: "CommentVotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_AspNetUsers_UserId",
                table: "CommentVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_Comments_ItemId",
                table: "CommentVotes",
                column: "ItemId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostVotes_AspNetUsers_UserId",
                table: "PostVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostVotes_Posts_ItemId",
                table: "PostVotes",
                column: "ItemId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_AspNetUsers_UserId",
                table: "CommentVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_Comments_ItemId",
                table: "CommentVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostVotes_AspNetUsers_UserId",
                table: "PostVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostVotes_Posts_ItemId",
                table: "PostVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostVotes",
                table: "PostVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentVotes",
                table: "CommentVotes");

            migrationBuilder.RenameTable(
                name: "PostVotes",
                newName: "Vote<Post>");

            migrationBuilder.RenameTable(
                name: "CommentVotes",
                newName: "Vote<Comment>");

            migrationBuilder.RenameIndex(
                name: "IX_PostVotes_UserId",
                table: "Vote<Post>",
                newName: "IX_Vote<Post>_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostVotes_ItemId",
                table: "Vote<Post>",
                newName: "IX_Vote<Post>_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentVotes_UserId",
                table: "Vote<Comment>",
                newName: "IX_Vote<Comment>_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentVotes_ItemId",
                table: "Vote<Comment>",
                newName: "IX_Vote<Comment>_ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote<Post>",
                table: "Vote<Post>",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote<Comment>",
                table: "Vote<Comment>",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote<Comment>_AspNetUsers_UserId",
                table: "Vote<Comment>",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote<Comment>_Comments_ItemId",
                table: "Vote<Comment>",
                column: "ItemId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote<Post>_AspNetUsers_UserId",
                table: "Vote<Post>",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote<Post>_Posts_ItemId",
                table: "Vote<Post>",
                column: "ItemId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
