using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Consultation
    {
      public int  ConsultationID { get; set; }
        public DateTime Date { get; set; }
        public string StudentFullName { get; set; }
        public string PracticeSubmitted { get; set; }
    }
}
