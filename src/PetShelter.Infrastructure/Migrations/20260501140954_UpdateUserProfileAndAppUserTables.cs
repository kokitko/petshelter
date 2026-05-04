using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetShelter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProfileAndAppUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "AppUsers");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
