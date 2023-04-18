using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedFollowRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PendingStatus",
                table: "FollowUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingStatus",
                table: "FollowUsers");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "AspNetUsers");
        }
    }
}
