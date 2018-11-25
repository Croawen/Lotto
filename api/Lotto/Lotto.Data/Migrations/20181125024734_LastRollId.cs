using Microsoft.EntityFrameworkCore.Migrations;

namespace Lotto.Data.Migrations
{
    public partial class LastRollId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasWon",
                table: "UserRolls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PreviousRollId",
                table: "Rolls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasWon",
                table: "UserRolls");

            migrationBuilder.DropColumn(
                name: "PreviousRollId",
                table: "Rolls");
        }
    }
}
