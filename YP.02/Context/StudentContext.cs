using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class StudentContext
    {
        public List<Student> LoadStudents(int groupId)
        {
            List<Student> students = new List<Student>();
            string query = $"SELECT * FROM `Students` WHERE GroupID = {groupId}";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = reader.GetInt32(0),
                        GroupId = reader.GetInt32(5),
                        Lastname = reader.GetString(1),
                        Firstname = reader.GetString(2),
                        Patronymic = reader.GetString(3),
                        DismissalDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                    });
                }
            }
            return students;
        }

        public bool Add(Student student)
        {
            string dismissalDate = student.DismissalDate.HasValue
                ? $"'{student.DismissalDate.Value.ToString("yyyy-MM-dd")}'"
                : "NULL";

            string query = $"INSERT INTO `Students`(`GroupID`, `LastName`, `FirstName`, `Patronymic`, `DismissalDate`) " +
                           $"VALUES ({student.GroupId}, '{student.Lastname}', '{student.Firstname}', '{student.Patronymic}', {dismissalDate})";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Student student)
        {
            string dismissalDate = student.DismissalDate.HasValue
                ? $"'{student.DismissalDate.Value.ToString("yyyy-MM-dd")}'"
                : "NULL";

            string query = $"UPDATE `Students` SET " +
                           $"`LastName` = '{student.Lastname}', " +
                           $"`FirstName` = '{student.Firstname}', " +
                           $"`Patronymic` = '{student.Patronymic}', " +
                           $"`DismissalDate` = {dismissalDate} " +
                           $"WHERE `StudentID` = {student.StudentId}";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int studentId)
        {
            string query = $"DELETE FROM `Students` WHERE `StudentID` = {studentId}";
            var result = Connection.Query(query);
            return result != null;
        }
    }
}
