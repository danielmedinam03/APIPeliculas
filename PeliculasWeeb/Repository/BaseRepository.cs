using Newtonsoft.Json;
using PeliculasWeb.Repository.IRepository;
using System.Text;
using System.Text.Json.Serialization;

namespace PeliculasWeb.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        //Inyeccion de depencias se debe importar el IHTTPClientFactory
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> AddAsync(string url, T entidad, string token ="")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);
            if (entidad != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _httpClientFactory.CreateClient();
            if (token!= null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> DeleteAsync(string url, int Id, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + Id);
            
            var cliente = _httpClientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetByIdAsync(string url, int Id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> UpdateAsync(string url, T entidad, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);
            if (entidad != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json"
                    );

            }
            else
            {
                return false;
            }

            var cliente = _httpClientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<T>> GetPeliculasEnCategoriasAsync(string url, int categoriaId)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + categoriaId);

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retornar los datos

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }

        }

        public async Task<IEnumerable<T>> Buscar(string url, string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + nombre);

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retornar los datos

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }

        }

    }
}
