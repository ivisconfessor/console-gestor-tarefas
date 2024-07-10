using Dapper;
using System.Data.SQLite;

namespace ArtigoTech.GestorTarefas.App.DataAccess
{
    public class Database
    {
        private const string connectionString = @"[SUA CONNECTION STRING]";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public static void CriarTabelaTarefas()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS TAREFAS (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL,
                        Descricao TEXT,
                        DataCriacao DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                connection.Execute(sql);
            }
        }
    }
}
