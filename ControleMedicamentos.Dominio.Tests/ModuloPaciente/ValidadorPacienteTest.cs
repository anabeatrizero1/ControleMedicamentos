using ControleMedicamentos.Dominio.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]
    public class ValidadorPacienteTest
    {
        public ValidadorPacienteTest()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");
        }
        [TestMethod]
        public void Nome_do_paciente_deve_ser_obrigatorio()
        {
            //arrange
            var paciente = new Paciente(null, "999989879");

            ValidadorPaciente validador = new ValidadorPaciente();

            //action
            var resultado = validador.Validate(paciente);

            //assert
            Assert.AreEqual("O campo 'Nome' é obrigatório", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void CartaoSUS_do_paciente_deve_ser_obrigatorio()
        {
            //arrange
            var paciente = new Paciente("Ana", "");

            ValidadorPaciente validador = new ValidadorPaciente();

            //action
            var resultado = validador.Validate(paciente);

            //assert
            Assert.AreEqual("O campo 'Cartão SUS' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
    }
}
