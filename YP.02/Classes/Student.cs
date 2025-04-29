using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Student
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string FullName => $"{Firstname} {Lastname} {Patronymic}".Trim();
    }
}
