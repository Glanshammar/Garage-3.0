using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleTypePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeName",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.RenameTable(
                name: "VehicleType",
                newName: "VehicleTypes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VehicleTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes",
                column: "Name");

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Name",
                keyValue: "Bus",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Name",
                keyValue: "Car",
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Name",
                keyValue: "Motorcycle",
                column: "Id",
                value: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_VehicleTypes_VehicleTypeName",
                table: "ParkedVehicles",
                column: "VehicleTypeName",
                principalTable: "VehicleTypes",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_VehicleTypes_VehicleTypeName",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VehicleTypes");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "VehicleType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeName",
                table: "ParkedVehicles",
                column: "VehicleTypeName",
                principalTable: "VehicleType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
