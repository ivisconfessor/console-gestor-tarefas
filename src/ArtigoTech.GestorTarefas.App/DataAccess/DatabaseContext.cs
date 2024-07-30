using System.Data.SQLite;
using System.Data.Entity;
using ArtigoTech.GestorTarefas.App.Models;

namespace ArtigoTech.GestorTarefas.App.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() :
            base("name=GestorTarefas")
        {
            Database.CreateIfNotExists();

            string sql = @"
                    CREATE TABLE IF NOT EXISTS TAREFAS (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL,
                        Descricao TEXT,
                        DataCriacao DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

            Database.ExecuteSqlCommand(sql);
        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
