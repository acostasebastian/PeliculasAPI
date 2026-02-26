using PeliculasAPI.DTO.ActorPelicula;
using PeliculasAPI.DTO.Genero;

namespace PeliculasAPI.DTO.Pelicula
{
    public class PeliculaDetallesDTO : PeliculaDTO
    {
        public List<GeneroDTO> Generos { get; set; }
        public List<ActorPeliculaDetalleDTO> Actores { get; set; }
    }
}
