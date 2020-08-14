/*
SMS och Email
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class NotReady_Messages
    {
        static readonly List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            var c = new Client();

            c.ClientCode(new SmsCreator());
            c.ClientCode(new EmailCreator());

            CollectionAssert.AreEqual(new[] {
                "Send Sms with content ContentA to destination DestinationA",
                "Send Sms with content ContentB to destination DestinationB",

                "Send Email with content ContentA to destination DestinationA",
                "Send Email with content ContentB to destination DestinationB",
            }, _log);
        }

        // "Creator"
        abstract class MessageCreator
        {
            readonly List<IMessage> _messages = new List<IMessage>();

            public abstract IMessage CreateMessage(string content, string destination);

            public void AddMessageToQueue(string content, string destination)
            {
                var message = CreateMessage(content, destination);
                _messages.Add(message);
            }

            public void SendMessages()
            {
                foreach (var m in _messages)
                    m.Send();
            }
        }

        // "Concrete Creator" 
        class SmsCreator : MessageCreator
        {
            public override IMessage CreateMessage(string content, string destination)
            {
                return new Sms(content, destination);
            }
        }

        // "Concrete Creator" 
        class EmailCreator : MessageCreator
        {
            public override IMessage CreateMessage(string content, string destination)
            {
                return new Email(content, destination);
            }
        }

        public abstract class IMessage
        {
            public IMessage(string content, string destination)
            {
                Content = content;
                Destination = destination;
            }
            public abstract void Send();
            protected string Content { get; }
            protected string Destination { get; }
        }

        // "Concrete Product"
        class Sms : IMessage
        {
            public Sms(string content, string destination) : base(content, destination)
            {
            }

            public override void Send()
            {
                _log.Add($"Send Sms with content {Content} to destination {Destination}");
            }
        }

        // "Concrete Product"
        class Email : IMessage
        {
            public Email(string content, string destination) : base(content, destination)
            {
            }

            public override void Send()
            {
                _log.Add($"Send Email with content {Content} to destination {Destination}");
            }
        }

        // "Client"
        class Client
        {
            public void ClientCode(MessageCreator creator)
            {
                creator.AddMessageToQueue("ContentA", "DestinationA");
                creator.AddMessageToQueue("ContentB", "DestinationB");
                creator.SendMessages();
            }
        }
    }
}
