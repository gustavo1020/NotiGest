using Core.Entityes;
using FluentValidation;

namespace Application.Validator
{
    public class NoticiaValidator : AbstractValidator<Noticia>
    {
        public NoticiaValidator()
        {
            RuleFor(x => x.Titulo).NotNull().NotEmpty().WithMessage("No puede agregar un titulo vacio");
            RuleFor(x => x.Contenido).NotNull().NotEmpty().WithMessage("No puede agregar una noticia vacia");
            RuleFor(x => x.Titulo).MaximumLength(50).MinimumLength(10).WithMessage("El numero de caracteres del titulo tiene que estar dentro de los rangos 10 y 50");
            RuleFor(x => x.Contenido).MaximumLength(500).MinimumLength(50).WithMessage("El numero de caracteres del contenido tiene que estar dentro de los rangos 50 y 500");
            RuleFor(x => x.AutorId).NotNull().WithMessage("No puede agregar una noticia sin autor relacionado");
        }
    }
}
