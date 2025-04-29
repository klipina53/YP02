using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class MarksContext
    {
        // Метод для загрузки оценок из базы данных
        public List<YP._02.Classes.Marks> LoadMarks()
        {
            List<YP._02.Classes.Marks> marksList = new List<YP._02.Classes.Marks>();
            string query = "SELECT id, mark, disciplineProgramId, studentId, description FROM `Marks`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    marksList.Add(new YP._02.Classes.Marks
                    {
                        Id = reader.GetInt32(0),
                        Mark = reader.GetString(1),
                        DisciplineProgramId = reader.GetInt32(2),
                        StudentId = reader.GetInt32(3),
                        Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
            }
            return marksList;
        }

        // Метод для добавления новой оценки
        public bool Add(YP._02.Classes.Marks marks)
        {
            string query = $"INSERT INTO `Marks` (`mark`, `disciplineProgramId`, `studentId`, `description`) " +
                           $"VALUES ('{marks.Mark}', {marks.DisciplineProgramId}, {marks.StudentId}, '{marks.Description}')";
            var result = Connection.Query(query);
            return result != null;
        }

        // Метод для обновления существующей оценки
        public bool Update(YP._02.Classes.Marks marks)
        {
            string query = $"UPDATE `Marks` SET `mark` = '{marks.Mark}', `disciplineProgramId` = {marks.DisciplineProgramId}, " +
                           $"studentId = {marks.StudentId}, `description` = '{marks.Description}' WHERE `id` = {marks.Id}";
            var result = Connection.Query(query);
            return result != null;
        }

        // Метод для удаления оценки
        public bool Delete(int marksId)
        {
            string query = $"DELETE FROM `Marks` WHERE `id` = {marksId}";
            var result = Connection.Query(query);
            return result != null;
        }

        // Метод для загрузки студентов
        public List<Student> LoadStudents()
        {
            List<Student> studentList = new List<Student>();
            string query = "SELECT StudentId, Lastname, Firstname, Patronymic FROM `Students`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    studentList.Add(new Student
                    {
                        StudentId = reader.GetInt32(0),
                        Lastname = reader.GetString(1),
                        Firstname = reader.GetString(2),
                        Patronymic = reader.GetString(3)
                    });
                }
            }
            return studentList;
        }

        // Метод для загрузки дисциплин
        public List<YP._02.Classes.DisciplinePrograms> LoadDisciplinePrograms()
        {
            List<YP._02.Classes.DisciplinePrograms> disciplineList = new List<YP._02.Classes.DisciplinePrograms>();
            string query = "SELECT DisciplineId, Topic, Type, Hours FROM `DisciplinePrograms`"; // Убедитесь, что есть все нужные столбцы

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    disciplineList.Add(new YP._02.Classes.DisciplinePrograms
                    {
                        DisciplineId = reader.GetInt32(0),
                        Topic = reader.GetString(1),
                        Type = reader.GetString(2),
                        Hours = reader.GetInt32(3)
                    });
                }
            }
            return disciplineList;
        }
    }
}


