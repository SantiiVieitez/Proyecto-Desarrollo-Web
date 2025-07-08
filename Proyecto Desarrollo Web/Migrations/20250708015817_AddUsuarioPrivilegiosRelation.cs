using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Desarrollo_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioPrivilegiosRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Privilegios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPrivilegios",
                columns: table => new
                {
                    PrivilegiosId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPrivilegios", x => new { x.PrivilegiosId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioPrivilegios_Privilegios_PrivilegiosId",
                        column: x => x.PrivilegiosId,
                        principalTable: "Privilegios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPrivilegios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPrivilegios_UsuarioId",
                table: "UsuarioPrivilegios",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioPrivilegios");

            migrationBuilder.DropTable(
                name: "Privilegios");
        }
    }
}
