using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Models
{
    //Clase para dar soporte al .Net identity
    public class AppUsuario : IdentityUser
    {
        //Añadir campos personalizados
        public string Nombre { get; set; }

    }
}
