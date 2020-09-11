/*

"Låt all kommunikation gå via Kalle"

 https://refactoring.guru/design-patterns/mediator/csharp/example


 Ett "behavioral" pattern som låter dig minska kaotiska beroenden mellan objekt. Direktkommunikation mellan objekt sker inte utan de går via ett "mediator" objekt

 Ingen direkt kommunikation mellan komponenter (för du vill att de ska vara oberoende av varandra, så du kan återanvända dem). 

 Komponenterna samarbeter indirekt genom att anropa ett mediator-objekt. Så komponenterna är bara beroende på detta objekt.

 Ju färre beroende en klass har, desto lättare är det att utvidga, utöka och återanvända

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Mediator
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex1()
        {
            // The client code.
            Component1 component1 = new Component1();
            Component2 component2 = new Component2();
            new ConcreteMediator(component1, component2);

            Console.WriteLine("Client triggers operation A.");
            component1.DoA(); // Triggar även C

            Console.WriteLine();

            Console.WriteLine("Client triggers operation D.");
            component2.DoD(); // Triggar även B och C
        }

        /*
        Client triggers operation A.
        Component 1 does A.
        Mediator reacts on A and triggers following operations:
        Component 2 does C.

        Client triggers operation D.
        Component 2 does D.
        Mediator reacts on D and triggers following operations:
        Component 1 does B.
        Component 2 does C. 
        */

        /*

            Detta interface har normalt bara en rad. Kan skicka med "context" 

         */

        public interface IMediator
        {
            void Notify(object sender, string ev);
        }
        /*
         "Concrete Mediator"

         Koordinerar ett par komponenter
        */
        class ConcreteMediator : IMediator
        {
            private Component1 _component1;

            private Component2 _component2;

            public ConcreteMediator(Component1 component1, Component2 component2)
            {
                _component1 = component1;
                _component1.SetMediator(this);
                _component2 = component2;
                _component2.SetMediator(this);
            }

            public void Notify(object sender, string ev)
            {
                if (ev == "A")
                {
                    Console.WriteLine("Mediator reacts on A and triggers following operations:");
                    _component2.DoC();
                }
                if (ev == "D")
                {
                    Console.WriteLine("Mediator reacts on D and triggers following operations:");
                    _component1.DoB();
                    _component2.DoC();
                }
            }
        }

        /*
         "Base component" sparar instansen av en mediator
        */
        class BaseComponent
        {
            protected IMediator _mediator;

            public BaseComponent(IMediator mediator = null)
            {
                _mediator = mediator;
            }

            public void SetMediator(IMediator mediator)
            {
                _mediator = mediator;
            }
        }

        /*
        "Konkreta komponenter"

        Alla komponenter avslutar med att anropa "mediatorn" (medlare) och skicka med sig själv och info vad som hänt 

        Komponenten behöver inte vara beroende av en konkret mediator.

        Komponenten är inte beroende av andra komponenter
            */
        class Component1 : BaseComponent
        {
            public void DoA()
            {
                Console.WriteLine("Component 1 does A.");

                _mediator.Notify(this, "A");
            }

            public void DoB()
            {
                Console.WriteLine("Component 1 does B.");

                _mediator.Notify(this, "B");
            }
        }

        class Component2 : BaseComponent
        {
            public void DoC()
            {
                Console.WriteLine("Component 2 does C.");

                _mediator.Notify(this, "C");
            }

            public void DoD()
            {
                Console.WriteLine("Component 2 does D.");

                _mediator.Notify(this, "D");
            }
        }

    }
}
