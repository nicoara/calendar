using System;
using System.ComponentModel.DataAnnotations;

namespace Calendar.Domain.Entities
{
    public class Day
    {
        [Key]
        public int      d_ID { get; set; }

        public DateTime d_Date { get; set; }
    }
}
