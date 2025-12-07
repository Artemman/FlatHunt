using FlatHunt.Server.Migrations.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatHunt.Server.Migrations.Parser
{
    /// <inheritdoc />
    public partial class AddCityModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LunCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GeoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeoType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunCities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LunCityId = table.Column<int>(type: "int", nullable: true),
                    FlatFyCityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_LunCities_LunCityId",
                        column: x => x.LunCityId,
                        principalTable: "LunCities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_LunCityId",
                table: "Cities",
                column: "LunCityId");

            migrationBuilder.SqlFile("Migrations/Scripts/SeedCities.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "LunCities");
        }
    }
}
