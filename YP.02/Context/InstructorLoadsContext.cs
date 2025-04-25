using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class InstructorLoadsContext
    {
        public List<InstructorLoads> LoadInstructorLoads()
        {
            List<InstructorLoads> loadsList = new List<InstructorLoads>();
            string query = @"SELECT il.LoadID, il.DisciplineID, d.Name AS DisciplineName, il.GroupID, g.Name AS GroupName, 
                            il.LectureHours, il.PracticalHours, il.ConsultationHours, il.ProjectHours, il.ExamHours
                            FROM InstructorLoads il
                            LEFT JOIN Disciplines d ON il.DisciplineID = d.DisciplineID
                            LEFT JOIN `Groups` g ON il.GroupID = g.GroupID"; 

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    loadsList.Add(new InstructorLoads
                    {
                        LoadID = reader.GetInt32(0),
                        DisciplineID = reader.GetInt32(1),
                        DisciplineName = reader.GetString(2),
                        GroupID = reader.GetInt32(3),
                        GroupName = reader.GetString(4),
                        LectureHours = reader.GetInt32(5),
                        PracticalHours = reader.GetInt32(6),
                        ConsultationHours = reader.GetInt32(7),
                        ProjectHours = reader.GetInt32(8),
                        ExamHours = reader.GetInt32(9)
                    });
                }
            }
            return loadsList;
        }

        public bool Add(InstructorLoads load)
        {
            // Формируем значения для полей, обрабатывая NULL
            string disciplineId = load.DisciplineID.HasValue ? load.DisciplineID.ToString() : "NULL";
            string groupId = load.GroupID.HasValue ? load.GroupID.ToString() : "NULL";
            string lectureHours = load.LectureHours.HasValue ? load.LectureHours.ToString() : "NULL";
            string practicalHours = load.PracticalHours.HasValue ? load.PracticalHours.ToString() : "NULL";
            string consultationHours = load.ConsultationHours.HasValue ? load.ConsultationHours.ToString() : "NULL";
            string projectHours = load.ProjectHours.HasValue ? load.ProjectHours.ToString() : "NULL";
            string examHours = load.ExamHours.HasValue ? load.ExamHours.ToString() : "NULL";

            // Формируем SQL-запрос
            string query = $"INSERT INTO `InstructorLoads` (`DisciplineID`, `GroupID`, `LectureHours`, `PracticalHours`, `ConsultationHours`, `ProjectHours`, `ExamHours`) VALUES " +
                          $"({disciplineId}, {groupId}, {lectureHours}, {practicalHours}, {consultationHours}, {projectHours}, {examHours})";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(InstructorLoads load)
        {
            // Формируем значения для полей, обрабатывая NULL
            string disciplineId = load.DisciplineID.HasValue ? load.DisciplineID.ToString() : "NULL";
            string groupId = load.GroupID.HasValue ? load.GroupID.ToString() : "NULL";
            string lectureHours = load.LectureHours.HasValue ? load.LectureHours.ToString() : "NULL";
            string practicalHours = load.PracticalHours.HasValue ? load.PracticalHours.ToString() : "NULL";
            string consultationHours = load.ConsultationHours.HasValue ? load.ConsultationHours.ToString() : "NULL";
            string projectHours = load.ProjectHours.HasValue ? load.ProjectHours.ToString() : "NULL";
            string examHours = load.ExamHours.HasValue ? load.ExamHours.ToString() : "NULL";

            // Формируем SQL-запрос
            string query = $"UPDATE `InstructorLoads` SET " +
                          $"`DisciplineID` = {disciplineId}, " +
                          $"`GroupID` = {groupId}, " +
                          $"`LectureHours` = {lectureHours}, " +
                          $"`PracticalHours` = {practicalHours}, " +
                          $"`ConsultationHours` = {consultationHours}, " +
                          $"`ProjectHours` = {projectHours}, " +
                          $"`ExamHours` = {examHours} " +
                          $"WHERE `LoadID` = {load.LoadID}";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int loadId)
        {
            string query = $"DELETE FROM `InstructorLoads` WHERE `LoadID` = {loadId}";

            var result = Connection.Query(query);
            return result != null;
        }

        // Методы для заполнения ComboBox
        public List<Disciplines> LoadDisciplines()
        {
            List<Disciplines> disciplines = new List<Disciplines>();
            string query = "SELECT DisciplineID, Name FROM Disciplines";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    disciplines.Add(new Disciplines
                    {
                        DisciplineId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            return disciplines;
        }

        public List<Groups> LoadGroups()
        {
            List<Groups> groups = new List<Groups>();
            string query = "SELECT GroupID, Name FROM `Groups`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    groups.Add(new Classes.Groups
                    {
                        GroupID  = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            return groups;
        }
    }

}
