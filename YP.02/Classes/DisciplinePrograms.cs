using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class DisciplinePrograms
    {
        public int ProgrammId { get; set; }
        public int DisciplineId { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public int Hours { get; set; }
        public string Topics => $"{Topic}".Trim();
    }
}
