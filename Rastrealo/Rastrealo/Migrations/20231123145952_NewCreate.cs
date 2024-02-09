using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastrealo.Migrations
{
    public partial class NewCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "shiftId",
                table: "route",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "shift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shiftName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    busId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    driverId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shift", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shift");

            migrationBuilder.DropColumn(
                name: "shiftId",
                table: "route");
        }
    }
}
