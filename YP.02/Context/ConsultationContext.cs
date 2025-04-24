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
            string query = "SELECT ConsultationID, Date, StudentFullName, PracticeSubmitted FROM `Consultations`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int consultationId = reader.GetInt32(0);
                    DateTime date = reader.GetDateTime(1);
                    string studentFullName = reader.GetString(2);
                    string practiceSubmitted = reader.IsDBNull(3) ? null : reader.GetString(3);

                    consultations.Add(new Consultation
                    {
                        ConsultationID = consultationId,
                        Date = date,
                        StudentFullName = studentFullName,
                        PracticeSubmitted = practiceSubmitted
                    });
                }
            }
            return consultations;
        }

        public bool Add(Consultation consultation)
        {
            string query = $"INSERT INTO `Consultations` (`Date`, `StudentFullName`, `PracticeSubmitted`) VALUES ('{consultation.Date:yyyy-MM-dd HH:mm:ss}', '{consultation.StudentFullName}', '{consultation.PracticeSubmitted}')";
            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(Consultation consultation)
        {
            string query = $"UPDATE `Consultations` SET `Date` = '{consultation.Date:yyyy-MM-dd HH:mm:ss}', `StudentFullName` = '{consultation.StudentFullName}', `PracticeSubmitted` = '{consultation.PracticeSubmitted}' WHERE `ConsultationID` = {consultation.ConsultationID}";
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
