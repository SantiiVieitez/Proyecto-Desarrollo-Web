namespace Proyecto_Desarrollo_Web.Models
{
    public class Privilegio
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public ICollection<UsuariosPrivilegios> Usuarios { get; set; } = new List<UsuariosPrivilegios>();
    }
}
