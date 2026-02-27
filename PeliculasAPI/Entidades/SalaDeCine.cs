using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;


namespace PeliculasAPI.Entidades
{
    public class SalaDeCine : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }

        //para usar la ubicacion geográfica. En SQL se mapeara al tipo Geography
        //El using del point debe ser de NetTopologySuite
        public Point Ubicacion { get; set; } 
        public List<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
    }
}
