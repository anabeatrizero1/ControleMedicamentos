using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class ValidadorMedicamentoTest
    {
        [TestMethod]
        public void Nome_do_remedio_deve_ser_obrigatorio()
        {
            //arrange
            var medicamento = new Medicamento(null, "Para dor de cabeça", "ABC", DateTime.Now.AddDays(2), new Fornecedor());
            medicamento.QuantidadeDisponivel = 10;

            ValidadorMedicamento validador = new ValidadorMedicamento();

            //action
            var resultado = validador.Validate(medicamento);

            //assert
            Assert.AreEqual("O campo 'Nome' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Descricao_do_remedio_deve_ser_obrigatorio()
        {
            //arrange
            var medicamento = new Medicamento("Dipirona", null, "ABC", DateTime.Today.AddDays(2), new Fornecedor());

            ValidadorMedicamento validador = new ValidadorMedicamento();

            //action
            var resultado = validador.Validate(medicamento);

            //assert
            Assert.AreEqual("O campo 'Descrição' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Lote_do_remedio_deve_ser_obrigatorio()
        {
            //arrange
            var medicamento = new Medicamento("Dipirona", "Para dor de cabeça", null, DateTime.Now.AddDays(2), new Fornecedor());
            medicamento.QuantidadeDisponivel = 10;

            ValidadorMedicamento validador = new ValidadorMedicamento();

            //action
            var resultado = validador.Validate(medicamento);

            //assert
            Assert.AreEqual("O campo 'Lote' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Validade_do_remedio_deve_ser_obrigatorio()
        {
            //arrange
            var medicamento = new Medicamento("Dipirona", "Para dor de cabeça", "ABC", DateTime.MinValue, new Fornecedor());
            medicamento.QuantidadeDisponivel = 10;


            ValidadorMedicamento validador = new ValidadorMedicamento();

            //action
            var resultado = validador.Validate(medicamento);

            //assert
            Assert.AreEqual("O campo 'Validade' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
    }
}
