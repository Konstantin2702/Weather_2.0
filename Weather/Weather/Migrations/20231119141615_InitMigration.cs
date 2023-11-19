using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfTaking = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOfTaking = table.Column<TimeSpan>(type: "time", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: false),
                    DewPoint = table.Column<float>(type: "real", nullable: false),
                    Pressure = table.Column<int>(type: "int", nullable: false),
                    WindDirection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WindSpeed = table.Column<int>(type: "int", nullable: true),
                    Cloudiness = table.Column<int>(type: "int", nullable: true),
                    CloudBase = table.Column<int>(type: "int", nullable: false),
                    HorizontalVisibility = table.Column<int>(type: "int", nullable: true),
                    WeatherConditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherInfos_WeatherConditions_WeatherConditionId",
                        column: x => x.WeatherConditionId,
                        principalTable: "WeatherConditions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherInfos_WeatherConditionId",
                table: "WeatherInfos",
                column: "WeatherConditionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherInfos");

            migrationBuilder.DropTable(
                name: "WeatherConditions");
        }
    }
}
