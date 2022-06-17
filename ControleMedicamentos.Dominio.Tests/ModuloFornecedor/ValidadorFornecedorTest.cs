using ControleMedicamentos.Dominio.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloFornecedor
{
    [TestClass]
    public class ValidadorFornecedorTest
    {
        [TestMethod]
        public void Nome_funcionario_deve_ser_obrigatorio()
        {
            //arrange
            var fornecedor = new Fornecedor(null, "999999999", "butantan@gmail.com", "São Paulo", "São Paulo");

            ValidadorFornecedor validador = new ValidadorFornecedor();

            //action
            var resultado = validador.Validate(fornecedor);

            //assert
            Assert.AreEqual("O campo 'Nome' é obrigatório", resultado.Errors[0].ErrorMessage);
        }
    }
}
