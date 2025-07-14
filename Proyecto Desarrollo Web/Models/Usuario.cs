namespace Proyecto_Desarrollo_Web.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClaveHash { get; set; }
        public string Salt { get; set; }
        public bool Activo { get; set; }

        public ICollection<UsuariosPrivilegios> Privilegios { get; set; } = new List<UsuariosPrivilegios>();
    }
}
