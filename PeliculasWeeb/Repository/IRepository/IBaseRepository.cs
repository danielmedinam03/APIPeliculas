namespace PeliculasWeb.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Metodo que trae una lista con toda la informacion de las entidades de la API
        /// </summary>
        /// <param name="url">Endpoint a consumir</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync(string url);
        /// <summary>
        /// Método que traer la información de un único valor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Id">ID de dato que se quiere traer</param>
        /// <returns></returns>
        public Task<T> GetByIdAsync(string url, int Id);
        /// <summary>
        /// Método que crea un nuevo valor
        /// </summary>
        /// <param name="url"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public Task<bool> AddAsync(string url, T entidad);
        /// <summary>
        /// Método que actualiza un valor anteriormente creado
        /// </summary>
        /// <param name="url"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(string url, T entidad);
        /// <summary>
        /// Método que elimina un valor especifico
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string url, int Id);
        /// <summary>
        ///  Metodo para buscar peliculas en categoria
        /// </summary>
        /// <param name="url"></param>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetPeliculasEnCategoriasAsync(string url, int categoriaId);
        /// <summary>
        /// Metodo para buscar por nombre
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Buscar(string url, string nombre);
    }
}
