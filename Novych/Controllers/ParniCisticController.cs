using Novych.Models;
using Novych.Models.Common;
using Novych.Models.ParniCistic;
using Novych.Resources;
using System;
using System.Web.Mvc;

namespace Novych.Controllers
{
    public class ParniCisticController : Controller
    {
        private static MailSender ms { get; set; } = new MailSender();

        public ActionResult Index(int? year, int? month)
        {
            if (year == null || year < 2015 || month == null)
            {
                year = DateTime.Today.Year;
                month = DateTime.Today.Month;
            }

            DateTime viewMonth = new DateTime(year.Value, month.Value, 1);

            CalendarModel model = new CalendarModel(viewMonth);

            Logger.log(LogClass.DEBUG, "Index: Show calendar [" + year + "-" + month + "]");

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult getCalendar(string yearStr, string monthStr, string status)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            int.TryParse(yearStr, out year);
            int.TryParse(monthStr, out month);

            DateTime viewMonth = new DateTime(year, month, 1);

            CalendarModel model = new CalendarModel(viewMonth);

            DateTime prevMonth = viewMonth.AddMonths(-1);
            DateTime nextMonth = viewMonth.AddMonths(1);

            var returnValue = new
            {
                calendar = model.ToHtml().ToString(),
                actualMonth = model.GetMonthYear(),
                prevYear = prevMonth.Year,
                prevMonth = prevMonth.Month,
                nextYear = nextMonth.Year,
                nextMonth = nextMonth.Month,
                status = status
            };

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReservation([Bind(Include = "Date, Email, Phone, DeleteDesc, ViewMonth")] ReservationModel res)
        {
            String status = "";

            if (ModelState.IsValid)
            {
                ReservationModel resCurrent = new ReservationModel(res.Date, res.ViewMonth);

                if (resCurrent.Res == null)
                {
                    status = "Rezervace neexistuje";
                }

                if (resCurrent.Email == res.Email && resCurrent.Phone == res.Phone)
                {
                    resCurrent.delete(res.DeleteDesc);

                    Logger.log(LogClass.INFO, "Deleted reservation [" + res.Date.ToString("dd.MM.yyyy") + ", " + res.Email + ", " + res.Phone + "]");

                    ms.Send(new MailDataModel(MailDataModel.MailDataType.DELETE, resCurrent));

                    status = "Rezervace na " + res.Date.ToString("dd.MM.yyyy") + " byla zrušena, potvrzení Vám přijde na Váš e-mail";
                }
                else if (res.Email == NovychResources.AdminMail && res.Phone == NovychResources.AdminPhone)
                {
                    resCurrent.delete(res.DeleteDesc);

                    Logger.log(LogClass.WARNING, "ADMIN - Deleted reservation [" + res.Date.ToString("dd.MM.yyyy") + ", " + res.Email + ", " + res.Phone + "]");

                    ms.Send(new MailDataModel(MailDataModel.MailDataType.DELETE_ADMIN, resCurrent));

                    status = "Rezervace na " + res.Date.ToString("dd.MM.yyyy") + " byla zrušena";
                }
                else
                {
                    Logger.log(LogClass.WARNING, "Delete reservation failed [" + res.Date.ToString("dd.MM.yyyy") + ", " + res.Email + ", " + res.Phone + "] [" + resCurrent.Email + "]x[" + res.Email + "] - [" + resCurrent.Phone + "]x[" + res.Phone + "]");

                    status = "Nesprávný e-mail nebo telefoní číslo";
                }

                return getCalendar(res.ViewMonth.Year.ToString(), res.ViewMonth.Month.ToString(), status);
            }

            Logger.log(LogClass.WARNING, "Delete - Model is not valid");

            return PartialView("_DeleteReservation", res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReservation([Bind(Include = "Date, Name, Email, Address, Phone, CreateDesc, ViewMonth")] ReservationModel res)
        {
            if (ModelState.IsValid)
            {
                String status = "";

                if (res.Date <= DateTime.Today)
                {
                    Logger.log(LogClass.WARNING, "Reservation on wrong date [" + res.Date.ToString("dd.MM.yyyy") + ", " + res.Email + "]");
                }

                ReservationModel ResCurrent = new ReservationModel(res.Date, res.ViewMonth);
                if (ResCurrent.Res == null)
                {

                    res.IpAddress = HttpContext.Request.UserHostAddress;
                    res.Identifier = HttpContext.Request.UserAgent;

                    res.put();

                    Logger.log(LogClass.INFO, "Created reservation [" + res.Date.ToString("dd.MM.yyyy") + ", " + res.Email + ", " + res.Phone + "]");

                    ms.Send(new MailDataModel(MailDataModel.MailDataType.CREATE, res));

                    status = "Rezervace na " + res.Date.ToString("dd.MM.yyyy") + " byla vytvořena, detaily Vám přijdou na Váš e-mail";
                }
                else
                {
                    status = "Rezervace na " + res.Date.ToString("dd.MM.yyyy") + " nebyla vytvořena";
                }

                return getCalendar(res.Date.Year.ToString(), res.Date.Month.ToString(), status);
            }

            Logger.log(LogClass.WARNING, "Create - Model is not valid");

            return PartialView("_CreateReservation", res);
        }
    }
}