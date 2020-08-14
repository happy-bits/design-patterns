// Factory method

using System;

namespace DesignPatterns.FactoryMethod.Messages.After
{
    class Client : IClient
    {
        public string[] Run(string environment)
        {
            ISmsService smsService = new SmsServiceFactory(environment).GetSmsService();
            IMailService mailService = new MailServiceFactory(environment).GetMailService();

            string event1 = smsService.SendSms("12345", "bla bla");
            string event2 = mailService.SendMail("oo@happybits.se", "bla bla");

            return new string[] { event1, event2 };

        }

    }

    class SmsServiceFactory
    {
        private readonly string _environment;

        public SmsServiceFactory(string environment)
        {
            _environment = environment;
        }
        public ISmsService GetSmsService()
        {
            switch (_environment)
            {
                case "development": return new FakeSmsService();
                case "production": return new SmsService();
                default: throw new Exception();
            }
        }
    }

    class MailServiceFactory 
    {
        private readonly string _environment;

        public MailServiceFactory(string environment)
        {
            _environment = environment;
        }
        public IMailService GetMailService()
        {
            switch (_environment)
            {
                case "development": return new FakeMailService();
                case "production": return new MailService();
                default: throw new Exception();
            }
        }
    }


}
