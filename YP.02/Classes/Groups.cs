using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Groups
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
    }
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Firstname { get; set; }

        public string Patronymic { get; set; }

        public DateTime? DismissalDate { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; } // Связь с группой

        public ICollection<Grade> Grades { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
