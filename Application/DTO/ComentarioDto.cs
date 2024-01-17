using System.ComponentModel;

namespace Application.DTO
{
    public class ComentarioDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime FechaBaja { get; set; }
        public string? Contenido { get; set; }
        public UsuarioDto? Autor { get; set; }
        public Guid NoticiaId { get; set; }
        public bool Destacado { get; set; }
    }

    public class ComentarioUpdateDto
    {
        public string? Contenido { get; set; }

        [DefaultValue(false)]
        public bool Destacado { get; set; } = false;

    }

    public class ComentarioInsertDto
    {
        public string? Contenido { get; set; }
        public Guid AutorId { get; set; }
        public  Guid NoticiaId { get; set; }

        [DefaultValue(false)]
        public bool Destacado { get; set; }
    }
}
