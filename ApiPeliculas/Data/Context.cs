using ApiPeliculas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class Context : IdentityDbContext<AppUsuario>
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        //Debe crearse para la AppUsuario
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Add models here
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<AppUsuario> AppUsuario { get; set; }

    }
}
