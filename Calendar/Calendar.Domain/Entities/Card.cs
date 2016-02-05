using System;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Domain.Entities
{
    public class Card
    {
        [Key]
        public int      c_ID { get; set; }

        public int      c_DayID { get; set; }
        public string   c_Name { get; set; }
        public int      c_Minutes { get; set; }
        public DateTime c_StartHour { get; set; }
        public int      c_Index { get; set; }
        public bool     c_FixedHour { get; set; }
        public bool     c_Done { get; set; }
    }
}
