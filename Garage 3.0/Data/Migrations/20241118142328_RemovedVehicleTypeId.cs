using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedVehicleTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VehicleType");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "ParkedVehicles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VehicleType",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "VehicleTypeName",
                table: "ParkedVehicles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_VehicleTypeName",
                table: "ParkedVehicles",
                column: "VehicleTypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeName",
                table: "ParkedVehicles",
                column: "VehicleTypeName",
                principalTable: "VehicleType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeName",
                table: "ParkedVehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType");

            migrationBuilder.DropIndex(
                name: "IX_ParkedVehicles_VehicleTypeName",
                table: "ParkedVehicles");

            migrationBuilder.DropColumn(
                name: "VehicleTypeName",
                table: "ParkedVehicles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "VehicleType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VehicleType",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "ParkedVehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleType",
                table: "VehicleType",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ParkedVehicles_VehicleTypeId",
                table: "ParkedVehicles",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkedVehicles_VehicleType_VehicleTypeId",
                table: "ParkedVehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
