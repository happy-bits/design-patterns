﻿// EJ KLAR

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


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

            caretaker.Undo();
            Assert.AreEqual("Circle(0,0,2)", originator.StateInfo);

            //caretaker.Backup();

            //originator.AddGraphic(circle3);
            //Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100)", originator.StateInfo);
            //caretaker.Backup();

            //originator.AddGraphic(dot1);
            //Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100) Dot(77,88)", originator.StateInfo);

            //// Undoing

            //caretaker.Undo();
            //Assert.AreEqual("Circle(0,0,2) Circle(5,5,10) Circle(200,300,100)", originator.StateInfo);

            //caretaker.Undo();
            //Assert.AreEqual("Circle(0,0,2) Circle(5,5,10)", originator.StateInfo);
        }



        /*
            "Originator" (upphovsmannen) håller ett viktigt state (dolt) som kan ändras över tid.


        */

        class Originator
        {
            private List<Graphic> _state;

            public Originator(params Graphic[] state)
            {
                _state = state.ToList();
            }

            /* Detta är affärslogik

               Här kan state't ändras

               Klienten behöver göra en backup av statet innan detta utförs

             */
            public void AddGraphic(Graphic graphic)
            {
                _state.Add(graphic);
            }

            /*
             Sparar state't i ett "memento" (minne)

             Egentligen sparas inget här utan vi returnerar vi bara ett konkret memento. Denna metod används av "Caretaker" som sparar minnet hos sig
            */
            public IMemento Save()
            {
                return new ConcreteMemento(_state);
            }

            // Återställ ett minne

            public void Restore(IMemento memento)
            {
                if (!(memento is ConcreteMemento))
                {
                    throw new Exception("Unknown memento class " + memento.ToString());
                }

                _state = memento.GetState().ToList();
            }

            public string StateInfo => string.Join(" ", _state.Select(s => s.ToString()));

        }

        // Fördel: vi gömmer Originatorns state

        public interface IMemento
        {
            List<Graphic> GetState();
        }

        // Sparar statet ett viss ögonblick
        class ConcreteMemento : IMemento
        {
            private readonly List<Graphic> _state;

            private readonly DateTime _date;

            public ConcreteMemento(IEnumerable<Graphic> state)
            {
                _state = state.ToList(); // måste ha "ToList" här
                _date = DateTime.Now;
            }

            // När "Originator" vill återställa ett state
            public List<Graphic> GetState()
            {
                return _state;
            }

           // public string GetStateInfo() => string.Join(" ", _state.Select(s => s.ToString()));
        }

        /*
         Caretaker (portvakt) har inte tillgång till Originatorn's state (pga att den är gömd i den konkreta Mementon)
         ...men har en referens till upphovsmannen så den kan ta hand om "backup" och "undo"
        */

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
                _mementos.Add(_originator.Save());
            }

            public void Undo()
            {
                if (_mementos.Count == 0)
                {
                    return;
                }

                var memento = _mementos.Last();
                _mementos.Remove(memento);


                try
                {
                    // Återställ statet

                    _originator.Restore(memento);
                }
                catch (Exception)
                {
                    Undo();
                }
            }



            //public void ShowHistory()
            //{
            //    Console.WriteLine("Caretaker: Here's the list of mementos:");

            //    foreach (var memento in _mementos)
            //    {
            //        Console.WriteLine(memento.GetName());
            //    }
            //}
        }

    }
}
