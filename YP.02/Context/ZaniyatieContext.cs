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
        public List<Zaniyatie> LoadZaniyatia()
        {
            List<Zaniyatie> zaniyatie = new List<Zaniyatie>();
            string query = "SELECT * FROM `Zaniyatie`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string zaniyatieName = reader.GetString(1);
                    int minutesMissed = reader.GetInt32(2);
                    string explanationText = reader.GetString(3); // Объяснительная в текстовом формате

                    zaniyatie.Add(new Zaniyatie
                    {
                        Id = id,
                        Name = zaniyatieName,
                        MinutesMissed = minutesMissed,
                        ExplanationText = explanationText // Изменено на текст
                    });
                }
            }
            return zaniyatie;
        }

        public bool Add(Zaniyatie zaniyatie)
        {
            string query = "INSERT INTO `Zaniyatie` (`ZaniyatieName`, `MinutesMissed`, `Obyasnitelnaya`) VALUES (@name, @minutes, @explanationText)";

            var parameters = new
            {
                name = zaniyatie.Name,
                minutes = zaniyatie.MinutesMissed,
                explanationText = zaniyatie.ExplanationText ?? (object)DBNull.Value // Объяснительная как текст
            };

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Zaniyatie zaniyatie)
        {
            string query = "UPDATE `Zaniyatie` SET `ZaniyatieName` = @name, `MinutesMissed` = @minutes, `Obyasnitelnaya` = @explanationText WHERE `Id` = @id";

            var parameters = new
            {
                id = zaniyatie.Id,
                name = zaniyatie.Name,
                minutes = zaniyatie.MinutesMissed,
                explanationText = zaniyatie.ExplanationText // Объяснительная как текст
            };
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM `Zaniyatie` WHERE `Id` = @id";

            var parameters = new { id };
            var result = Connection.Query(query);
            return result != null;
        }
    }
}


