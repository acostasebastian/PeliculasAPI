namespace PeliculasAPI.DTO.Paginacion
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;

        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int CantidadRegistrosPorPagina 
        { 
            get => cantidadRegistrosPorPagina; 
            set
            {
                //para que como maximo pueda poner 50, y si pone 100 le ponemos solo 50
                cantidadRegistrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}
