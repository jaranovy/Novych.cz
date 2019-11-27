using Novych.Resources;
using System;
using System.Text;
using System.Web;

namespace Novych.Models.ParniCistic
{
    public class MailDataModel
    {
        public enum MailDataType { CREATE, DELETE, DELETE_ADMIN }

        public string To { get; set; }
        public string Bcc { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string File { get; set; }

        public MailDataType Type { get; set; }
        public DateTime ReservationDate { get; set; }

        public MailDataModel(MailDataType type, ReservationModel res)
        {
            this.To = res.Email;
            this.Bcc = NovychResources.AdminMail;
            this.From = NovychResources.AppMail;
            this.File = null;
            this.Type = type;
            ReservationDate = res.Date;

            StringBuilder buffer = new StringBuilder(100);

            if (type == MailDataType.CREATE)
            {
                this.Subject = "Parní Čistič Kärcher - Vytvořena nová rezervace na " + res.Date.ToString("dd.MM.yyyy");

                buffer.Append("<p>Dobrý den,<p>");

                buffer.Append("<p>Tímto potvrzujeme vytvoření Vaší rezervace na pronájem parního čističe Kärcher na " + res.Date.ToString("dd.MM.yyyy") + ".</p>");

                buffer.Append("<p style=\"margin-bottom:0px\">Podmínky zapůjčení:</p>");
                buffer.Append("<ul style=\"list-style-type:circle; margin-top:0px\">");
                buffer.Append("<li>Parní čistič Vám bude pronajat na 1 den, pokud není provedena rezervace na více, po sobě jdoucích dní.</li>");
                buffer.Append("<li>Vyzvednutí i vrácení čističe probíhá na adrese uvedené níže, mezi 7 a 9 hodinou ranní.</li>");
                buffer.Append("<li>Parní čistič je předáván v originální krabici a v Ikea tašce. Lze jej tedy pohodlně odnést.</li>");
                buffer.Append("<li>Poplatek za zapůjčení činí 200 Kč na den.</li>");
                buffer.Append("<li>Vratná kauce za pronájem je stanovena na 500 Kč a vybírá se při vyzvednutí čističe.</li>");
                buffer.Append("<li>Při vyzvednutí parního čističe Vám bude dána k podpisu nájemní smlouva. Její vzor nalzente <a href=\"http://" + NovychResources.AppAddress + "/Content/Data/smlouva-o-najmu.pdf\">zde</a>.</li>");
                buffer.Append("<li>Před prvním použitím parního čístiče si prosím přečtěte <a href=\"http://" + NovychResources.AppAddress + "/Content/Data/SC1020_manual.pdf\">tento návod</a>.</li>");
                buffer.Append("</ul>");

                buffer.Append("<p style=\"margin-bottom:0px\">Parní čistič si prosím vyzvedněte na adrese:</p>");
                buffer.Append("<ul style=\"list-style-type:none; margin-top:0px\">");
                buffer.Append("<li>Jaroslav Nový</li>");
                buffer.Append("<li>Slévačská 905/32, byt č. 46</li>");
                buffer.Append("<li>Praha 9</li>");
                buffer.Append("<li>Tel: 734 580 323</li>");
                buffer.Append("</ul>");

                buffer.Append("<p style=\"margin-bottom:0px\">Vaše údaje zadané při vytvoření rezervace:</p>");
                buffer.Append("<ul style=\"list-style-type:none; margin-top:0px\">");
                buffer.Append("<li>Jméno: " + res.Name + "</li>");
                buffer.Append("<li>Adresa: " + res.Address + "</li>");
                buffer.Append("<li>E-mail: " + res.Email + "</li>");
                buffer.Append("<li>Tel: " + res.Phone + "</li>");
                buffer.Append("<li>Poznámka: " + res.CreateDesc + "</li>");
                buffer.Append("</ul>");
                buffer.Append("</br>");
                buffer.Append("<p style=\"margin-bottom:0px\">Rezervaci je možné zrušit kliknutím na Vámi rezervované datum na <a href=\"http://" + NovychResources.AppAddress + "\">" + NovychResources.AppAddress + "</a></p>");
                buffer.Append("</br>");
            }

            if (type == MailDataType.DELETE)
            {
                this.Subject = "Parní Čistič Kärcher - Zrušení rezervace na " + res.Date.ToString("dd.MM.yyyy");

                buffer.Append("<p>Dobrý den,<p>");
                buffer.Append("<p>Tímto vám potvrzujeme zrušení Vaší rezervace na pronájem parního čističe Kärcher na " + res.Date.ToString("dd.MM.yyyy") + ".<p>");
                buffer.Append("<p>Vaše poznámka: " + res.DeleteDesc + "<p>");
            }

            if (type == MailDataType.DELETE_ADMIN)
            {
                this.Subject = "Parní Čistič Kärcher - Zrušení rezervace na " + res.Date.ToString("dd.MM.yyyy");

                buffer.Append("<p>Dobrý den,<p>");
                buffer.Append("<p>Správce zrušil Vaši rezervaci na pronájem parního čističe Kärcher na " + res.Date.ToString("dd.MM.yyyy") + "<p>");
                buffer.Append("<p>Poznámka: " + res.DeleteDesc + "<p>");
            }

            buffer.Append("</br>");
            buffer.Append("<p>S pozdravem,<br/>Jaroslav Nový<br />");
            buffer.Append("<a href=\"http://" + NovychResources.AppAddress + "\">" + NovychResources.AppAddress + "</a><br />");
            buffer.Append("<a href=\"mailto:" + NovychResources.AppMail + "\">" + NovychResources.AppMail + "</a><p>");

            Body = buffer.ToString();
        }
    }
}