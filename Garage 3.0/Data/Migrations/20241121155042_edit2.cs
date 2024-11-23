using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class edit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_AspNetUsers_ApplicationUserId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_VehicleTypes_VehicleTypeName",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicleIndexViewModel_ApplicationUserId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicleIndexViewModel_ParkingSpotId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicleIndexViewModel_VehicleTypeName",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropColumn(
                name: "ParkingSpotId",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleTypeName",
                table: "ParkedVehicleIndexViewModel",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "ParkedVehicleIndexViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParkingSpotNumber",
                table: "ParkedVehicleIndexViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropColumn(
                name: "ParkingSpotNumber",
                table: "ParkedVehicleIndexViewModel");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleTypeName",
                table: "ParkedVehicleIndexViewModel",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ParkedVehicleIndexViewModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotId",
                table: "ParkedVehicleIndexViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicleIndexViewModel_ApplicationUserId",
                table: "ParkedVehicleIndexViewModel",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicleIndexViewModel_ParkingSpotId",
                table: "ParkedVehicleIndexViewModel",
                column: "ParkingSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicleIndexViewModel_VehicleTypeName",
                table: "ParkedVehicleIndexViewModel",
                column: "VehicleTypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_AspNetUsers_ApplicationUserId",
                table: "ParkedVehicleIndexViewModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_ParkingSpots_ParkingSpotId",
                table: "ParkedVehicleIndexViewModel",
                column: "ParkingSpotId",
                principalTable: "ParkingSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicleIndexViewModel_VehicleTypes_VehicleTypeName",
                table: "ParkedVehicleIndexViewModel",
                column: "VehicleTypeName",
                principalTable: "VehicleTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
