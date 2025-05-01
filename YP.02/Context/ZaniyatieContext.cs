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
            const string sql = @"
        INSERT INTO `Zaniyatie`
            (`Name`, `MinutesMissed`, `ExplanationText`)
        VALUES
            (@name, @minutes, @explanationText);
    ";

            var parameters = new Dictionary<string, object>
            {
                ["@name"] = zaniyatie.Name,
                ["@minutes"] = zaniyatie.MinutesMissed,
                ["@explanationText"] = (object)zaniyatie.ExplanationText ?? DBNull.Value
            };

            int rowsAffected = Connection.Execute(sql, parameters);
            return rowsAffected > 0;
        }

        public bool Update(Zaniyatie zaniyatie)
        {
            const string sql = @"
        UPDATE `Zaniyatie`
        SET
            `Name`            = @name,
            `MinutesMissed`   = @minutes,
            `ExplanationText` = @explanationText
        WHERE `Id` = @id;
    ";

            var parameters = new Dictionary<string, object>
            {
                ["@id"] = zaniyatie.Id,
                ["@name"] = zaniyatie.Name,
                ["@minutes"] = zaniyatie.MinutesMissed,
                ["@explanationText"] = (object)zaniyatie.ExplanationText ?? DBNull.Value
            };

            int rowsAffected = Connection.Execute(sql, parameters);
            return rowsAffected > 0;
        }

        public bool Delete(int id)
        {
            const string sql = @"
        DELETE FROM `Zaniyatie`
        WHERE `Id` = @id;
    ";
            var parameters = new Dictionary<string, object>
            {
                ["@id"] = id
            };
            int rowsAffected = Connection.Execute(sql, parameters);
            return rowsAffected > 0;
        }

    }
}


