using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsOccupied", "Location", "Size", "SpotNumber" },
                values: new object[,]
                {
                    { 1, false, "North", "Small", "A1" },
                    { 2, true, "South", "Medium", "B2" },
                    { 3, false, "East", "Large", "C3" }
                });

            migrationBuilder.InsertData(
                table: "VehicleType",
                column: "Name",
                values: new object[]
                {
                    "Bus",
                    "Car",
                    "Motorcycle"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Bus");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Car");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Motorcycle");
        }
    }
}
