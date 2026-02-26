using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTO.Genero
{
    public class GeneroCreacionDTO
    {
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}
