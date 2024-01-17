using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Core.Entityes
{
    public class Noticia : IdentityBase
    {
        [Required]
        [MaxLength(50)]
        public required string Titulo { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Contenido { get; set; }

        [Required]
        public Guid AutorId { get; set; }

        public required User Autor { get; set; }

        [DefaultValue(false)]
        public bool Destacado { get; set; }

        [Url]
        public string? EnlaceImagen { get; set; }

        public ICollection<Comentario>? Comentarios { get; set; }

    }
}
