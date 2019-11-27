using Novych.Models.Database;
using Novych.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Novych.Models.ParniCistic
{
    public class CalendarModel
    {
        private NovychDbContext db = new NovychDbContext();

        public List<ReservationModel> Calendar { get; set; }
        public DateTime ViewMonth { get; set; }

        public CalendarModel()
        {
        }

        public CalendarModel(DateTime viewMonth)
        {
            Calendar = new List<ReservationModel>(42);
            this.ViewMonth = viewMonth;

            DateTime startDate = viewMonth.AddMonths(-1);
            DateTime endDate = viewMonth.AddMonths(2);

            var reservationList = db.Reservations.Where(x => x.Date > startDate && x.Date < endDate && x.DeleteDate == null);

            int offset = -(((int)viewMonth.DayOfWeek == 0) ? 7 : (int)viewMonth.DayOfWeek);

            for (int i = offset; i <= (42 + offset); i++)
            {
                DateTime dayDate = viewMonth.AddDays(i);
                ParniCisticReservation res = reservationList.Where(x => x.Date == dayDate).FirstOrDefault();
                Calendar.Add(new ReservationModel(dayDate, viewMonth, res));
            }
        }

        public ReservationModel ResAt(int i)
        {
            return Calendar.ElementAt(i);
        }

        public HtmlString ToHtml()
        {
            StringBuilder output = new StringBuilder();

            output.Append("<table width = \"300\">");
            output.Append("<tr class=\"tableHeader\">");
            output.Append("<th class=\"day\">" + NovychResources.Monday + "</th><th class=\"day\">" + NovychResources.Tuesday + "</th><th class=\"day\">" + NovychResources.Wednesday + "</th><th class=\"day\">" + NovychResources.Thursday + "</th><th class=\"day\">" + NovychResources.Friday + "</th><th class=\"day\">" + NovychResources.Saturday + "</th><th class=\"day\">" + NovychResources.Sunday + "</th></tr>");

            for (int i = 0; i < 6; i++)
            {
                output.Append("<tr>");
                for (int j = 1; j <= 7; j++)
                {
                    output.Append("<td>" + ResAt(i * 7 + j).ToHtml() + "</td>");
                }
                output.Append("</tr>");
            }

            output.Append("</table>");

            return new HtmlString(output.ToString());
        }


        public string GetMonthYear()
        {
            string output = ViewMonth.ToString("MMMM - yyyy");

            return char.ToUpper(output[0]) + output.Substring(1);
        }
    }
}