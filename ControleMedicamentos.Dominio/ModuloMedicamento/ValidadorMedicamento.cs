using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class ValidadorMedicamento : AbstractValidator<Medicamento>
    {
        public ValidadorMedicamento()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("O campo 'Nome' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Nome' é obrigatório");

            RuleFor(x => x.Descricao)
                .NotNull().WithMessage("O campo 'Descrição' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Descricao' é obrigatório");

            RuleFor(x => x.Lote)
                .NotNull().WithMessage("O campo 'Lote' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Lote' é obrigatório");

            RuleFor(x => x.Validade)
                .NotEqual(DateTime.MinValue)
                .WithMessage("O campo 'Validade' é obrigatório");
        }
    }
}
