using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class VehicleParkingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSpotId",
                table: "ParkedVehicles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                unique: true,
                filter: "[ParkingSpotId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSpotId",
                table: "ParkedVehicles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicles",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
