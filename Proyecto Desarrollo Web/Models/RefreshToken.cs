namespace Proyecto_Desarrollo_Web.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public Usuario user { get; set; }  // navegación opcional
    }
}

