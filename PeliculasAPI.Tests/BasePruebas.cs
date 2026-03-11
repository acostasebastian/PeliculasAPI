using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using PeliculasAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PeliculasAPI.Tests
{
    public class BasePruebas
    {
        protected string usuarioPorDefectoId = "9722b56a-77ea-4e41-941d-e319b6eb3712";
        protected string usuarioPorDefectoEmail = "ejemplo@hotmail.com";

        //Simula el DbContext
        protected ApplicationDbContext ConstruirContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nombreDB).Options;

            var dbContext = new ApplicationDbContext(opciones);
            return dbContext;
        }

        //Simula El AutoMapper
        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfiles(geometryFactory));
            });

            return config.CreateMapper();
        }

        //Simula crear claims de usuario
        protected ControllerContext ConstruirControllerContext()
        {
            var usuario = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuarioPorDefectoEmail),
                new Claim(ClaimTypes.Email, usuarioPorDefectoEmail),
                new Claim(ClaimTypes.NameIdentifier, usuarioPorDefectoId)
            }));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = usuario }
            };
        }

        //Me permite crear el Api en memoria para poder usarlo en las pruebas para hacer pruebas de integración
        // el ignorarSeguridad es para que no pregunte constantemente si es o no Admin
        protected WebApplicationFactory<Startup> ConstruirWebApplicationFactory(string nombreBD,
            bool ignorarSeguridad = true)
        {
            var factory = new WebApplicationFactory<Startup>();

            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {

                    //CODIGO DEL CURSO, NO FUNCIONABA
                //var descriptorDBContext = services.SingleOrDefault(d =>
                //d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                //if (descriptorDBContext != null)
                //{
                //    services.Remove(descriptorDBContext);
                //}

                //services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseInMemoryDatabase(nombreBD));
                //////////////////////////////////////////////////
                // 1. LIMPIEZA TOTAL: Borramos CUALQUIER registro de DbContextOptions
                // A veces Identity registra varios descriptores internos, por eso usamos un ToList() y un bucle.
                var contextos = services.Where(d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();
                foreach (var con in contextos)
                {
                    services.Remove(con);
                }

                // 2. Borramos el contexto en sí
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ApplicationDbContext));
                if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

                // 3. REGISTRO LIMPIO
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // Forzamos a que NO use el proveedor interno que ya podría estar contaminado
                    options.UseInMemoryDatabase(nombreBD);
                });

                if (ignorarSeguridad)
                    {
                        services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();

                        services.AddControllers(options =>
                        {
                            options.Filters.Add(new UsuarioFalsoFiltro());
                        });
                    }
                });
            });

            return factory;
        }
    }
}
