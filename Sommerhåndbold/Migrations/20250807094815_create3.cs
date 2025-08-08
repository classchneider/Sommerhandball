using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SommerhåndboldFejlretning.Migrations
{
    /// <inheritdoc />
    public partial class create3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teamscores_Groups_GroupId",
                table: "Teamscores");

            migrationBuilder.DropForeignKey(
                name: "FK_Teamscores_Teams_TeamId",
                table: "Teamscores");

            migrationBuilder.AddForeignKey(
                name: "FK_Teamscores_Groups_GroupId",
                table: "Teamscores",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teamscores_Teams_TeamId",
                table: "Teamscores",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teamscores_Groups_GroupId",
                table: "Teamscores");

            migrationBuilder.DropForeignKey(
                name: "FK_Teamscores_Teams_TeamId",
                table: "Teamscores");

            migrationBuilder.AddForeignKey(
                name: "FK_Teamscores_Groups_GroupId",
                table: "Teamscores",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teamscores_Teams_TeamId",
                table: "Teamscores",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
