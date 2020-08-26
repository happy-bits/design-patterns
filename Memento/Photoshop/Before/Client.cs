
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Memento.Photoshop.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            throw new NotImplementedException();
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

        class Originator
        {
            private List<Graphic> _state;

            public Originator(params Graphic[] state)
            {
                _state = state.ToList();
            }

            public void AddGraphic(Graphic graphic)
            {
                _state.Add(graphic);
            }

            public IMemento CreateMemento()
            {
                return new ConcreteMemento(_state);
            }

            public void Restore(IMemento memento)
            {
                if (!(memento is ConcreteMemento))
                {
                    throw new Exception("Unknown memento class " + memento.ToString());
                }

                _state = memento.GetState().ToList();
            }

            public void RemoveAllCircles() => _state.RemoveAll(g => g is Circle);

            public string StateInfo => string.Join(" ", _state.Select(s => s.ToString()));

        }

        public interface IMemento
        {
            List<Graphic> GetState();
        }

        class ConcreteMemento : IMemento
        {
            private readonly List<Graphic> _state;

            public ConcreteMemento(IEnumerable<Graphic> state)
            {
                _state = state.ToList(); // måste ha "ToList" här
            }

            public List<Graphic> GetState() => _state;
        }

        class Caretaker
        {
            private readonly List<IMemento> _mementos = new List<IMemento>();

            private readonly Originator _originator = null;

            public Caretaker(Originator originator)
            {
                _originator = originator;
            }

            public void Backup()
            {
                _mementos.Add(_originator.CreateMemento());
            }

            public void Undo()
            {
                if (_mementos.Count == 0)
                {
                    return;
                }

                var memento = _mementos.Last();
                _mementos.Remove(memento);

                // Funkar denna try-catch?

                try
                {
                    _originator.Restore(memento);
                }
                catch (Exception)
                {
                    Undo();
                }
            }
        }

    }
}
