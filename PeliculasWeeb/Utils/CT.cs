namespace PeliculasWeb.Utils
{
    public static class CT
    {
        public static string UrlBaseApi = "https://localhost:7189/";
        public static string RutaCategoriasApi = UrlBaseApi + "api/Categorias/";
        public static string RutaPeliculasApi = UrlBaseApi + "api/Peliculas/";
        //public static string RutaUsuariosApi = UrlBaseApi + "api/NetIdentity/AppUsuarios/";
        public static string RutaUsuariosApi = UrlBaseApi + "api/Usuarios/";

        //Rutas para filtrar y bsucar 

        public static string RutaPeliculasEnCategoriasApi = UrlBaseApi + "api/Peliculas/Categoria/";
        public static string RutaPeliculasApiBusqueda = UrlBaseApi + "api/Peliculas/Nombre/";

    }
}
