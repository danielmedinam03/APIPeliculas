namespace PeliculasWeb.Domain.DTOs
{
    public class UsuarioRegistroDTO
    {
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Rol { get; set; } = new List<string>();
    }
}
