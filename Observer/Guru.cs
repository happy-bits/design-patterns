// "När någon händer notifiera en central EventManager som andra kan prenumera på"

// https://refactoring.guru/design-patterns/observer/csharp/example
/*

Du kan definiera en prenumerationsmekanism för att låta flera objekt få reda på att något har hänt (att state't ändrats)

Det går att prenumerera och avsluta prenumeration

Ganska vanligt pattern i C#, speciellt för gui-komponenter. Det gör att ett objekt kan reagera på ett annat objekt utan att klasserna är hårt kopplade
 
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatterns.Observer
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA);                 // Lägg till en prenumerant på "subject"

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB);   // ObserverB tar bort som prenumerant

            subject.SomeBusinessLogic();
        }

    }

    /*
    Subject: Attached an observer.
    Subject: Attached an observer.

    Subject: I'm doing something important.
    Subject: My state has just changed to: 8
    Subject: Notifying observers...
    ConcreteObserverB: Reacted to the event.

    Subject: I'm doing something important.
    Subject: My state has just changed to: 6
    Subject: Notifying observers...
    ConcreteObserverB: Reacted to the event.
    Subject: Detached an observer.

    Subject: I'm doing something important.
    Subject: My state has just changed to: 2
    Subject: Notifying observers...
    ConcreteObserverA: Reacted to the event. 
    */

    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    // Subjekt äger nåt viktigt state och meddelare observers när state't ändras

    public class Subject : ISubject
    {
        public int State { get; set; } = 0;

        // Fördel: subject bryr sig inte om den exakta klassen, utan det räcker att klassen implementerar IObserver
        private readonly List<IObserver> _observers = new List<IObserver>();

        // Prenumera
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Subject: Attached an observer.");
            _observers.Add(observer);
        }

        // Avprenumera
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine("Subject: Detached an observer.");
        }

        // Notifiera alla observers att något har hänt
        public void Notify()
        {
            Console.WriteLine("Subject: Notifying observers...");

            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public void SomeBusinessLogic()
        {
            Console.WriteLine("\nSubject: I'm doing something important.");
            State = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Subject: My state has just changed to: " + State);
            Notify();
        }
    }

    // Konkret observer reagerar på uppdateringen från subject
    class ConcreteObserverA : IObserver
    {
        // Denna anropas av "subject" och behöver alltså vara publik
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State < 3)
            {
                Console.WriteLine("ConcreteObserverA: Reacted to the event.");
            }
        }
    }

    // Det kan finnas helt olika "observers" som lyssnar på samma "subject"
    class ConcreteObserverB : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Console.WriteLine("ConcreteObserverB: Reacted to the event.");
            }
        }
    }
}
