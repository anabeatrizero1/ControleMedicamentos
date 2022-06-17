using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private string enderecoBanco =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ControleMedicamentosDb;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
       
        private const string sqlInserir =
          @"INSERT INTO [TBREQUISICAO]
               (
			       [FUNCIONARIO_ID],
			       [PACIENTE_ID],
			       [MEDICAMENTO_ID],
			       [QUANTIDADEMEDICAMENTO],
			       [DATA]
                ) 
            VALUES
               (
                   @FUNCIONARIO_ID,
                   @PACIENTE_ID,
                   @MEDICAMENTO_ID,
                   @QUANTIDADEMEDICAMENTO,
                   @DATA

                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBREQUISICAO]	
		        SET
			       [FUNCIONARIO_ID] = @FUNCIONARIO_ID,
			       [PACIENTE_ID] = @PACIENTE_ID,
			       [MEDICAMENTO_ID] = MEDICAMENTO_ID,
			       [QUANTIDADEMEDICAMENTO] = @QUANTIDADEMEDICAMENTO,
			       [DATA] = @DATA
		        WHERE
			        [ID] = @ID";

        private const string sqlExcluir =
           @"DELETE FROM [TBREQUISICAO]			        
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarPorId =
          @"SELECT 
		        R.[ID],
		        R.[FUNCIONARIO_ID],
		        R.[PACIENTE_ID],
		        R.[MEDICAMENTO_ID],
		        R.[QUANTIDADEMEDICAMENTO],
		        R.[DATA]
           FROM 
		        [TBREQUISICAO] AS R INNER JOIN
		        [TBFUNCIONARIO] ON R.FUNCIONARIO_ID = [TBFUNCIONARIO].ID INNER JOIN
		        [TBPACIENTE] ON R.PACIENTE_ID = [TBPACIENTE].ID INNER JOIN
		        [TBMEDICAMENTO] ON R.MEDICAMENTO_ID = [TBMEDICAMENTO].ID 

		   WHERE
                R.[ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT 
		        R.[ID],
		        R.[FUNCIONARIO_ID],
		        R.[PACIENTE_ID],
		        R.[MEDICAMENTO_ID],
		        R.[QUANTIDADEMEDICAMENTO],
		        R.[DATA]
           FROM 
		        [TBREQUISICAO] AS R INNER JOIN
		        [TBFUNCIONARIO] ON R.FUNCIONARIO_ID = [TBFUNCIONARIO].ID INNER JOIN
		        [TBPACIENTE] ON R.PACIENTE_ID = [TBPACIENTE].ID INNER JOIN
		        [TBMEDICAMENTO] ON R.MEDICAMENTO_ID = [TBMEDICAMENTO].ID 
            ";

        public ValidationResult Inserir(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            requisicao.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Requisicao requisicao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", requisicao.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public Requisicao SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            Requisicao requisicao = null;
            if (leitorRequisicao.Read())
                requisicao = ConverterParaRequisicao(leitorRequisicao);

            conexaoComBanco.Close();

            if (requisicao != null) {
                var repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
                requisicao.Funcionario = repositorioFuncionario.SelecionarPorId(requisicao.Funcionario.Id);
                var repositorioPaciente = new RepositorioPacienteEmBancoDados();
                requisicao.Paciente = repositorioPaciente.SelecionarPorId(requisicao.Paciente.Id);
                var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
                requisicao.Medicamento = repositorioMedicamento.SelecionarPorId(requisicao.Paciente.Id);
            }
            return requisicao;
        }
        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicao.Read())
                requisicoes.Add(ConverterParaRequisicao(leitorRequisicao));

            conexaoComBanco.Close();

            foreach (var r in requisicoes)
            {
                var repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
                r.Funcionario = repositorioFuncionario.SelecionarPorId(r.Funcionario.Id);
                var repositorioPaciente = new RepositorioPacienteEmBancoDados();
                r.Paciente = repositorioPaciente.SelecionarPorId(r.Paciente.Id);
                var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
                r.Medicamento = repositorioMedicamento.SelecionarPorId(r.Paciente.Id);
            }



            return requisicoes;
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRequisicao)
        {
            var id = Convert.ToInt32(leitorRequisicao["ID"]);
            var funcionarioId = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);
            var pacienteId = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            var medicamentoId = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_ID"]);
            var quantidadeMedicamento = Convert.ToInt32(leitorRequisicao["QUANTIDADEMEDICAMENTO"]);
            var data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            var requisicao = new Requisicao()
            {
                Id = id,
                Data = data,
                QtdMedicamento = quantidadeMedicamento,

                Funcionario = new(" ", " ", " ")
                {
                    Id = funcionarioId,
                },

                Paciente = new(" ", " ")
                {
                    Id = pacienteId,
                },

                Medicamento = new(" ", " ", " ", DateTime.Today, null)
                {
                    Id = medicamentoId
                }
            };
            return requisicao;
        }

        private void ConfigurarParametrosRequisicao( Requisicao requisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", requisicao.Id);
            comando.Parameters.AddWithValue("FUNCIONARIO_ID", requisicao.Funcionario.Id);
            comando.Parameters.AddWithValue("PACIENTE_ID", requisicao.Paciente.Id);
            comando.Parameters.AddWithValue("MEDICAMENTO_ID", requisicao.Medicamento.Id);
            comando.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", requisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("DATA", requisicao.Data);
         }
    }
}
