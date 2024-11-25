using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingCostToParkingSpot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.AddColumn<decimal>(
                name: "ParkingCost",
                table: "ParkingSpots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1,
                column: "ParkingCost",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2,
                column: "ParkingCost",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3,
                column: "ParkingCost",
                value: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.DropColumn(
                name: "ParkingCost",
                table: "ParkingSpots");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId");
        }
    }
}
