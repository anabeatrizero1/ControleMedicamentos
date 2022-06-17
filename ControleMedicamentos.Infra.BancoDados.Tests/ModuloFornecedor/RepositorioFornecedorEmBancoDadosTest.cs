using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        public Fornecedor fornecedor;
        public RepositorioFornecedorEmBancoDados repositorio;

        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");


            fornecedor = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            repositorio = new RepositorioFornecedorEmBancoDados();
            
        }
        [TestMethod]
        public void Deve_inserir_novo_fornecedor()
        {
            //action
            repositorio.Inserir(fornecedor);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(fornecedor, funcionarioEncontrado);
        }
        [TestMethod]
        public void Deve_editar_informacoes_fornecedor()
        {
            //arrange                      
            repositorio.Inserir(fornecedor);

            //action
            fornecedor.Nome = "Neosul Lages";
            fornecedor.Telefone = "(49) 98878-8875";
            fornecedor.Email = "neosul_lages@gmail.com";
            fornecedor.Cidade = "Lages";
            fornecedor.Estado = "Santa Catarina";
            repositorio.Editar(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange           
            repositorio.Inserir(fornecedor);

            //action           
            repositorio.Excluir(fornecedor);

            //assert
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);
            Assert.IsNull(fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_fornecedor()
        {
            //arrange          
            repositorio.Inserir(fornecedor);

            //action
            var fornecedorEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            //assert
            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_fornecedor()
        {
            //arrange
            var f01 = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            var f02 = new Fornecedor("MedSul Pharma", "(49) 95874-5889", "medsulfarma@gmail.com", "Joinville", "Santa Catarina");
            var f03 = new Fornecedor("Vera Cruz", "(11) 98758-5879", "veraCruz@gmail.com", "São Paulo", "São Paulo");

            
            var repositorio = new RepositorioFornecedorEmBancoDados();
            repositorio.Inserir(f01);
            repositorio.Inserir(f02);
            repositorio.Inserir(f03);

            //action
            var pacientes = repositorio.SelecionarTodos();

            //assert
            Assert.AreEqual(3, pacientes.Count);

            Assert.AreEqual(f01, pacientes[0]);
            Assert.AreEqual(f02, pacientes[1]);
            Assert.AreEqual(f03, pacientes[2]);
        }
    }
}
