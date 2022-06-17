using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        public Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;

        RepositorioFornecedorEmBancoDados repositorioFornecedor;
        public RepositorioMedicamentoEmBancoDadosTest()
        {
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();


            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
            
            var fornecedor = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            repositorioFornecedor.Inserir(fornecedor);

            medicamento = new Medicamento("Dipirona","Para dor de cabeça e febre", "ABC123", DateTime.Today.AddMonths(15), fornecedor);
            medicamento.QuantidadeDisponivel = 50;


        }

        [TestMethod]
        public void Deve_inserir_medicamento()
        {
            //action
            repositorioMedicamento.Inserir(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);

        }
        [TestMethod]
        public void Deve_editar_medicamento()
        {
            //action
            repositorioMedicamento.Inserir(medicamento);

            //assert
            medicamento.Nome = "Paracetamol";
            medicamento.QuantidadeDisponivel = 30;
            medicamento.Validade = DateTime.Today.AddMonths(10);
            medicamento.Lote = "CBA321";

            repositorioMedicamento.Editar(medicamento);

            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);

        }
        [TestMethod]
        public void Deve_excluir_medicamento()
        {
            //arrange           
            repositorioMedicamento.Inserir(medicamento);

            //action           
            repositorioMedicamento.Excluir(medicamento);
           
            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);
            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_medicamento()
        {
            //arrange          
            repositorioMedicamento.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_medicamentos()
        {
            //arrange
            var fornecedor = new Fornecedor("Neosul", "(49) 95478-5789", "neosul@gmail.com", "Lages", "Santa Catarina");
            repositorioFornecedor.Inserir(fornecedor);

            var m01 = new Medicamento("Dipirona", "Para dor de cabeça e febre", "ABC123", DateTime.Today.AddMonths(15), fornecedor);
            m01.QuantidadeDisponivel = 10;
            var m02 = new Medicamento("Paracetamol","Para dores e febre", "FES432", DateTime.Today.AddMonths(10), fornecedor);
            m02.QuantidadeDisponivel = 40;
            var m03 = new Medicamento("Ibuprofeno", "Para dor", "CH23W", DateTime.Today.AddMonths(15), fornecedor);
            m03.QuantidadeDisponivel = 90;

            var repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorio.Inserir(m01);
            repositorio.Inserir(m02);
            repositorio.Inserir(m03);

            //action
            var medicamentos = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(3, medicamentos.Count);

            Assert.AreEqual(m01, medicamentos[0]);
            Assert.AreEqual(m02, medicamentos[1]);
            Assert.AreEqual(m03, medicamentos[2]);
        }
        

    }
}
