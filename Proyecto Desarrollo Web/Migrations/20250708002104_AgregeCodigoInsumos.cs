using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Desarrollo_Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregeCodigoInsumos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Insumos");
        }
    }
}
