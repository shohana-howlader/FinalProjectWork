using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class kk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_State_StateID",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_State_Countries_CountryID",
                table: "State");

            migrationBuilder.DropPrimaryKey(
                name: "PK_State",
                table: "State");

            migrationBuilder.RenameTable(
                name: "State",
                newName: "States");

            migrationBuilder.RenameIndex(
                name: "IX_State_CountryID",
                table: "States",
                newName: "IX_States_CountryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_States",
                table: "States",
                column: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_States_StateID",
                table: "Locations",
                column: "StateID",
                principalTable: "States",
                principalColumn: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Countries_CountryID",
                table: "States",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_States_StateID",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Countries_CountryID",
                table: "States");

            migrationBuilder.DropPrimaryKey(
                name: "PK_States",
                table: "States");

            migrationBuilder.RenameTable(
                name: "States",
                newName: "State");

            migrationBuilder.RenameIndex(
                name: "IX_States_CountryID",
                table: "State",
                newName: "IX_State_CountryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_State",
                table: "State",
                column: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_State_StateID",
                table: "Locations",
                column: "StateID",
                principalTable: "State",
                principalColumn: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_State_Countries_CountryID",
                table: "State",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
