using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.FactoryMethod.Messages
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(),
            new After.Client() 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                string[] events = client.Run("development");
                CollectionAssert.AreEqual(new[] {
                    "No sms sent",
                    "No mail sent"
                }, events);
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {
                string[] events = client.Run("production");
                CollectionAssert.AreEqual(new[] {
                    "Sms with message 'bla bla' is sent to 12345",
                    "Mail with message 'bla bla' is sent to oo@happybits.se"
                }, events);
            }
        }

    }

    interface ISmsService
    {
        string SendSms(string to, string message);
    }

    class SmsService : ISmsService
    {
        public string SendSms(string to, string message)
        {
            return $"Sms with message '{message}' is sent to {to}";
        }
    }
    class FakeSmsService : ISmsService
    {
        public string SendSms(string to, string message)
        {
            return "No sms sent";
        }
    }

    interface IMailService
    {
        string SendMail(string emailaddress, string message);
    }

    class MailService : IMailService
    {
        public string SendMail(string emailaddress, string message)
        {
            return $"Mail with message '{message}' is sent to {emailaddress}";
        }
    }

    class FakeMailService : IMailService
    {
        public string SendMail(string emailaddress, string message)
        {
            return "No mail sent";
        }
    }

}
