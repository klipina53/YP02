using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Instructors
    {
        public int InstructorId { get; set; }

        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }

        public string Login { get; set; }
        public string Password { get; set; } // Теперь соответствует полю из базы данных

        public ICollection<InstructorsLoad> TeacherLoads { get; set; } // Связь с нагрузкой преподавателя
    }

    public class InstructorsLoad
    {
        public int TeacherLoadId { get; set; }

        public int TeacherId { get; set; }
        public Instructors Teacher { get; set; }

        public int DisciplineId { get; set; }
        public Disciplines Discipline { get; set; }

        public int GroupId { get; set; }
        public Groups Group { get; set; }

        public int LectureHours { get; set; }
        public int PracticeHours { get; set; }
        public int ConsultationHours { get; set; }
        public int ProjectHours { get; set; }
        public int ExamHours { get; set; }
    }
}
