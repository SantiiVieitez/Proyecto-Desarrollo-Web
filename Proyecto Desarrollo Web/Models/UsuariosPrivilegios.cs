namespace Proyecto_Desarrollo_Web.Models
{
    public class UsuariosPrivilegios
    {
        public int UsuarioId { get; set; }
        public int PrivilegiosId { get; set; }

        public Usuario Usuario { get; set; }
        public Privilegio Privilegio { get; set; }
    }
}
