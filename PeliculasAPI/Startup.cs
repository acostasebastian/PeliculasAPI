using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;
using System.Text;

namespace PeliculasAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Agrego Automapper para mapear los DTO
            services.AddAutoMapper(typeof(Startup));

            //Agrego el servicio de Azure
            //services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();

            //Agrego el servicio de Guardado en local           
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            services.AddHttpContextAccessor(); //este es para local solamente

            //Se agrega el servicio para calcular la Latitud y Longitud
            //el numero representa al sistema de coordenadas usado en la tierra)
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            //para agregar el filtro de si existe la pelicula
            services.AddScoped<PeliculaExisteAttribute>();

            //se agrega esto para el mapper, debido a que en AutomapperProfile se agregó el GeometryFactory para pasarselo (Injeccion de Dependencia)
            services.AddSingleton(provider =>            
                new MapperConfiguration(config =>
                {
                    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                    config.AddProfile(new AutoMapperProfiles(geometryFactory));
                }).CreateMapper()
            );


            //Agrego la conexión con la base de datos de SQL
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"),
              sqlServerOptions => sqlServerOptions.UseNetTopologySuite() //para usar la localizacion, con la libreria Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite
              ));

            //Para usar el servicio de NewtonsoftJson
            services.AddControllers().AddNewtonsoftJson();

            //Para poder usar el sistema de Usuarios de Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                       ClockSkew = TimeSpan.Zero
                   }
               );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();               
            }

            app.UseHttpsRedirection();


            app.UseStaticFiles(); //esto se agregar para que al ver en el navegador la url de una imagen guardada en BD, la podamos ver

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
