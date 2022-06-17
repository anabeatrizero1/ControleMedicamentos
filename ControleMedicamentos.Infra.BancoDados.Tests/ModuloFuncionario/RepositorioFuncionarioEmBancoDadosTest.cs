using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        public Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorio;

        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            funcionario = new Funcionario("Maria Fernanda", "maria_fernanda", "fernanda321");
            repositorio = new RepositorioFuncionarioEmBancoDados();

        }


        [TestMethod]
        public void Deve_inserir_novo_funcionario()
        {
            //action
            repositorio.Inserir(funcionario);

            //assert
            var funcionarioEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(funcionarioEncontrado);
            Assert.AreEqual(funcionario, funcionarioEncontrado);

        }

        [TestMethod]
        public void Deve_editar_informacoes_funcionario()
        {
            //arrange                      
            repositorio.Inserir(funcionario);

            //action
            funcionario.Nome = "Maria da Silva";
            funcionario.Login = "maria_silva";
            funcionario.Senha = "mariazinha123";
            repositorio.Editar(funcionario);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(funcionario, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_funcionario()
        {
            //arrange           
            repositorio.Inserir(funcionario);

            //action           
            repositorio.Excluir(funcionario);

            //assert
            var pacienteEncontrado = repositorio.SelecionarPorId(funcionario.Id);
            Assert.IsNull(pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_funcionario()
        {
            //arrange          
            repositorio.Inserir(funcionario);

            //action
            var pacienteEncontrado = repositorio.SelecionarPorId(funcionario.Id);

            //assert
            Assert.IsNotNull(pacienteEncontrado);
            Assert.AreEqual(funcionario, pacienteEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_funcioanrios()
        {
            //arrange
            var f01 = new Funcionario("Pedro Silva", "pedro_silva", "acordapedrinho");
            var f02 = new Funcionario("Leonardo Fontes", "leonardo_fontes", "leozin123");
            var f03 = new Funcionario("Amanda Alves", "amanda_alves", "amanda123");

            var repositorio = new RepositorioFuncionarioEmBancoDados();
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
