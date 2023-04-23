using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gym_tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedBlockUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockUser",
                columns: table => new
                {
                    BlockerId = table.Column<string>(type: "text", nullable: false),
                    BlockingId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockUser", x => new { x.BlockerId, x.BlockingId });
                    table.ForeignKey(
                        name: "FK_BlockUser_AspNetUsers_BlockerId",
                        column: x => x.BlockerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockUser_AspNetUsers_BlockingId",
                        column: x => x.BlockingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockUser_BlockingId",
                table: "BlockUser",
                column: "BlockingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockUser");
        }
    }
}
