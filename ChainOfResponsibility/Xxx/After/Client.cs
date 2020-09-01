
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Template.Xxx.After
{
    class Client : IClient
    {
        static List<string> _logger = new List<string>();

        public void DoStuff()
        {
            var monkey = new MonkeyHandler();
            var squirrel = new SquirrelHandler();
            var dog = new DogHandler();

            // Here the chain is created
            monkey.SetNext(squirrel).SetNext(dog);

            // Send one food at the time through the chain
            Clienty.ClientCode(monkey, new[] { "Nut", "Banana", "Cup of coffee" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a Nut?",
                    "Squirrel: I'll eat the Nut",

                    "Client: Who wants a Banana?",
                    "Monkey: I'll eat the Banana",

                    "Client: Who wants a Cup of coffee?",
                    "Cup of coffee was left untouched"
                },
                _logger);

            // Send a meatball through the chain
            _logger.Clear();

            Clienty.ClientCode(monkey, new[] { "MeatBall" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a MeatBall?",
                    "Dog: I'll eat the MeatBall",
                },
                _logger);

            // Client can send a request to the middle of the chain
            // Here a banana is sent to the chain, but after the monkey, so no-one will handle it

            _logger.Clear();

            Clienty.ClientCode(squirrel, new[] { "Banana" });

            CollectionAssert.AreEqual(
                new[] {
                    "Client: Who wants a Banana?",
                    "Banana was left untouched",
                },
                _logger);
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

        class Clienty
        {
            // The client code is usually suited to work with a single handler. In
            // most cases, it is not even aware that the handler is part of a chain.
            public static void ClientCode(AbstractHandler handler, IEnumerable<string> foodlist)
            {
                foreach (var food in foodlist)
                {
                    _logger.Add($"Client: Who wants a {food}?");

                    var result = handler.Handle(food);

                    if (result != null)
                    {
                        _logger.Add(result.ToString());
                    }
                    else
                    {
                        _logger.Add($"{food} was left untouched");
                    }
                }
            }
        }

    }
}

