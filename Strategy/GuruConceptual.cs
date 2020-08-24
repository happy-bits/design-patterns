using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Strategy
{
    [TestClass]
    public class GuruConceptual
    {
        [TestMethod]
        public void client_code()
        {
            /*
               Välj en konkret strategi och skicka in den i "Context". 
               Clienten vet skillnad på strategierna
            */
            var context = new Context();

            Console.WriteLine("Client: Strategy is set to normal sorting.");
            context.SetStrategy(new ConcreteStrategyA());
            context.DoSomeBusinessLogic();

            Console.WriteLine();

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            context.SetStrategy(new ConcreteStrategyB());
            context.DoSomeBusinessLogic();
        }

        class Context
        {
            /*
                "Context" håller en referens till ett strategiobjekt

                "Context" känner inte till de konkreta strategierna (bra) utan jobbar med strategins interface
            */
            private IStrategy _strategy;

            public Context()
            {
            }

            // Vanligt att strategin kan sättas i konstrutorn men att det också finns en setter
            public Context(IStrategy strategy)
            {
                _strategy = strategy;
            }

            // Vi kan ändra strategin i runtime
            public void SetStrategy(IStrategy strategy)
            {
                _strategy = strategy;
            }

            // Istället för att context själv implementerar olika varianter av strategier så delegeras det jobbet
            public void DoSomeBusinessLogic()
            {
                Console.WriteLine("Context: Sorting data using the strategy (not sure how it'll do it)");
                var result = _strategy.DoAlgorithm(new List<string> { "a", "b", "c", "d", "e" });

                string resultStr = string.Empty;
                foreach (var element in result)
                {
                    resultStr += element + ",";
                }

                Console.WriteLine(resultStr);
            }
        }

        public interface IStrategy
        {
            List<string> DoAlgorithm(object data);
        }

        class ConcreteStrategyA : IStrategy
        {
            public List<string> DoAlgorithm(object data)
            {
                var list = data as List<string>;
                list.Sort();

                return list;
            }
        }

        class ConcreteStrategyB : IStrategy
        {
            public List<string> DoAlgorithm(object data)
            {
                var list = data as List<string>;
                list.Sort();
                list.Reverse();

                return list;
            }
        }
    }
}
