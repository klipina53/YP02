using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class DisciplineProgramContext
    {
        public List<DisciplinePrograms> LoadDisciplinePrograms(int disciplineId)
        {
            List<DisciplinePrograms> programs = new List<DisciplinePrograms>();
            string query = $"SELECT * FROM `DisciplinePrograms` WHERE DisciplineID = {disciplineId};";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    programs.Add(new DisciplinePrograms
                    {
                        ProgrammId = reader.GetInt32(0),
                        DisciplineId = reader.GetInt32(1),
                        Topic = reader.GetString(2),
                        Type = reader.GetString(3),
                        Hours = reader.GetInt32(4)
                    });
                }
            }
            return programs;
        }

        public bool Add(DisciplinePrograms program)
        {
            string query = $"INSERT INTO `DisciplinePrograms`(`DisciplineID`, `Topic`, `Type`, `Hours`) VALUES ({program.DisciplineId},'{program.Topic}','{program.Type}', {program.Hours})";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(DisciplinePrograms program)
        {
            string query = $"UPDATE `DisciplinePrograms` SET `DisciplineID`= {program.DisciplineId},`Topic`= '{program.Topic}',`Type`= '{program.Type}',`Hours`= {program.Hours} WHERE `ProgramID`= {program.ProgrammId}";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int programId)
        {
            string query = $"DELETE FROM `DisciplinePrograms` WHERE `ProgramID`= {programId}";
            var result = Connection.Query(query);
            return result != null;
        }
    }
}
