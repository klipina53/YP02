using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class ZaniyatieContext
    {
        private readonly string _connectionString;

        public ZaniyatieContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Zaniyatie> LoadZaniyatia()
        {
            List<Zaniyatie> zaniyatia = new List<Zaniyatie>();
            string query = "SELECT * FROM `Zaniyatie`";

            using (var connection = new MySqlConnection(_connectionString)) // Используется MySqlConnection или другой подходящий для вашей БД
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string zaniyatieName = reader.GetString(1);
                            int minutesMissed = reader.GetInt32(2);
                            byte[] obyasnitelnaya = reader.IsDBNull(3) ? null : (byte[])reader[3];

                            zaniyatia.Add(new Zaniyatie
                            {
                                Id = id,
                                ZaniyatieName = zaniyatieName,
                                MinutesMissed = minutesMissed,
                                Obyasnitelnaya = obyasnitelnaya
                            });
                        }
                    }
                }
            }
            return zaniyatia;
        }

        public bool Add(Zaniyatie zaniyatie)
        {
            string query = $"INSERT INTO `Zaniyatie` (`ZaniyatieName`, `MinutesMissed`, `Obyasnitelnaya`) VALUES (@ZaniyatieName, @MinutesMissed, @Obyasnitelnaya)";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ZaniyatieName", zaniyatie.ZaniyatieName);
                    command.Parameters.AddWithValue("@MinutesMissed", zaniyatie.MinutesMissed);
                    command.Parameters.AddWithValue("@Obyasnitelnaya", (object)zaniyatie.Obyasnitelnaya ?? DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Zaniyatie zaniyatie)
        {
            string query = $"UPDATE `Zaniyatie` SET `ZaniyatieName` = @ZaniyatieName, `MinutesMissed` = @MinutesMissed, `Obyasnitelnaya` = @Obyasnitelnaya WHERE `Id` = @Id";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ZaniyatieName", zaniyatie.ZaniyatieName);
                    command.Parameters.AddWithValue("@MinutesMissed", zaniyatie.MinutesMissed);
                    command.Parameters.AddWithValue("@Obyasnitelnaya", (object)zaniyatie.Obyasnitelnaya ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", zaniyatie.Id);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            string query = $"DELETE FROM `Zaniyatie` WHERE `Id` = @Id";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

