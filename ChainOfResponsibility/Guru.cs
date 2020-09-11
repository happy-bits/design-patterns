/*
 * From: https://refactoring.guru/design-patterns/chain-of-responsibility/csharp/example
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void monkey_squirrel_dog()
        {
            var monkey = new MonkeyHandler();
            var squirrel = new SquirrelHandler();
            var dog = new DogHandler();

            // Here the chain is created
            monkey.SetNext(squirrel).SetNext(dog);

            // Send one food at the time through the chain
            var result = Client.ClientCode(monkey, new[] { "Nut", "Banana", "Cup of coffee" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a Nut?",
                    "Squirrel: I'll eat the Nut",

                    "Client: Who wants a Banana?",
                    "Monkey: I'll eat the Banana",

                    "Client: Who wants a Cup of coffee?",
                    "Cup of coffee was left untouched"
                },
                result);

            // Send a meatball through the chain

            var result2 = Client.ClientCode(monkey, new[] { "MeatBall" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a MeatBall?",
                    "Dog: I'll eat the MeatBall",
                },
                result2);

            // Client can send a request to the middle of the chain
            // Here a banana is sent to the chain, but after the monkey, so no-one will handle it


            var result3 = Client.ClientCode(squirrel, new[] { "Banana" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a Banana?",
                    "Banana was left untouched",
                },
                result3);

        }


        // The Handler interface declares a method for building the chain of
        // handlers. It also declares a method for executing a request.
        interface IHandler
        {
            IHandler SetNext(IHandler handler);

            string Handle(string request);
        }

        // The default chaining behavior can be implemented inside a base handler
        // class.
        abstract class AbstractHandler : IHandler
        {
            private IHandler _nextHandler;

            public IHandler SetNext(IHandler handler)
            {
                _nextHandler = handler;

                // Returning a handler from here will let us link handlers in a
                // convenient way like this:
                // monkey.SetNext(squirrel).SetNext(dog);
                return handler;
            }

            public virtual string Handle(string request)
            {
                if (_nextHandler != null)
                {
                    return _nextHandler.Handle(request);
                }
                else
                {
                    // No one could handle the request
                    return null;
                }
            }
        }

        class MonkeyHandler : AbstractHandler
        {
            public override string Handle(string request)
            {
                if (request == "Banana")
                {
                    // This will end the chain
                    return $"Monkey: I'll eat the {request.ToString()}";
                }
                else
                {
                    // Now the default behaviour will be executed, 
                    // which in this case is: go to next handler in the chain
                    return base.Handle(request);
                }
            }
        }

        class SquirrelHandler : AbstractHandler
        {
            public override string Handle(string request)
            {
                if (request == "Nut")
                {
                    return $"Squirrel: I'll eat the {request.ToString()}";
                }
                else
                {
                    return base.Handle(request);
                }
            }
        }

        class DogHandler : AbstractHandler
        {
            public override string Handle(string request)
            {
                if (request == "MeatBall")
                {
                    return $"Dog: I'll eat the {request.ToString()}";
                }
                else
                {
                    return base.Handle(request);
                }
            }
        }

        class Client
        {
            // The client code is usually suited to work with a single handler. In
            // most cases, it is not even aware that the handler is part of a chain.
            public static List<string> ClientCode(AbstractHandler handler, IEnumerable<string> foodlist)
            {
                var events = new List<string>();

                foreach (var food in foodlist)
                {
                    events.Add($"Client: Who wants a {food}?");

                    var result = handler.Handle(food);

                    if (result != null)
                    {
                        events.Add(result.ToString());
                    }
                    else
                    {
                        events.Add($"{food} was left untouched");
                    }
                }
                return events;
            }
        }

    }

}
