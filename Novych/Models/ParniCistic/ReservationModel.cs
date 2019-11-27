using System;
using System.Linq;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Novych.Models.Database;
using Novych.Resources;

namespace Novych.Models.ParniCistic
{
    public class ReservationModel
    {
        private NovychDbContext db = new NovychDbContext();

        [Key]
        public int ID { get; set; }

        //[DataType(DataType.Date)] // Does not work in Chrome
        [Display(ResourceType = typeof(NovychResources), Name = "DbDate")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbDateValidation")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbEmail")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbEmailValidation")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbPhone")]
        [Required(ErrorMessageResourceType = typeof(NovychResources), ErrorMessageResourceName = "DbPhoneValidation")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbAddress")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDateCreated")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDesc")]
        [DataType(DataType.MultilineText)]
        public string CreateDesc { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDateDeleted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss.fff}", ApplyFormatInEditMode = true)]
        public DateTime? DeleteDate { get; set; }

        [Display(ResourceType = typeof(NovychResources), Name = "DbDesc")]
        [DataType(DataType.MultilineText)]
        public string DeleteDesc { get; set; }

        public string IpAddress { get; set; }
        public string Identifier { get; set; }

        public DateTime ViewMonth { get; set; }

        public ParniCisticReservation Res;

        public enum ResType { ACTUAL, DELETED };

        public ReservationModel(DateTime date, ResType resType)
        {
            this.Date = date;

            get(resType);
        }

        public ReservationModel(DateTime date, DateTime viewMonth)
        {
            this.Date = date;
            this.ViewMonth = viewMonth;

            get(ResType.ACTUAL);
        }

        public ReservationModel(DateTime date, DateTime viewMonth, ParniCisticReservation res)
        {
            this.Date = date;
            this.ViewMonth = viewMonth;
            this.Res = res;
        }

        public ReservationModel()
        {
            this.CreateDate = DateTime.Now;

            this.DeleteDate = null;
            this.DeleteDesc = "";
        }

        public void get(ResType resType)
        {
            if (resType == ResType.ACTUAL)
            {
                Res = db.Reservations.Where(x => x.Date == Date && x.DeleteDate == null).FirstOrDefault<ParniCisticReservation>();
            }
            else
            {
                Res = db.Reservations.Where(x => x.Date == Date).FirstOrDefault<ParniCisticReservation>();
            }

            if (Res != null)
            {
                Email = Res.Email;
                Phone = Res.Phone;
                Name = Res.Name;
                Address = Res.Address;

                CreateDate = Res.CreateDate;
                CreateDesc = Res.CreateDesc;

                DeleteDate = Res.DeleteDate;
                DeleteDesc = Res.DeleteDesc;

                IpAddress = Res.IpAddress;
                Identifier = Res.Identifier;
            }
        }

        public void put()
        {
            if (Res == null)
            {
                CreateDate = DateTime.Now;

                Res = new ParniCisticReservation();
                Res.Date = Date;
                Res.Email = Email;
                Res.Phone = Phone;
                Res.Name = Name;
                Res.Address = Address;

                Res.CreateDate = CreateDate;
                Res.CreateDesc = CreateDesc;

                Res.DeleteDate = DeleteDate;
                Res.DeleteDesc = DeleteDesc;

                Res.IpAddress = IpAddress;
                Res.Identifier = Identifier;

                db.Reservations.Add(Res);
            }
            else
            {
                Res.Date = Date;
                Res.Email = Email;
                Res.Phone = Phone;
                Res.Name = Name;
                Res.Address = Address;

                Res.CreateDate = CreateDate;
                Res.CreateDesc = CreateDesc;

                Res.DeleteDate = DeleteDate;
                Res.DeleteDesc = DeleteDesc;

                Res.IpAddress = IpAddress;
                Res.Identifier = Identifier;

            }

            db.SaveChanges();
        }

        public void delete(String DeleteDesc)
        {
            DeleteDate = DateTime.Now;
            Res.DeleteDate = DeleteDate;

            this.DeleteDesc = DeleteDesc;
            Res.DeleteDesc = DeleteDesc;

            db.SaveChanges();
        }

        public void deleteOld()
        {
            db.Reservations.Remove(Res);

            db.SaveChanges();
        }

        public HtmlString ToHtml()
        {
            StringBuilder output = new StringBuilder();

            output.Append("<div class=\"day");

            if (Date == DateTime.Today)
            {
                output.Append(" today");
            }
            else if ((int)Date.DayOfWeek == 0 || (int)Date.DayOfWeek == 6)
            {
                output.Append(" weekend");
            }
            if (Date > DateTime.Today)
            {
                output.Append(" open-dialog");
            }
            if (Date.Month != ViewMonth.Month)
            {
                output.Append(" anotherMonth");
            }
            if (Res != null && Date > DateTime.Today)
            {
                output.Append(" reserved");
            }
            output.Append("\"");

            output.Append(" data-date=\"");
            output.Append(Date.ToString("dd.MM.yyyy"));
            output.Append("\"");

            output.Append(" data-type=\"");
            if (Date > DateTime.Today)
            {
                if (Res == null)
                {
                    output.Append("create");
                }
                else
                {
                    output.Append("delete");
                }
            }
            output.Append("\"");

            output.Append(">");
            output.Append(Date.Day);
            output.Append("</div>");

            return new HtmlString(output.ToString());
        }
    }
}