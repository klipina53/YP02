using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP._02.Classes
{
 

        public class Lessons
        {
            public int ID { get; set; } // Первичный ключ, уникальный идентификатор записи
            public string Name { get; set; } // Наименование занятия, может быть null
            public string GroupId { get; set; } // Идентификатор группы, может быть null
            public int? TotalClasses { get; set; } // Общее количество занятий, может быть null
            public int? ConductedHours { get; set; } // Проведенное количество часов, может быть null
        }



    
}
