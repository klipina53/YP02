using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class DisciplineContext
    {
        public List<Disciplines> LoadDisciplines()
        {
            List<Disciplines> disciplines = new List<Disciplines>();
            string query = "SELECT * FROM `Disciplines`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int disciplineId = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int totalHours = CalculateTotalHours(disciplineId);

                    disciplines.Add(new Disciplines
                    {
                        DisciplineId = disciplineId,
                        Name = name,
                        TotalHours = totalHours
                    });
                }
            }
            return disciplines;
        }

        private int CalculateTotalHours(int disciplineId)
        {
            int totalHours = 0;
            string query = $"SELECT SUM(Hours) AS TotalHours FROM DisciplinePrograms WHERE DisciplineId = {disciplineId}";

            using (var reader = Connection.Query(query))
            {
                if (reader.Read())
                {
                    // Проверяем, является ли значение DBNull перед его получением
                    if (!reader.IsDBNull(0))
                    {
                        totalHours = reader.GetInt32(0);
                    }
                }
            }

            return totalHours;
        }

        public bool Add(Disciplines discipline)
        {
            string query = $"INSERT INTO `Disciplines`(`Name`) VALUES ('{discipline.Name}')";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Disciplines discipline)
        {
            string query = $"UPDATE `Disciplines` SET `Name`= '{discipline.Name}' WHERE `DisciplineID`= {discipline.DisciplineId}";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int disciplineId)
        {
            string query = $"DELETE FROM `Disciplines` WHERE `DisciplineID`= {disciplineId}";
            var result = Connection.Query(query);
            return result != null;
        }

    }
}
