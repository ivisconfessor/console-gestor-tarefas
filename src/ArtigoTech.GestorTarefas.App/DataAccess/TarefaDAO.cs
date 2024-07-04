using ArtigoTech.GestorTarefas.App.Interfaces;
using ArtigoTech.GestorTarefas.App.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

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
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Nome", tarefa.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", tarefa.Descricao);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Tarefa> ObterTarefas()
        {
            var tarefas = new List<Tarefa>();

            using (var connection = Database.GetConnection())
            {
                connection.Open();

                string sql = "SELECT * FROM TAREFAS";
                using (var cmd = new SQLiteCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarefa = new Tarefa
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            DataCriacao = Convert.ToDateTime(reader["DataCriacao"]).ToLocalTime()
                        };
                        tarefas.Add(tarefa);
                    }
                }
            }

            return tarefas;
        }

        public Tarefa ObterTarefaPorId(int id)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();

                string sql = "SELECT * FROM TAREFAS WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Tarefa
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString(),
                                Descricao = reader["Descricao"].ToString(),
                                DataCriacao = Convert.ToDateTime(reader["DataCriacao"]).ToLocalTime()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();

                string sql = "UPDATE TAREFAS SET Nome = @Nome, Descricao = @Descricao WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Nome", tarefa.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", tarefa.Descricao);
                    cmd.Parameters.AddWithValue("@Id", tarefa.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletarTarefa(int id)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();

                string sql = "DELETE FROM TAREFAS WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
