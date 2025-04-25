using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class InstructorLoads
    {
        public int LoadID { get; set; }
        public int? DisciplineID { get; set; }
        public string DisciplineName { get; set; } 
        public int? GroupID { get; set; }
        public string GroupName { get; set; } 
        public int? LectureHours { get; set; }
        public int? PracticalHours { get; set; }
        public int? ConsultationHours { get; set; }
        public int? ProjectHours { get; set; }
        public int? ExamHours { get; set; }
    }
}

