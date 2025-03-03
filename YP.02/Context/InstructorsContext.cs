using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class InstructorsContext
    {
        public List<Instructors> LoadInstructors()
        {
            List<Instructors> instructors = new List<Instructors>();
            string query = "SELECT * FROM `Instructors`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    instructors.Add(new Instructors
                    {
                        InstructorId = reader.GetInt32(0),
                        Lastname = reader.GetString(1),
                        Firstname = reader.GetString(2),
                        Patronymic = reader.GetString(3),
                        Login = reader.GetString(4),
                        Password = reader.GetString(5)
                    });
                }
            }
            return instructors;
        }

        public bool Add(Instructors instructor)
        {
            string query = $"INSERT INTO `Instructors`(`Lastname`, `Firstname`, `Patronymic`, `Login`, `PasswordHash`) " +
                           $"VALUES ('{instructor.Lastname}', '{instructor.Firstname}', '{instructor.Patronymic}', '{instructor.Login}', '{instructor.Password}')";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Instructors instructor)
        {
            string query = $"UPDATE `Instructors` SET " +
                           $"`Lastname` = '{instructor.Lastname}', " +
                           $"`Firstname` = '{instructor.Firstname}', " +
                           $"`Patronymic` = '{instructor.Patronymic}', " +
                           $"`Login` = '{instructor.Login}', " +
                           $"`PasswordHash` = '{instructor.Password}' " +
                           $"WHERE `InstructorID` = {instructor.InstructorId}";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int instructorId)
        {
            string query = $"DELETE FROM `Instructors` WHERE `InstructorID` = {instructorId}";
            var result = Connection.Query(query);
            return result != null;
        }
    }
}
