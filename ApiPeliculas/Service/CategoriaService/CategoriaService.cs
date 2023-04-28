using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos.CategoriaDTOs;
using ApiPeliculas.Repository.IRepository;

namespace ApiPeliculas.Service.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository= categoriaRepository;
        }
        public async Task<int> AddAsync(Categoria entity)
        {
            var data = await _categoriaRepository.AddAsync(entity);
            return entity.ID;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            return await _categoriaRepository.DeleteAsync(x => x.ID==Id);

        }

        public async Task<bool> ExistAsync(int Id)
        {
            var data = await _categoriaRepository.ExistGenericAsync(x => x.ID==Id);
            return data;
        }

        public async Task<bool> ExistNameAsync(string name)
        {
            var data = await _categoriaRepository.ExistGenericAsync(x=>x.Nombre.ToLower().Trim() == name.ToLower().Trim());
            return data;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            var data = await _categoriaRepository.GenericGetAllAsync();
            return data;
        }

        public async Task<Categoria> GetByIdAsync(int Id)
        {
            var data = await _categoriaRepository.GenericGetAsync(x=>x,x => x.ID==Id);
            return data;
        }

        public Task<Categoria> GetByIdAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Categoria entity)
        {
            var data = await _categoriaRepository.UpdateGenericAsync(entity);
            return data;
        }
    }
}
