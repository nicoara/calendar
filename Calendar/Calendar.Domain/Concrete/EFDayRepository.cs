using System;
using System.Collections.Generic;
using System.Linq;
using Calendar.Domain.Abstract;
using Calendar.Domain.Entities;

namespace Calendar.Domain.Concrete
{
    public class EFDayRepository : IDayRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Day> Days
        {
            get { return context.Days; }
        }

        public int SaveDay(Day date)
        {
            if (date.d_ID == 0)
            {
                List<Day> listDay = context.Days
                    .Where(d => d.d_Date == date.d_Date)
                    .ToList();

                if (listDay.Count == 0)
                {
                    context.Days.Add(date);
                    context.SaveChanges();   
                }
                return date.d_ID;
            }
            else
            {
                return date.d_ID;
            }
        }

        public int DeleteDay(Day date)
        {
            Day dbEntry = context.Days.Find(date.d_ID);
            context.Days.Remove(dbEntry);
            context.SaveChanges(); 
            return 0;
        }
    }
}
