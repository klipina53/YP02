using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class InstructorLoadsContext
    {
        public List<InstructorLoads> LoadInstructorLoads()
        {
            List<InstructorLoads> loadsList = new List<InstructorLoads>();
            string query = "SELECT * FROM `InstructorLoads`";

            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    int loadId = reader.GetInt32(0);
                    int? disciplineId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
                    int? groupId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                    int? lectureHours = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                    int? practicalHours = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                    int? consultationHours = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                    int? projectHours = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);
                    int? examHours = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7);

                    loadsList.Add(new InstructorLoads
                    {
                        LoadID = loadId,
                        DisciplineID = disciplineId,
                        GroupID = groupId,
                        LectureHours = lectureHours,
                        PracticalHours = practicalHours,
                        ConsultationHours = consultationHours,
                        ProjectHours = projectHours,
                        ExamHours = examHours
                    });
                }
            }
            return loadsList;
        }

        public bool Add(InstructorLoads load)
        {
            string query = $"INSERT INTO `InstructorLoads`(`DisciplineID`, `GroupID`, `LectureHours`, `PracticalHours`, `ConsultationHours`, `ProjectHours`, `ExamHours`) VALUES " +
                           $"({load.DisciplineID}, {load.GroupID}, {load.LectureHours}, {load.PracticalHours}, {load.ConsultationHours}, {load.ProjectHours}, {load.ExamHours})";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Update(InstructorLoads load)
        {
            string query = $"UPDATE `InstructorLoads` SET `DisciplineID` = {load.DisciplineID}, `GroupID` = {load.GroupID}, " +
                           $"`LectureHours` = {load.LectureHours}, `PracticalHours` = {load.PracticalHours}, " +
                           $"`ConsultationHours` = {load.ConsultationHours}, `ProjectHours` = {load.ProjectHours}, " +
                           $"`ExamHours` = {load.ExamHours} WHERE `LoadID` = {load.LoadID}";

            var result = Connection.Query(query);
            return result != null;
        }

        public bool Delete(int loadId)
        {
            string query = $"DELETE FROM `InstructorLoads` WHERE `LoadID` = {loadId}";

            var result = Connection.Query(query);
            return result != null;
        }
    }

}
