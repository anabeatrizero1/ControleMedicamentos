using ControleMedicamentos.Dominio.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloFuncionario
{
    [TestClass]
    public class ValidadorFuncionarioTest
    {
        [TestMethod]
        public void Nome_funcionario_deve_ser_obrigatorio()
        {
            //arrange
            var funcionario = new Funcionario(null, "login", "senha123");

            ValidadorFuncionario validador = new ValidadorFuncionario();

            //action
            var resultado = validador.Validate(funcionario);

            //assert
            Assert.AreEqual("O campo 'Nome' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
    }
}
