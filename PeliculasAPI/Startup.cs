using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Servicios;

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


            //Agrego la conexión con la base de datos de SQL
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));           
            services.AddControllers().AddNewtonsoftJson();
            
            
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
