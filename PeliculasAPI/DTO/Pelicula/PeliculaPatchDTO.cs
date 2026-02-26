using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTO.Pelicula
{
    public class PeliculaPatchDTO 
    {
        
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }

    }
}
