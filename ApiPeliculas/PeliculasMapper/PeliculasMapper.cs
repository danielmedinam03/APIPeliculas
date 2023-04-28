using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.CategoriaDTOs;
using ApiPeliculas.Models.Dtos.PeliculaDTOs;
using ApiPeliculas.Models.Dtos.UsuarioDTOs;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaCreateDTO>().ReverseMap();
            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioRepuestaLoginDTO>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatoDTO>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDTO>().ReverseMap();
        }
    }
}
