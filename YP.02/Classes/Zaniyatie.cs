using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
    public class Zaniyatie
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public int MinutesMissed { get; set; }

        public string ExplanationText { get; set; } // Новое свойство для объяснительной
    }
}
