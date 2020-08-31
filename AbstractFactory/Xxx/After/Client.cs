
using System;
using System.Collections.Generic;

namespace DesignPatterns.AbstractFactory.Xxx.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            // The client code can work with any concrete factory class.
            Console.WriteLine("Client: Testing client code with the first factory type...");
            ClientMethod(new ConcreteFactory1());
            Console.WriteLine();

            Console.WriteLine("Client: Testing the same client code with the second factory type...");
            ClientMethod(new ConcreteFactory2());
        }

        public void ClientMethod(IAbstractFactory factory)
        {
            var productA = factory.CreateProductA();
            var productB = factory.CreateProductB();

            Console.WriteLine(productB.UsefulFunctionB());
            Console.WriteLine(productB.AnotherUsefulFunctionB(productA));
        }



        public interface IAbstractFactory
        {
            IAbstractProductA CreateProductA();
            IAbstractProductB CreateProductB();
        }

        /*

           Konkreta factories producerar en familj av produkter. En familj kan ha flera varianter, 
           men produkterna av en familj är inte kompatibla med produkter i andra familjer

           IAbstractProductA kan vara en stol och ConcreteProductA1 en viktoriansk stol

           "Viktoriansk fabrik"
        */
        class ConcreteFactory1 : IAbstractFactory
        {
            // "Viktoriansk stol"
            public IAbstractProductA CreateProductA()
            {
                return new ConcreteProductA1();
            }

            // "Viktorianskt bord"
            public IAbstractProductB CreateProductB()
            {
                return new ConcreteProductB1();
            }
        }

        /*
           En till fabrik (som t.ex tillverkar moderna möbler eller art-deco-möbler
           "Art-deco fabrik"
        */
        class ConcreteFactory2 : IAbstractFactory
        {
            // "Art-deco stol"
            public IAbstractProductA CreateProductA()
            {
                return new ConcreteProductA2();
            }

            // "Art-deco bord"
            public IAbstractProductB CreateProductB()
            {
                return new ConcreteProductB2();
            }
        }

        // En förmåga som alla stolar har
        public interface IAbstractProductA
        {
            string UsefulFunctionA();
        }

        class ConcreteProductA1 : IAbstractProductA
        {
            public string UsefulFunctionA()
            {
                return "The result of the product A1.";
            }
        }

        class ConcreteProductA2 : IAbstractProductA
        {
            public string UsefulFunctionA()
            {
                return "The result of the product A2.";
            }
        }

        // En förmåga som alla bord har (de kan också samarbeta med stolarna)
        public interface IAbstractProductB
        {
            // Product B kan gör sin egen grej
            string UsefulFunctionB();

            // ...men den kan också samarbeta med ProductA.

            string AnotherUsefulFunctionB(IAbstractProductA collaborator);
        }

        class ConcreteProductB1 : IAbstractProductB
        {
            public string UsefulFunctionB()
            {
                return "The result of the product B1.";
            }

            public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
            {
                var result = collaborator.UsefulFunctionA();

                return $"The result of the B1 collaborating with the ({result})";
            }
        }

        class ConcreteProductB2 : IAbstractProductB
        {
            public string UsefulFunctionB()
            {
                return "The result of the product B2.";
            }

            public string AnotherUsefulFunctionB(IAbstractProductA collaborator)
            {
                var result = collaborator.UsefulFunctionA();

                return $"The result of the B2 collaborating with the ({result})";
            }
        }
    }
}
