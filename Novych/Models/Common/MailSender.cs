using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using Novych.Models.ParniCistic;
using Novych.Resources;

namespace Novych.Models.Common
{
    public class MailSender
    {
        public static SmtpClient smtp;

        public MailSender()
        {
            smtp = new SmtpClient
            {
                Host = NovychResources.AppMailSmtp,
                Port = Int32.Parse(NovychResources.AppMailSmtpPort),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(NovychResources.AppMail, NovychResources.AppMailPassword)
            };
        }

        public bool Send(MailDataModel data)
        {
            var message = new MailMessage(data.From, data.To)
            {
                Subject = data.Subject,
                Body = data.Body,
                IsBodyHtml = true,

            };

            if (data.File != null)
            {
                try
                {
                    message.Attachments.Add(new Attachment(data.File, MediaTypeNames.Application.Pdf));
                }
                catch (Exception ex)
                {
                    Logger.log(LogClass.ERROR, "Exception while adding attachment [" + ex.ToString() + "]");
                }
            }

            if (data.Bcc != null)
            {
                message.Bcc.Add(data.Bcc);
            }

            Logger.log(LogClass.DEBUG, "E-mail prepared to send [" + data.Type + ": " + data.ReservationDate.ToString("dd.MM.yyyy") + " -> " + data.To + "]");

            new Thread(() =>
            {
                try
                {
                    smtp.Send(message);
                    Logger.log(LogClass.INFO, "E-mail sent [" + data.Type + ": " + data.ReservationDate.ToString("dd.MM.yyyy") + " -> " + data.To + "]");

                }
                catch (Exception ex)
                {
                    Logger.log(LogClass.INFO, "E-mail cannot be sent [" + data.Type + ": " + data.ReservationDate.ToString("dd.MM.yyyy") + " -> " + data.To + "] " + ex.ToString());
                }


                message.Dispose();
            }).Start();

            return true;
        }
    }
}