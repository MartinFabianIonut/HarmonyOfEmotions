using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarmonyOfEmotions.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTrackPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTrackPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrackId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrackPreferences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTrackPreferences_UserId_TrackId",
                table: "UserTrackPreferences",
                columns: new[] { "UserId", "TrackId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [TrackId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTrackPreferences");
        }
    }
}
