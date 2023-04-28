using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiPeliculas.Service.PeliculaService
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        public PeliculaService(IPeliculaRepository peliculaRepository, ICategoriaRepository categoriaRepository)
        {
            _peliculaRepository = peliculaRepository;
            _categoriaRepository = categoriaRepository; 
        }
        public async Task<int> AddAsync(Pelicula entity)
        {
            var data = await _peliculaRepository.AddAsync(entity);
            return entity.Id;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var data = await _peliculaRepository.DeleteAsync(x => x.Id==Id);
            return data;
        }

        public async Task<bool> ExistAsync(int id)
        {
            var data = await _peliculaRepository.ExistGenericAsync(x => x.Id == id);
            return data;
        }

        public async Task<bool> ExistNameAsync(string name)
        {
            var data = await _peliculaRepository.ExistGenericAsync(x => x.Nombre == name);
            return data;
        }

        public async Task<IEnumerable<Pelicula>> GetAllAsync()
        {
            var data = await _peliculaRepository.GenericGetAllAsync();
            return data;
        }

        public async Task<Pelicula> GetByIdAsync(int Id)
        {
            var data = await _peliculaRepository.GenericGetAsync(x => x, x => x.Id==Id);
            return data;
        }

        public Task<Pelicula> GetByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Pelicula>> GetPeliculasByCategoriaID(int IdCat)
        {
            var existCat = await _categoriaRepository.ExistGenericAsync(x=> x.ID==IdCat);
            if (!existCat)
            {
                return Enumerable.Empty<Pelicula>();
            }
            var data = await _peliculaRepository.GenericGetAllAsync(x => x.CategoriaId == IdCat,x => x.Include(x => x.Categoria));
            return data;
        }

        public async Task<IEnumerable<Pelicula>> GetPeliculasByName(string name)
        {
            List<Pelicula> data = new();
            if (!String.IsNullOrEmpty(name))
            {
                data = await _peliculaRepository.GenericGetAllAsync(x => x.Nombre.ToLower().Trim().Contains(name));
            }
            return data;
        }

        public async Task<bool> UpdateAsync(Pelicula entity)
        {
            var data = await _peliculaRepository.UpdateGenericAsync(entity);
            return data;
        }
    }
}
