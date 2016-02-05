using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Calendar.Domain.Entities;

namespace Calendar.WebUI.Models
{
    public class DayViewModel
    {
        public List<Day> lstDays; 
    }

    public class DayFormViewModel
    {
        public int Month;
        public int Day;
    }

    public class EditDayModel
    {
        public Day Date;
        public List<Card> lstCards;
    }

    public class CardModel
    {
        public int cID { get; set; }
        public int DayId { get; set; }
        public int Index  { get; set; }
        public string Name { get; set; }
        public int Minutes { get; set; }
        public DateTime StartHour { get; set; }
        public bool Fixed { get; set; }
        public bool Done { get; set; }

    }
}