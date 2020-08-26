
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Memento.Photoshop.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var circle1 = new Circle(0, 0, 2);
            var circle2 = new Circle(5, 5, 10);
            var circle3 = new Circle(200, 300, 100);
            var dot1 = new Dot(77, 88);

            Originator originator = new Originator();
            Caretaker caretaker = new Caretaker(originator);

            originator.AddGraphic(circle1);
            Assert.AreEqual("Circle(0,0,2)", originator.StateInfo);
            caretaker.Backup();         

            originator.AddGraphic(circle2);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10)", originator.StateInfo);
            caretaker.Backup();

            originator.AddGraphic(circle3);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100)", originator.StateInfo);
            caretaker.Backup();

            originator.AddGraphic(dot1);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100) Dot(77,88)", originator.StateInfo);
            caretaker.Backup();

            originator.RemoveAllCircles();
            Assert.AreEqual("Dot(77,88)", originator.StateInfo);

            // Undoing

            caretaker.Undo();
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100) Dot(77,88)", originator.StateInfo);

            caretaker.Undo();
            caretaker.Undo();
            caretaker.Undo();
            Assert.AreEqual("Circle(0,0,2)", originator.StateInfo);
        }

        /*
         Fördel: Originator har (nästan) bara ansvar för att lägga till och ta bort grafik. Sparandet sköts av Caretaker. Vi kan skapa tester för Caretaker separat.

         Nackdel: mer avancerad uppsättning av kod än before (det är en engånggrej iofs)

         Liten nackdel: "CreateMemento" och "Restore" behöver vara i klassen
             
             */

        class Originator
        {
            private List<Graphic> _state = new List<Graphic>();

            public void AddGraphic(Graphic graphic)
            {
                _state.Add(graphic);
            }

            // En del av mönstret: snappshotet skapas av ägaren av state't (originator). Ägaren har full access till statet
            public IMemento CreateMemento()
            {
                return new ConcreteMemento(_state);
            }

            public void Restore(IMemento memento)
            {
                _state = memento.GetState().ToList();
            }

            public void RemoveAllCircles() => _state.RemoveAll(g => g is Circle);

            public string StateInfo => string.Join(" ", _state.Select(s => s.ToString()));

        }

        public interface IMemento
        {
            IEnumerable<Graphic> GetState();
        }

        /*
            I "mementon" sparas en kopia av statet.
            Någon utifrån kan komma åt metadata med inte original-statet
        */

        class ConcreteMemento : IMemento
        {
            private readonly IEnumerable<Graphic> _state; // innehållet är dolt (bra)

            public ConcreteMemento(IEnumerable<Graphic> state)
            {
                var clonedState = state.ToList();
                _state = clonedState;
            }

            public IEnumerable<Graphic> GetState() => _state;
        }

        /*
            Caretakers sparar flera mementos

            Caretaker kan inte av misstag pilla på statet innuti mementot (bra)

        */

        class Caretaker
        {
            private readonly Stack<IMemento> _mementos = new Stack<IMemento>();

            private readonly Originator _originator = null;

            public Caretaker(Originator originator)
            {
                _originator = originator;
            }

            public void Backup()
            {
                _mementos.Push(_originator.CreateMemento());
            }

            public bool IsEmpty() => !_mementos.Any();

            public void Undo()
            {
                if (IsEmpty())
                    return;

                var memento = _mementos.Pop();

                _originator.Restore(memento);
            }
        }

    }
}
