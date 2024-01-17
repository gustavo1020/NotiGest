using Core.Entityes;
using FluentValidation;

namespace Application.Validator
{
    public class ComentarioValidator : AbstractValidator<Comentario>
    {
        public ComentarioValidator() 
        {
            RuleFor(x => x.Contenido).NotNull().NotEmpty().WithMessage("No puede agregar un comentario vacio");
            RuleFor(x => x.AutorId).NotNull().WithMessage("No puede agregar un comentario sin autor relacionado");
            RuleFor(x => x.NoticiaId).NotNull().WithMessage("No puede agregar un comentario sin una noticia asociada");
            RuleFor(x => x.Contenido).MaximumLength(100).MinimumLength(10).WithMessage("El numero de caracteres del comentario tiene que estar dentro de los rangos 10 y 100");
        }
    }
}
