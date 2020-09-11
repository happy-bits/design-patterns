/*

"En exportfunktion av Shapes..."

Låter dej separera algoritmer från objekten som den jobbar med

Inte så vanligt mönster pga sin komplexitet och snäva tillämpning

Du kan dra största nytta av mönstret när du använder det med en komplex objekt struktur som Composite tree

Mönstret låter dina klasser vara mer fokucerad på deras huvudsakliga jobb. Annat beteende skickas till "visitor"-klasser.

*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Visitor
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            List<IComponent> components = new List<IComponent>
            {
                new ConcreteComponentA(),
                new ConcreteComponentB()
            };

            Console.WriteLine("The client code works with all visitors via the base Visitor interface:");
            var visitor1 = new ConcreteVisitor1();
            Client.ClientCode(components, visitor1);

            Console.WriteLine();

            Console.WriteLine("It allows the same client code to work with different types of visitors:");
            var visitor2 = new ConcreteVisitor2();
            Client.ClientCode(components, visitor2);
        }

        interface IComponent
        {
            void Accept(IVisitor visitor);
        }

        class ConcreteComponentA : IComponent
        {

            public void Accept(IVisitor visitor)
            {
                // Metoden "VisitConcreteComponentA" ska motsvara komponenten "ConcreteComponentA"
                visitor.VisitConcreteComponentA(this);
            }

            // Visitor känner till den konkreta komponenten så kan anropa denna metod
            public string ExclusiveMethodOfConcreteComponentA()
            {
                return "A";
            }
        }

        class ConcreteComponentB : IComponent
        {
            public void Accept(IVisitor visitor)
            {
                visitor.VisitConcreteComponentB(this);
            }

            public string SpecialMethodOfConcreteComponentB()
            {
                return "B";
            }
        }

        interface IVisitor
        {
            // Motsvarar en komponentklass
            void VisitConcreteComponentA(ConcreteComponentA element);

            // Motsvarar en komponentklass
            void VisitConcreteComponentB(ConcreteComponentB element);
        }

        /*
         "Konkret visitor"
         Implementerar flera versioner av samma algoritm
       */

        class ConcreteVisitor1 : IVisitor
        {
            public void VisitConcreteComponentA(ConcreteComponentA element)
            {
                Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor1");
            }

            public void VisitConcreteComponentB(ConcreteComponentB element)
            {
                Console.WriteLine(element.SpecialMethodOfConcreteComponentB() + " + ConcreteVisitor1");
            }
        }

        class ConcreteVisitor2 : IVisitor
        {
            public void VisitConcreteComponentA(ConcreteComponentA element)
            {
                Console.WriteLine(element.ExclusiveMethodOfConcreteComponentA() + " + ConcreteVisitor2");
            }

            public void VisitConcreteComponentB(ConcreteComponentB element)
            {
                Console.WriteLine(element.SpecialMethodOfConcreteComponentB() + " + ConcreteVisitor2");
            }
        }

        class Client
        {
            // Samma klientkod kan jobba med flera olika sorters "visitors"
            public static void ClientCode(List<IComponent> components, IVisitor visitor)
            {
                foreach (var component in components)
                {
                    component.Accept(visitor);
                }
            }
        }

     
    }
}
