using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class ValidadorRequisicao : AbstractValidator<Requisicao>
    {
        public ValidadorRequisicao()
        {
            RuleFor(x => x.Medicamento)
                .NotNull().WithMessage("O campo 'Medicamento' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Medicamento' é obrigatório");
            RuleFor(x => x.Paciente)
                .NotNull().WithMessage("O campo 'Paciente' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Paciente' é obrigatório");
            RuleFor(x => x.Funcionario)
                .NotNull().WithMessage("O campo 'Funcionario' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Funcionario' é obrigatório");
        }
    }
}
