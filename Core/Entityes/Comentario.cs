using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Entityes
{
    public class Comentario : IdentityBase
    {

        [Required]
        [MaxLength(100)]
        public required string Contenido { get; set; }

        [Required]
        public Guid AutorId { get; set; }

        public required User Autor { get; set; }

        [Required]
        public Guid NoticiaId { get; set; }

        public required Noticia Noticia { get; set; }

        [DefaultValue(false)]
        public bool Destacado { get; set; }
    }
}
