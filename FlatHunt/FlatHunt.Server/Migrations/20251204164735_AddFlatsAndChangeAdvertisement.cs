using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatHunt.Server.Migrations.Parser
{
    /// <inheritdoc />
    public partial class AddFlatsAndChangeAdvertisement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_FlatSources_FlatSourceId",
                table: "Advertisement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advertisement",
                table: "Advertisement");

            migrationBuilder.RenameTable(
                name: "Advertisement",
                newName: "Advertisements");

            migrationBuilder.RenameIndex(
                name: "IX_Advertisement_FlatSourceId",
                table: "Advertisements",
                newName: "IX_Advertisements_FlatSourceId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "Advertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advertisements",
                table: "Advertisements",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Flats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    FloorCount = table.Column<int>(type: "int", nullable: false),
                    AreaTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flats", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_FlatId",
                table: "Advertisements",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_FlatSources_FlatSourceId",
                table: "Advertisements",
                column: "FlatSourceId",
                principalTable: "FlatSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Flats_FlatId",
                table: "Advertisements",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_FlatSources_FlatSourceId",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Flats_FlatId",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advertisements",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_FlatId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Advertisements");

            migrationBuilder.RenameTable(
                name: "Advertisements",
                newName: "Advertisement");

            migrationBuilder.RenameIndex(
                name: "IX_Advertisements_FlatSourceId",
                table: "Advertisement",
                newName: "IX_Advertisement_FlatSourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advertisement",
                table: "Advertisement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_FlatSources_FlatSourceId",
                table: "Advertisement",
                column: "FlatSourceId",
                principalTable: "FlatSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
