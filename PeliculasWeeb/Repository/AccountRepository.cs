using Newtonsoft.Json;
using PeliculasWeb.Models;
using PeliculasWeb.Repository.IRepository;
using System.Net.Http;
using System.Text;

namespace PeliculasWeb.Repository
{
    public class AccountRepository : BaseRepository<UsuarioM>, IAccountRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UsuarioM> LoginAsync(string url, UsuarioM itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (itemCrear!=null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear),Encoding.UTF8,"application/json"
                );
            }
            else
            {
                return new UsuarioM();
            }

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioM>(jsonString);
            }
            else
            {
                return new UsuarioM();
            }

        }

        public async Task<bool> RegisterAsync(string url, UsuarioM itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (itemCrear != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json"
                );
            }
            else
            {
                return false;
            }

            var cliente = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
