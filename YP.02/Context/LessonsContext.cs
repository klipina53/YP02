using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class LessonsContext
    {
        public List<Lessons> LoadLessons(int disciplineId)
        {
        List<Lessons> lessons = new List<Lessons>();
        string query = $"SELECT * FROM `Lessons` WHERE `DisciplineId` = {disciplineId}";

        using (var reader = Connection.Query(query))
        {
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.IsDBNull(1) ? null : reader.GetString(1);
                string groupId = reader.IsDBNull(2) ? null : reader.GetString(2);
                int? totalClasses = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                int? conductedHours = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);

                lessons.Add(new Lessons
                {
                    ID = id,
                    Name = name,
                    GroupId = groupId,
                    TotalClasses = totalClasses,
                    ConductedHours = conductedHours
                });
            }
        }
        return lessons;
    }

    public bool Add(Lessons lesson)
    {
        string query = $"INSERT INTO `Lessons`(`Name`, `GroupId`, `TotalClasses`, `ConductedHours`, `DisciplineId`) VALUES ('{lesson.Name}', '{lesson.GroupId}', {(lesson.TotalClasses.HasValue ? lesson.TotalClasses.Value.ToString() : "NULL")}, {(lesson.ConductedHours.HasValue ? lesson.ConductedHours.Value.ToString() : "NULL")}, {lesson.DisciplineId})";
        var result = Connection.Query(query);
        return result != null;
    }

    public bool Update(Lessons lesson)
    {
        string query = $"UPDATE `Lessons` SET `Name` = '{lesson.Name}', `GroupId` = '{lesson.GroupId}', `TotalClasses` = {(lesson.TotalClasses.HasValue ? lesson.TotalClasses.Value.ToString() : "NULL")}, `ConductedHours` = {(lesson.ConductedHours.HasValue ? lesson.ConductedHours.Value.ToString() : "NULL")} WHERE `ID` = {lesson.ID}";
        var result = Connection.Query(query);
        return result != null;
    }

    public bool Delete(int lessonId)
    {
        string query = $"DELETE FROM `Lessons` WHERE `ID` = {lessonId}";
        var result = Connection.Query(query);
        return result != null;
    }
    }
}

