using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlatHunt.Server.Migrations.Parser
{
    /// <inheritdoc />
    public partial class CreateSourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.Sql(@"INSERT INTO FlatSources(Name, Type) VALUES ('Lun', 1), ('FlatFy', 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlatSources");
        }
    }
}
