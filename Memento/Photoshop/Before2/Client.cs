﻿
// Variant med en klass till: "TimeMachine"
// Varför är denna sämre än After?

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Memento.Photoshop.Before2
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

        class Originator
        {
            private List<Graphic> _state = new List<Graphic>();

            // Nackdel: detta fält behövs
            private readonly TimeMachine _timeMachine = new TimeMachine();

            public void AddGraphic(Graphic graphic)
            {
                _state.Add(graphic);
            }
            
            public void RemoveAllCircles() => _state.RemoveAll(g => g is Circle);

            public void Backup()
            {
                _timeMachine.Backup(_state);
            }

            public void Undo()
            {
                _state = _timeMachine.Undo(_state);
            }

            public string StateInfo => string.Join(" ", _state.Select(s => s.ToString()));

        }

        class TimeMachine
        {
            private readonly Stack<List<Graphic>> _backup = new Stack<List<Graphic>>();

            public void Backup(IEnumerable<Graphic> state)
            {
                var clonedState = state.ToList();
                _backup.Push(clonedState);
            }

            public bool IsEmpty() => !_backup.Any();

            public List<Graphic> Undo(List<Graphic> _state)
            {
                if (IsEmpty())
                    return _state;

                return _backup.Pop();
            }
        }
    }
}
