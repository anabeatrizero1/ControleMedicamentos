using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {   

        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public Funcionario Funcionario { get; set; }


        public Requisicao(Medicamento medicamento, Paciente paciente, Funcionario funcionario, int qtdMedicamento, DateTime data)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            Funcionario = funcionario;
            QtdMedicamento = qtdMedicamento;
            Data = data;
        }
        public Requisicao()
        {

        }

        public override bool Equals(object obj)
        {
            Requisicao requisicao = obj as Requisicao;

            if (requisicao == null)
                return false;

            return
                requisicao.Id.Equals(Id) &&
                requisicao.QtdMedicamento.Equals(QtdMedicamento) &&
                requisicao.Paciente.Equals(Paciente) &&
                requisicao.Funcionario.Equals(Funcionario) &&
                requisicao.Medicamento.Equals(Medicamento);
        }
    }
}
