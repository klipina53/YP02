using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int DisciplineId { get; set; }
        public Disciplines Discipline { get; set; }

        public int AbsenceMinutes { get; set; }

        public string ExcuseFilePath { get; set; }
    }
    public class Grade
    {
        public int GradeId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int DisciplineId { get; set; }
        public Disciplines Discipline { get; set; }

        public string GradeValue { get; set; } // Оценка 2, 3, 4, 5
    }
}
