using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddArrivalTimeToParkedVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "VehicleDetailsViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ParkingDuration",
                table: "VehicleDetailsViewModel",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ParkingSpotNumber",
                table: "VehicleDetailsViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalParkingCost",
                table: "VehicleDetailsViewModel",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "ParkedVehicles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "ParkedVehicleCreateViewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "VehicleDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "ParkingDuration",
                table: "VehicleDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "ParkingSpotNumber",
                table: "VehicleDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "TotalParkingCost",
                table: "VehicleDetailsViewModel");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "ParkedVehicles");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "ParkedVehicleCreateViewModel");
        }
    }
}
