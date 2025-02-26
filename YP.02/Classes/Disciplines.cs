using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Disciplines
    {
        public int DisciplineId { get; set; }
        public string Name { get; set; }
    }
    public class DisciplinePrograms
    {
        public int ProgrammId { get; set; }
        public int DisciplineId { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public int Hours { get; set; }
    }
}
