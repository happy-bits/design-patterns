/*
 FACTORY METHOD - https://refactoring.guru/design-patterns/factory-method/csharp/example
 (Written as a test)
 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class GuruConceptual
    {
        static List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            var c = new Client();

            _log.Add("App: Launched with the ConcreteCreator1.");
            c.ClientCode(new ConcreteCreator1());

            _log.Add("App: Launched with the ConcreteCreator2.");
            c.ClientCode(new ConcreteCreator2());


            CollectionAssert.AreEqual(new[] {
                "App: Launched with the ConcreteCreator1.",
                "Client: I'm not aware of the creator's class,but it still works.",
                "Creator: The same creator's code has just worked with {Result of ConcreteProduct1}",
                "App: Launched with the ConcreteCreator2.",
                "Client: I'm not aware of the creator's class,but it still works.",
                "Creator: The same creator's code has just worked with {Result of ConcreteProduct2}",
            }, _log);
        }
        /*
         "Creator class"

            Här finns "factory method" som returnerar en Product

            Underklasserna till Creator implementerar ofta denna metod
             
         */
        abstract class Creator
        {
            // Får även ge default implementation av "factory method" (ej i detta fall) (jag ändrade denna till "protected")
            protected abstract IProduct FactoryMethod();

            /*
             Creator är inte främst ansvarig för att skapa produkter (även om det låter så)
             Den innehåller vanligtvis nån business logik som är beroende på Product-objekten
             */
            public string SomeOperation()
            {
                // Skapa en produkt
                var product = FactoryMethod();
                // Använd produkten
                var result = "Creator: The same creator's code has just worked with "
                    + product.Operation();

                return result;
            }
        }

        // "Concrete Creators" 
        // Kan själv välja exakt vilken typ som ska skapas
        class ConcreteCreator1 : Creator
        {
            // Skapar en konkret produkt. Samtidigt är Creatorn inte beroende av någon konkret produkt (bara IProduct)
            protected override IProduct FactoryMethod()
            {
                return new ConcreteProduct1();
            }
        }

        class ConcreteCreator2 : Creator
        {
            protected override IProduct FactoryMethod()
            {
                return new ConcreteProduct2();
            }
        }

        // De operationer som alla konkreta produkter måste ha
        public interface IProduct
        {
            string Operation();
        }

        // "Concrete Product"
        class ConcreteProduct1 : IProduct
        {
            public string Operation()
            {
                return "{Result of ConcreteProduct1}";
            }
        }

        class ConcreteProduct2 : IProduct
        {
            public string Operation()
            {
                return "{Result of ConcreteProduct2}";
            }
        }

        class Client
        {
            /*
            Klienten jobber med en instans av en "concreate creator"
            */

            public void ClientCode(Creator creator)
            {
                // ...
                _log.Add("Client: I'm not aware of the creator's class,but it still works.");
                _log.Add(creator.SomeOperation());
                // ...
            }
        }
    }
}
