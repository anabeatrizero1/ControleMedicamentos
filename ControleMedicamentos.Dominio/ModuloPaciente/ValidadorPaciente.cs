﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class ValidadorPaciente : AbstractValidator<Paciente>
    {
        public ValidadorPaciente()
        {
           RuleFor(x => x.Nome)
                .NotNull().WithMessage("O campo 'Nome' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Nome' é obrigatório");
            RuleFor(x => x.CartaoSUS)
                .NotNull().WithMessage("O campo 'Cartão SUS' é obrigatório")
                .NotEmpty().WithMessage("O campo 'Cartão SUS' é obrigatório"); 
        }

    }
}
