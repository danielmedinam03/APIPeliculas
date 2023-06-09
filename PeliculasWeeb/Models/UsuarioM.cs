﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class UsuarioM
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "el usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [StringLength(12,MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 12 caracteres")]
        public string Nombre { get; set; }
        public IEnumerable<string> Rol { get; set; } = new List<string>();
        public string Password { get; set; }
        public string? token { get; set; }
    }
}
