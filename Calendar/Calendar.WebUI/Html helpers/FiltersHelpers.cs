using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Calendar.WebUI.Html_helpers
{
    public static class FiltersHelpers
    {
        public static string cleanDay(this HtmlHelper html, DateTime date)
        {
            List<string> months = new List<string>{"Zero","Ianuarie", "Februarie", "Martie", "Aprilie", "Mai",
                "Iunie", "Iulie","August", "Septembrie", "Octombrie", "Noiembrie", "Decembrie"};

            return date.Day.ToString() + " " + months[date.Month];
        }


    }
}