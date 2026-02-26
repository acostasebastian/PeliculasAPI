using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTO.Actor
{
    public class ActorCreacionDTO : ActorPatchDTO
    {
        

        [PesoArchivoValidacion(PesoMaximoEnMegaBytes:4)]
        [TipoArchivoValidacion(grupoTipoArchivo:GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
