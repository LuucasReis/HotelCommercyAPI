using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelComercyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaToVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillasNumber",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VillasNumber_VillaId",
                table: "VillasNumber",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillasNumber_Villas_VillaId",
                table: "VillasNumber",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillasNumber_Villas_VillaId",
                table: "VillasNumber");

            migrationBuilder.DropIndex(
                name: "IX_VillasNumber_VillaId",
                table: "VillasNumber");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillasNumber");
        }
    }
}
