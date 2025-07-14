using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Desarrollo_Web.Migrations
{
    /// <inheritdoc />
    public partial class FixUsuariosPrivilegiosPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioPrivilegios");

            migrationBuilder.CreateTable(
                name: "UsuariosPrivilegios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PrivilegiosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosPrivilegios", x => new { x.UsuarioId, x.PrivilegiosId });
                    table.ForeignKey(
                        name: "FK_UsuariosPrivilegios_Privilegios_PrivilegiosId",
                        column: x => x.PrivilegiosId,
                        principalTable: "Privilegios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosPrivilegios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosPrivilegios_PrivilegiosId",
                table: "UsuariosPrivilegios",
                column: "PrivilegiosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosPrivilegios");

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
    }
}
