using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        public Requisicao requisicao;
        private RepositorioRequisicaoEmBancoDados repositorioRequisicao;

        public Funcionario funcionario;
        public RepositorioFuncionarioEmBancoDados repositorioFuncionario;
        public RepositorioPacienteEmBancoDados repositorioPaciente;
        public RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        
        public Fornecedor fornecedor;
        public RepositorioFornecedorEmBancoDados repositorioFornecedor;

        public RepositorioRequisicaoEmBancoDadosTest()
        {
            repositorioRequisicao = new RepositorioRequisicaoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioPaciente = new RepositorioPacienteEmBancoDados();
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();

            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            var fornecedor = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            fornecedor.Id = 1;
            repositorioFornecedor.Inserir(fornecedor);

            var medicamento = new Medicamento("Dipirona", "Para dor de cabeça e febre", "ABC123", DateTime.Today.AddMonths(15), fornecedor);
            medicamento.Id = 1;
            medicamento.QuantidadeDisponivel = 5;
            repositorioMedicamento.Inserir(medicamento);

            var paciente = new Paciente("José da Silva", "321654987");
            paciente.Id = 1;
            repositorioPaciente.Inserir(paciente);

            var funcionario = new Funcionario("Maria Fernanda", "maria_fernanda", "fernanda321");
            funcionario.Id = 1;
            repositorioFuncionario.Inserir(funcionario);

            requisicao = new Requisicao(medicamento, paciente, funcionario, 200, DateTime.Today);
            requisicao.Id = 1;


        }
        [TestMethod]
        public void Deve_inserir_requisicao()
        {
            //action
            repositorioRequisicao.Inserir(requisicao);

            //assert
            var requisicaoEncontrado = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao, requisicaoEncontrado);

        }

         [TestMethod]
        public void Deve_editar_requisicao()
        {
            //action
            var funcionario = new Funcionario("Julio Rodrigues", "julio_rodrigues", "julio321");
            funcionario.Id = 2;
            repositorioFuncionario.Inserir(funcionario);


            repositorioRequisicao.Inserir(requisicao);

            //assert
            requisicao.QtdMedicamento = 10;
            requisicao.Data = DateTime.Today;
            requisicao.Funcionario = funcionario;

            repositorioRequisicao.Editar(requisicao);

            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao, requisicaoEncontrada);

        }
        [TestMethod]
        public void Deve_excluir_requisicao()
        {
            //arrange           
            repositorioRequisicao.Inserir(requisicao);

            //action           
            repositorioRequisicao.Excluir(requisicao);

            //assert
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);
            Assert.IsNull(requisicaoEncontrada);
        }
        [TestMethod]
        public void Deve_selecionar_apenas_uma_requisicao()
        {
            //arrange          
            repositorioRequisicao.Inserir(requisicao);

            //action
            var requisicaoEncontrada = repositorioRequisicao.SelecionarPorId(requisicao.Id);

            //assert
            Assert.IsNotNull(requisicaoEncontrada);
            Assert.AreEqual(requisicao, requisicaoEncontrada);
        }

        [TestMethod]
        public void Deve_selecionar_todas_requisicoes()
        {
            //arrange
            var fornecedor = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            repositorioFornecedor.Inserir(fornecedor);

            var m01 = new Medicamento("Dipirona", "Para dor de cabeça e febre", "ABC123", DateTime.Today.AddMonths(15), fornecedor);
            var m02 = new Medicamento("Paracetamol", "Para dores e febre", "FES432", DateTime.Today.AddMonths(10), fornecedor);
            var m03 = new Medicamento("Ibuprofeno", "Para dor", "CH23W", DateTime.Today.AddMonths(15), fornecedor);
            repositorioMedicamento.Inserir(m01);
            repositorioMedicamento.Inserir(m02);
            repositorioMedicamento.Inserir(m03);
            Assert.AreEqual(m01, repositorioMedicamento.SelecionarPorId(m01.Id));


            var p01 = new Paciente("Maria da Silva", "321654987");
            var p02 = new Paciente("João da Silva", "322654987");
            var p03 = new Paciente("Francisco da Silva", "321654337");
            repositorioPaciente.Inserir(p01);
            repositorioPaciente.Inserir(p02);
            repositorioPaciente.Inserir(p03);

            var funcionario = new Funcionario("Maria Fernanda", "maria_fernanda", "fernanda321");
            repositorioFuncionario.Inserir(funcionario);

            var r01 = new Requisicao(m01, p01, funcionario, 200, DateTime.Today);
            var r02 = new Requisicao(m02, p02, funcionario, 23, DateTime.Today);
            var r03 = new Requisicao(m03, p03, funcionario, 49, DateTime.Today);
            repositorioRequisicao.Inserir(r01);
            repositorioRequisicao.Inserir(r02);
            repositorioRequisicao.Inserir(r03);

            //action
            var requisicoes  = repositorioRequisicao.SelecionarTodos();

            //assert

            Assert.AreEqual(3, requisicoes.Count);

            Assert.AreEqual(r01, requisicoes[0]);
            Assert.AreEqual(r02, requisicoes[1]);
            Assert.AreEqual(r03, requisicoes[2]);
        }

    }
}
