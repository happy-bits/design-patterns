// No pattern

using System;

namespace DesignPatterns.FactoryMethod.Messages.Before
{
    class Client : IClient
    {
        public string[] Run(string environment)
        {
            var factory = new Factory(environment);
            ISmsService smsService = factory.GetSmsService();
            IMailService mailService = factory.GetMailService();
            
            string event1 = smsService.SendSms("12345", "bla bla");
            string event2 = mailService.SendMail("oo@happybits.se", "bla bla");

            return new string[] { event1, event2 };

        }

    }

    class Factory
    {
        private readonly string _environment;

        public Factory(string environment)
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
