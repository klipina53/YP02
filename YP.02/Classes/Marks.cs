using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes // Используйте правильное пространство имен
{
    public class Marks
    {
        public int Id { get; set; } // Используйте заглавную букву
        public string Mark { get; set; } // Используйте заглавную букву
        public int DisciplineProgramId { get; set; } // Используйте заглавную букву
        public int StudentId { get; set; } // Используйте заглавную букву
        public string Description { get; set; } // Используйте заглавную букву
    }
}
