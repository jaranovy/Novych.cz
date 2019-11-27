using Microsoft.VisualStudio.TestTools.UnitTesting;
using Novych.Models.Common;
using Novych.Models.ParniCistic;
using System;

namespace Novych.Tests.Models.Common
{
    [TestClass]
    public class MailSenderTest
    {

        [TestMethod]
        public void SendMail()
        {
            bool result = false;
            var ms = new MailSender();

            var res = new ReservationModel();
            res.Address = "UnitTestAddress";
            res.Name = "UnitTestName";
            res.Date = DateTime.Now;
            res.Email = "unit-test@test.test";
            res.Phone = "123456789";

            var msg = new MailDataModel(MailDataModel.MailDataType.CREATE, res);

            result = ms.Send(msg);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
