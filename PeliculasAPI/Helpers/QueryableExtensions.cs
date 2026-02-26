using PeliculasAPI.DTO.Paginacion;

namespace PeliculasAPI.Helpers
{
    public  static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosPorPagina) // el Skip es para saltear algunos registros
                .Take(paginacionDTO.CantidadRegistrosPorPagina);
        }
    }
}
