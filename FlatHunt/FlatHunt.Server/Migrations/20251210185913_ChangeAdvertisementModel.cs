using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatHunt.Server.Migrations.Parser
{
    /// <inheritdoc />
    public partial class ChangeAdvertisementModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_FlatSources_FlatSourceId",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "FlatSources");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_FlatSourceId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "FlatSourceId",
                table: "Advertisements",
                newName: "FlatSourceType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlatSourceType",
                table: "Advertisements",
                newName: "FlatSourceId");

            migrationBuilder.CreateTable(
                name: "FlatSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatSources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_FlatSourceId",
                table: "Advertisements",
                column: "FlatSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_FlatSources_FlatSourceId",
                table: "Advertisements",
                column: "FlatSourceId",
                principalTable: "FlatSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
