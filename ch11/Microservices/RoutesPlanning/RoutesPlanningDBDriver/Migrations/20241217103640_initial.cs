using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace RoutesPlanningDBDriver.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutputQueueItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageCode = table.Column<int>(type: "int", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReadyTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputQueueItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<LineString>(type: "geography", nullable: false),
                    When = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Source_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Source_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source_Location = table.Column<Point>(type: "geography", nullable: false),
                    Destination_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Destination_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination_Location = table.Column<Point>(type: "geography", nullable: false),
                    WhenStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false),
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteRequests_RouteOffers_RouteId",
                        column: x => x.RouteId,
                        principalTable: "RouteOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutputQueueItems_ReadyTime",
                table: "OutputQueueItems",
                column: "ReadyTime");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOffers_Status",
                table: "RouteOffers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOffers_When",
                table: "RouteOffers",
                column: "When");

            migrationBuilder.CreateIndex(
                name: "IX_RouteRequests_RouteId",
                table: "RouteRequests",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteRequests_WhenEnd",
                table: "RouteRequests",
                column: "WhenEnd");

            migrationBuilder.CreateIndex(
                name: "IX_RouteRequests_WhenStart",
                table: "RouteRequests",
                column: "WhenStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutputQueueItems");

            migrationBuilder.DropTable(
                name: "RouteRequests");

            migrationBuilder.DropTable(
                name: "RouteOffers");
        }
    }
}
