// "Bryt ut spara och undo av händelser"

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DesignPatterns.Memento
{
    [TestClass]
    public class Guru
    {

        [TestMethod]
        public void Ex1()
        {
            Originator originator = new Originator("Super-duper-super-puper-super."); // Originator: My initial state is: Super-duper-super-puper-super.
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();         // Caretaker: Saving Originator's state...
            originator.DoSomething();   // Originator: I'm doing something important.
                                        // Originator: and my state has changed to: bGADLfCcXnurOPNEtRbahJvpPWWZrC

            caretaker.Backup();
            originator.DoSomething();

            caretaker.Backup();
            originator.DoSomething();

            Console.WriteLine();
            caretaker.ShowHistory();  // Caretaker: Here's the list of mementos:
                                      // 8/25/2020 1:38:49 PM / (Super-dup)...
                                      // 8/25/2020 1:38:49 PM / (bGADLfCcX)...
                                      // 8/25/2020 1:38:50 PM / (CdTxgFhLe)...

            Console.WriteLine("\nClient: Now, let's rollback!\n");
            caretaker.Undo(); // Caretaker: Restoring state to: 8 / 25 / 2020 1:38:50 PM / (CdTxgFhLe)...
                              // Originator: My state has changed to: CdTxgFhLesthaUGiAuPRzcmnOdWWzA

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine();
        }
        /*
        Originator: My initial state is: Super-duper-super-puper-super.

        Caretaker: Saving Originator's state...
        Originator: I'm doing something important.
        Originator: and my state has changed to: bGADLfCcXnurOPNEtRbahJvpPWWZrC

        Caretaker: Saving Originator's state...
        Originator: I'm doing something important.
        Originator: and my state has changed to: CdTxgFhLesthaUGiAuPRzcmnOdWWzA

        Caretaker: Saving Originator's state...
        Originator: I'm doing something important.
        Originator: and my state has changed to: VpxgwdjcuUitzutLvmushnWlOlCsuO

        Caretaker: Here's the list of mementos:
        8/25/2020 1:38:49 PM / (Super-dup)...
        8/25/2020 1:38:49 PM / (bGADLfCcX)...
        8/25/2020 1:38:50 PM / (CdTxgFhLe)...

        Client: Now, let's rollback!

        Caretaker: Restoring state to: 8/25/2020 1:38:50 PM / (CdTxgFhLe)...
        Originator: My state has changed to: CdTxgFhLesthaUGiAuPRzcmnOdWWzA

        Client: Once more!

        Caretaker: Restoring state to: 8/25/2020 1:38:49 PM / (bGADLfCcX)...
        Originator: My state has changed to: bGADLfCcXnurOPNEtRbahJvpPWWZrC 
        */

        /*
            "Originator" (upphovsmannen) håller ett viktigt state (dolt) som kan ändras över tid.


        */

        class Originator
        {
            private string _state; // för enkelhets skull så är statet bara en sträng

            public Originator(string state)
            {
                _state = state;
                Console.WriteLine("Originator: My initial state is: " + state);
            }

            /* Detta är affärslogik

               Här kan state't ändras

               Klienten behöver göra en backup av statet innan detta utförs

             */
            public void DoSomething()
            {
                Console.WriteLine("Originator: I'm doing something important.");
                _state = GenerateRandomString(30);
                Console.WriteLine($"Originator: and my state has changed to: {_state}");
            }

            private string GenerateRandomString(int length = 10)
            {
                string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string result = string.Empty;

                while (length > 0)
                {
                    result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

                    Thread.Sleep(12);

                    length--;
                }

                return result;
            }

            /*
             Sparar state't i ett "mememento" (minne)

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

                _state = memento.GetState();
                Console.Write($"Originator: My state has changed to: {_state}");
            }
        }

        // Fördel: vi gömmer Originatorns state

        public interface IMemento
        {
            string GetName();

            string GetState();

            DateTime GetDate();
        }

        // Sparar statet ett viss ögonblick
        class ConcreteMemento : IMemento
        {
            private readonly string _state;

            private readonly DateTime _date;

            public ConcreteMemento(string state)
            {
                _state = state;
                _date = DateTime.Now;
            }

            // När "Originator" vill återställa ett state
            public string GetState()
            {
                return _state;
            }

            // Skriver ut lite info om state't (används av Caretaker)
            public string GetName()
            {
                return $"{_date} / ({_state.Substring(0, 9)})...";
            }

            public DateTime GetDate()
            {
                return _date;
            }
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
                Console.WriteLine("\nCaretaker: Saving Originator's state...");
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

                Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

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

            public void ShowHistory()
            {
                Console.WriteLine("Caretaker: Here's the list of mementos:");

                foreach (var memento in _mementos)
                {
                    Console.WriteLine(memento.GetName());
                }
            }
        }

    }
}
