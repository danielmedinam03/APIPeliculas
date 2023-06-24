namespace ApiPeliculas.Models.Dtos.UsuarioDTOs
{
    public class AppUsuarioLoginRespuestaDTO
    {
        public string Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public IEnumerable<string> Rol { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
