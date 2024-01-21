using System.ComponentModel;

namespace Application.DTO
{
    public class NoticiaDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime FechaBaja { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public UsuarioDto? Autor { get; set; }
        public bool Destacada { get; set; }
        public string? EnlaceImagen { get; set; }
    }

    public class NoticiaUpdateDto
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        [DefaultValue(false)]
        public bool Destacada { get; set; }
        public string? EnlaceImagen { get; set; }
    }

    public class NoticiaInsertDto
    {
        public Guid Usuario { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        [DefaultValue(false)]
        public bool Destacada { get; set; }
        public string? EnlaceImagen { get; set; }
    }

    public class Root
    {
        public DateTime date { get; set; }
        public required string title { get; set; }
        public required string content { get; set; }
        public required string link { get; set; }
        public required List<string> symbols { get; set; }
        public required List<object> tags { get; set; }
        public required Sentiment sentiment { get; set; }
    }

    public class Sentiment
    {
        public double polarity { get; set; }
        public double neg { get; set; }
        public double neu { get; set; }
        public double pos { get; set; }
    }
}
