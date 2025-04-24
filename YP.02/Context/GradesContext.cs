using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class GradesContext
    {
        public List<Grades> LoadGrades()
        {
            List<Grades> gradesList = new List<Grades>();
            string query = "SELECT * FROM `Grades`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int gradeId = reader.GetInt32(0);
                    string className = reader.GetString(1);
                    int gradeValue = reader.GetInt32(2);
                    int studentId = reader.GetInt32(3);

                    gradesList.Add(new Grades
                    {
                        GradeID = gradeId,
                        Class = className,
                        Grade = gradeValue,
                        StudentID = studentId
                    });
                }
            }
            return gradesList;
        }

        public bool Add(Grades grade)
        {
            string query = $"INSERT INTO `Grades`(`Class`, `Grade`, `StudentID`) VALUES ('{grade.Class}', {grade.Grade}, {grade.StudentID})";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Grades grade)
        {
            string query = $"UPDATE `Grades` SET `Class` = '{grade.Class}', `Grade` = {grade.Grade}, `StudentID` = {grade.StudentID} WHERE `GradeID` = {grade.GradeID}";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int gradeId)
        {
            string query = $"DELETE FROM `Grades` WHERE `GradeID` = {gradeId}";

            var result = Connection.Query(query);
            return result != null;
        }
    }

}
