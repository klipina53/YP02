using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
namespace YP._02.Context
{
    public class ConsultationContext
    {
        public List<Consultation> LoadConsultations()
        {
            List<Consultation> consultations = new List<Consultation>();
            string query = @"
        SELECT c.ConsultationID, c.Date, s.Lastname, s.Firstname, s.Patronymic, c.PracticeSubmitted
        FROM `Consultations` c
        JOIN `Students` s ON c.StudentID = s.StudentId";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int consultationId = reader.GetInt32(0);
                    DateTime date = reader.GetDateTime(1);
                    string lastName = reader.GetString(2);
                    string firstName = reader.GetString(3);
                    string patronymic = reader.IsDBNull(4) ? null : reader.GetString(4);
                    string practiceSubmitted = reader.IsDBNull(5) ? null : reader.GetString(5);

                    string fullName = $"{lastName} {firstName} {(patronymic != null ? patronymic : "")}".Trim();

                    consultations.Add(new Consultation
                    {
                        ConsultationID = consultationId,
                        Date = date,
                        StudentFullName = fullName, // Сохраняем полное имя студента
                        PracticeSubmitted = practiceSubmitted
                    });
                }
            }
            return consultations;
        }
        public string GetStudentFullName(int studentId)
        {
            string query = "SELECT Lastname, Firstname, Patronymic FROM Students WHERE StudentId = @studentId";
                using (var reader = Connection.Query(query))
                {
                    if (reader.Read())
                    {
                        string lastName = reader.GetString(0);
                        string firstName = reader.GetString(1);
                        string patronymic = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);

                        return $"{lastName} {firstName} {patronymic}".Trim();
                    }
                }
            
            return string.Empty; // Возврат пустой строки, если студент не найден
        }

        public bool Add(Consultation consultation)
        {
            string query = "INSERT INTO `Consultations` (`Date`, `StudentID`, `PracticeSubmitted`) VALUES (@date, @studentID, @practiceSubmitted)";

          
                var result = Connection.Query(query);
               return result != null;
            

        }

        public bool Update(Consultation consultation)
        {
            string query = $"UPDATE `Consultations` SET `Date` = '{consultation.Date:yyyy-MM-dd HH:mm:ss}', `StudentID` = {consultation.StudentFullName}, `PracticeSubmitted` = {consultation.PracticeSubmitted} WHERE `ConsultationID` = {consultation.ConsultationID}";


            var result = Connection.Query(query);
                return result != null;
            
        }

        public bool Delete(int consultationId)
        {
            string query = $"DELETE FROM `Consultations` WHERE `ConsultationID` = {consultationId}";

            var result = Connection.Query(query);
            return result != null;

        }
    }
}

