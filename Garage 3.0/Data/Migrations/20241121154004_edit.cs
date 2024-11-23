using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_3._0.Data.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ParkedVehicleCreateViewModel");

            migrationBuilder.CreateTable(
                name: "MemberOverviewViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfRegisteredVehicles = table.Column<int>(type: "int", nullable: false),
                    TotalParkingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberOverviewViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkedVehicleIndexViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleTypeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParkingSpotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkedVehicleIndexViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkedVehicleIndexViewModel_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkedVehicleIndexViewModel_ParkingSpots_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkingSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkedVehicleIndexViewModel_VehicleTypes_VehicleTypeName",
                        column: x => x.VehicleTypeName,
                        principalTable: "VehicleTypes",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDetailsViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentParkingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MemberOverviewViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDetailsViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleDetailsViewModel_MemberOverviewViewModel_MemberOverviewViewModelId",
                        column: x => x.MemberOverviewViewModelId,
                        principalTable: "MemberOverviewViewModel",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetailsViewModel_MemberOverviewViewModelId",
                table: "VehicleDetailsViewModel",
                column: "MemberOverviewViewModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkedVehicleIndexViewModel");

            migrationBuilder.DropTable(
                name: "VehicleDetailsViewModel");

            migrationBuilder.DropTable(
                name: "MemberOverviewViewModel");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ParkedVehicleCreateViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
