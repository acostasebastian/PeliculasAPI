using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Entidades
{
    public class Genero : IId //esta interfaz que heredo es la que tiene el ID para poder usar en el controller generico
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
