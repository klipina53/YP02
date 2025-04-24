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
            List<Zaniyatie> zaniyatia = new List<Zaniyatie>();
            string query = "SELECT * FROM `Zaniyatie`";

            using (var reader = Connection.Query(query))
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
            return zaniyatia;
        }

        public bool Add(Zaniyatie zaniyatie)
        {
            string query = $"INSERT INTO `Zaniyatie` (`ZaniyatieName`, `MinutesMissed`, `Obyasnitelnaya`) VALUES (@ZaniyatieName, @MinutesMissed, @Obyasnitelnaya)";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Zaniyatie zaniyatie)
        {
            string query = $"UPDATE `Zaniyatie` SET `ZaniyatieName` = @ZaniyatieName, `MinutesMissed` = @MinutesMissed, `Obyasnitelnaya` = @Obyasnitelnaya WHERE `Id` = @Id";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int id)
        {
            string query = $"DELETE FROM `Zaniyatie` WHERE `Id` = @Id";

            var result = Connection.Query(query);
            return result != null;
        }
    }
}

