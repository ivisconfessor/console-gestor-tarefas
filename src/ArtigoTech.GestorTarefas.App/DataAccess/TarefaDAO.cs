using ArtigoTech.GestorTarefas.App.Interfaces;
using ArtigoTech.GestorTarefas.App.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace ArtigoTech.GestorTarefas.App.DataAccess
{
    public class TarefaDAO : ITarefaDAO
    {
        public void AdicionarTarefa(Tarefa tarefa)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO TAREFAS (Nome, Descricao) VALUES (@Nome, @Descricao)";
                connection.Execute(sql, tarefa);
            }
        }

        public List<Tarefa> ObterTarefas()
        {
            var tarefas = new List<Tarefa>();

            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM TAREFAS";
                tarefas = connection.Query<Tarefa>(sql).ToList();

                tarefas.ForEach(t => t.DataCriacao = t.DataCriacao.ToLocalTime());

                return tarefas;
            }
        }

        public Tarefa ObterTarefaPorId(int id)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM TAREFAS WHERE Id = @Id";
                var tarefa = connection.QuerySingleOrDefault<Tarefa>(sql, new { Id = id });
            
                if (tarefa != null)
                {
                    tarefa.DataCriacao = tarefa.DataCriacao.ToLocalTime();
                }

                return tarefa;
            }
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "UPDATE TAREFAS SET Nome = @Nome, Descricao = @Descricao WHERE Id = @Id";
                connection.Execute(sql, tarefa);
            }
        }

        public void DeletarTarefa(int id)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string sql = "DELETE FROM TAREFAS WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }
    }
}
