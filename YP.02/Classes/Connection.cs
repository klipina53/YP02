using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Connection
    {
        private static readonly string connectionString = "server=127.0.0.1;port=3306;database= electronic_journal;user=root;pwd=;";
        public static MySqlDataReader Query(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();

        }

        public static int Execute(string sql, Dictionary<string, object> parameters = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var kv in parameters)
                        {
                            cmd.Parameters.AddWithValue(kv.Key, kv.Value ?? DBNull.Value);
                        }
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
