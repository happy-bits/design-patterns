
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Memento.Photoshop.Before
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

            originator.AddGraphic(circle1);
            Assert.AreEqual("Circle(0,0,2)", originator.StateInfo);
            originator.Backup();

            originator.AddGraphic(circle2);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10)", originator.StateInfo);
            originator.Backup();

            originator.AddGraphic(circle3);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100)", originator.StateInfo);
            originator.Backup();

            originator.AddGraphic(dot1);
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100) Dot(77,88)", originator.StateInfo);
            originator.Backup();

            originator.RemoveAllCircles();
            Assert.AreEqual("Dot(77,88)", originator.StateInfo);

            // Undoing

            originator.Undo();
            Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100) Dot(77,88)", originator.StateInfo);

            originator.Undo();
            originator.Undo();
            originator.Undo();
            Assert.AreEqual("Circle(0,0,2)", originator.StateInfo);
        }

        /*
         Fördel: kortare och enklare kod än memento-lösning
         Nackdel: kod för att hantera backup och undo är här (så denna klass är ansvarig för två saker istället för en)
        */
        class Originator
        {
            private List<Graphic> _state;

            // Nackdel: kod för sparande av gamla state är här "_backup"
            private readonly Stack<List<Graphic>> _backup = new Stack<List<Graphic>>();

            public Originator(params Graphic[] state)
            {
                _state = state.ToList();
            }

            public void AddGraphic(Graphic graphic)
            {
                _state.Add(graphic);
            }
            
            public void RemoveAllCircles() => _state.RemoveAll(g => g is Circle);

            // Nackdel: kod för backup är här
            public void Backup()
            {
                var clonedState = _state.ToList();
                _backup.Push(clonedState);
            }

            // Nackdel: kod för undo är här
            public void Undo()
            {
                if (_backup.Count == 0)
                    return;

                _state = _backup.Pop();
            }

            public string StateInfo => string.Join(" ", _state.Select(s => s.ToString()));

        }
    }
}
