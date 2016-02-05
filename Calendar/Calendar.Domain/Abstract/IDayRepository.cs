using System;
using System.Linq;
using Calendar.Domain.Entities;

namespace Calendar.Domain.Abstract
{
    public interface IDayRepository
    {
        IQueryable<Day> Days { get; }

        int SaveDay(Day date);

        int DeleteDay(Day date);
    }
}