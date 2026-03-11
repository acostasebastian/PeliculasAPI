using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTO.Genero;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : CustomBaseController
    {        
        public GenerosController( ApplicationDbContext context, IMapper mapper) 
            :base(context,mapper) // se los paso para que los pueda usar el controller custom
        {
            
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get() 
        {             
            //var entidades  = await context.Generos.ToListAsync();
            //var dtos = mapper.Map<List<GeneroDTO>>(entidades);

            //return dtos;

            //ESTO REEMPLAZA LO QUE ESTÁ ARRIBA AL HABERLO HECHO GENÉRICO
            return await Get<Genero,GeneroDTO>(); 

        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Post<GeneroCreacionDTO, Genero, GeneroDTO>(generoCreacionDTO,"obtenerGenero");

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            return await Put<GeneroCreacionDTO, Genero>(id, generoCreacionDTO);

        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
          return await Delete<Genero>(id);

        }
    }
}
