using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedParkingSpot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_ParkingSpot_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot");

            migrationBuilder.RenameTable(
                name: "ParkingSpot",
                newName: "ParkingSpots");

            migrationBuilder.RenameColumn(
                name: "row",
                table: "ParkingSpots",
                newName: "Row");

            migrationBuilder.RenameColumn(
                name: "column",
                table: "ParkingSpots",
                newName: "Column");

            migrationBuilder.AddColumn<bool>(
                name: "Occupied",
                table: "ParkingSpots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotId",
                table: "ParkingSpots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots");

            migrationBuilder.DropColumn(
                name: "Occupied",
                table: "ParkingSpots");

            migrationBuilder.DropColumn(
                name: "ParkingSpotId",
                table: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "ParkingSpots",
                newName: "ParkingSpot");

            migrationBuilder.RenameColumn(
                name: "Row",
                table: "ParkingSpot",
                newName: "row");

            migrationBuilder.RenameColumn(
                name: "Column",
                table: "ParkingSpot",
                newName: "column");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpot",
                table: "ParkingSpot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_ParkingSpot_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                principalTable: "ParkingSpot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
