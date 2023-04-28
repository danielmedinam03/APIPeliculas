using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using ApiPeliculas.Service.AppUsuarioService;
using ApiPeliculas.Service.CategoriaService;
using ApiPeliculas.Service.PeliculaService;
using ApiPeliculas.Service.UsuarioService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Configuration connection to db
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//Soporte para autenticacion con .Net identity
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<Context>();

//Añadimos cache
builder.Services.AddResponseCaching();

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

//AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Configuracion de la autenticación
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Categoria GroupServices
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
//Peliculas GroupServices
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IPeliculaService, PeliculaService>();
//Usuario GroupServices
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
//AppUsuario GroupServices
builder.Services.AddScoped<IAppUsuarioRepository, AppUsuarioRepository>();
builder.Services.AddScoped<IAppUsuarioService, AppUsuarioService>();

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("PorDefecto20Seg", new CacheProfile() { Duration = 20 });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configurar para que se pueda autenticar desde SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description =
        "Autenticacion JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra 'Bearer' seguida de un espacio y despues su Token en el campo de abajo \r\n\r\n " +
        "Ejemplo: \"Bearer ashdgjashgdlasu\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Soporte para CORS
/*
 Se puedn habilitar 1: Dominio, 2: Varios dominios separados por comas,
3: cualquier dominio, teniendo en cuenta la seguridad
(*) Se usa para todos los dominios
 */
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PolicyCors");
//Se añade la autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
