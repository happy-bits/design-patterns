/*

 Bra och verkligt exempel

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.ChainOfResponsibility
{
    // Extensionmetoder till specification gör att vi kan gruppera ihop flera specifikationer!
    public static class SpecificationExtensions
    {
        // Kolla att både a och b uppfyller specifikationen
        public static Specification<T> And<T>(this ISpecification<T> a, ISpecification<T> b)
        {
            if (a != null && b != null)
            {
                return new Specification<T>(expression => a.IsSatisfiedBy(expression) && b.IsSatisfiedBy(expression));
            }
            return null;
        }
        public static Specification<T> Or<T>(this ISpecification<T> a, ISpecification<T> b)
        {
            if (a != null && b != null)
            {
                return new Specification<T>(expression => a.IsSatisfiedBy(expression) || b.IsSatisfiedBy(expression));
            }
            return null;
        }
        public static Specification<T> Not<T>(this ISpecification<T> a)
        {
            return a != null ? new Specification<T>(expression => !a.IsSatisfiedBy(expression)) : null;
        }
    }

    public static class Logging
    {
        public static List<string> Message { get; } = new List<string>();

        public static void Log(string value)
        {
            Message.Add(value);
        }
    }

    [TestClass]
    public class BookPublishing
    {
        [TestMethod]
        public void Ex1()
        {
            // IsSatisfiedBy returnerar true om inkomna strängen är "hej"
            var hejSpec = new Specification<string>(y => y == "hej");

            // IsSatisfiedBy returnerar true om inkomna siffran är 42
            var fortytwoSpec = new Specification<int>(y => y == 42);

            Assert.IsTrue(hejSpec.IsSatisfiedBy("hej"));
            Assert.IsFalse(hejSpec.IsSatisfiedBy("oooo"));
        }

        [TestMethod]
        public void Ex2()
        {
            // Skapa ett par böcker (med titel, författare, boktyp och publiceringskostnad)

            var books = new List<Book> {
                new Book("The Stand", "Stephen King", CoverType.Paperback, 35000),
                new Book("The Hobbit", "J.R.R. Tolkien", CoverType.Paperback, 25000),
                new Book("The Name of the Wind", "Patrick Rothfuss", CoverType.Digital, 7500),
                new Book("To Kill a Mockingbird", "Harper Lee", CoverType.Hard, 65000),
                new Book("1984", "George Orwell", CoverType.Paperback, 22500) ,
                new Book("Jane Eyre", "Charlotte Brontë", CoverType.Hard, 82750)
            };

            // Skapa specifikationer för olika sorters böcker (de kräver att boken är Digital, Hard, Paperback)

            var digitalCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Digital);
            var hardCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Hard);
            var paperbackCoverSpec = new Specification<Book>(book => book.CoverType == CoverType.Paperback);

            // Skapa specifikationer för intervall av publiceringskostnader

            var extremeBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 75000);
            var highBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 50000 && book.PublicationCost < 75000);
            var mediumBudgetSpec = new Specification<Book>(book => book.PublicationCost >= 25000 && book.PublicationCost < 50000);
            var lowBudgetSpec = new Specification<Book>(book => book.PublicationCost < 25000);
            var defaultSpec = new Specification<Book>(book => true);

            // Skapa de anställda och om de kan publicera böcker eller ej
            var publicationProcess = new PublicationProcess();

            // Emplyee<Book> ==> en anställd som kan publicera Book's
            // Tredje parametern hänvisar till en enkel metod som antingen ger error eller försöker publicera boken
            // Tredje parametern är en Action<T>, så det måste i detta fall vara en funktion som tar en bok som indata

            var ceo = new Employee<Book>("Alice", Position.CEO, publicationProcess.PublishBook);
            var president = new Employee<Book>("Bob", Position.President, publicationProcess.PublishBook);
            var cfo = new Employee<Book>("Christine", Position.CFO, publicationProcess.PublishBook);
            var director = new Employee<Book>("Dave", Position.DirectorOfPublishing, publicationProcess.PublishBook);
            var defaultEmployee = new Employee<Book>("INVALID", Position.Default, publicationProcess.FailedPublication);

            // Ange vad de olika anställda har för rättighet att göra (ex director kan bara hantera digitala och billiga böcker)
            // Tack vare "specification pattern" kan vi slå ihop flera specifikationer

            director.SetSpecification(digitalCoverSpec.And(lowBudgetSpec));
            cfo.SetSpecification(digitalCoverSpec.Or(paperbackCoverSpec).And(mediumBudgetSpec.Or(highBudgetSpec)));
            president.SetSpecification(mediumBudgetSpec.Or(highBudgetSpec));
            ceo.SetSpecification(extremeBudgetSpec);
            defaultEmployee.SetSpecification(defaultSpec);

            // Chain of responsibility: director => cfo => president => ceo (börja med den som har lägst makt)
            director.SetSuccessor(cfo);
            cfo.SetSuccessor(president);
            president.SetSuccessor(ceo);
            ceo.SetSuccessor(defaultEmployee); // om inte ens CEO kan publicera så kommer vi försöka med "defaultEmployee" vilket kommer ge fel

            // Först nu så används strukturen ovan
            // Gå igenom alla böcker och försök publicera
            // Börja med lägsta nivån (director)
            books.ForEach(book => director.PublishBook(book));

            CollectionAssert.AreEqual(
                new[] {
                "CFO Christine approved publication of The Stand.",
                "Successfully published The Stand.",

                "CFO Christine approved publication of The Hobbit.",
                "Successfully published The Hobbit.",

                "DirectorOfPublishing Dave approved publication of The Name of the Wind.",
                "Successfully published The Name of the Wind.",

                "President Bob approved publication of To Kill a Mockingbird.",
                "Successfully published To Kill a Mockingbird.",

                "Unable to publish: 1984.",

                "CEO Alice approved publication of Jane Eyre.",
                "Successfully published Jane Eyre.",

                },

                Logging.Message);

        }


    }

    public class PublicationProcess
    {
        public void PublishBook(Book book)
        {
            book.Publish();
        }

        public void FailedPublication(Book book)
        {
            Logging.Log($"Unable to publish: {book}.");
        }
    }

    public enum CoverType
    {
        Digital,
        Hard,
        Paperback
    }

    public interface IPublishable
    {
        string Author { get; set; }
        CoverType CoverType { get; }
        decimal PublicationCost { get; set; }
        void Publish();
        string Title { get; set; }
    }

    public class Book : IPublishable
    {
        public string Author { get; set; }
        public CoverType CoverType { get; set; }
        public decimal PublicationCost { get; set; }
        public string Title { get; set; }

        public Book(string title, string author, CoverType coverType, decimal publicationCost)
        {
            this.Author = author;
            this.PublicationCost = publicationCost;
            this.CoverType = coverType;
            this.Title = title;
        }

        public void Publish()
        {
            Logging.Log($"Successfully published {this}.");
        }

        public override string ToString()
        {
            return Title; // $"{CoverType} cover '{Title}' by {Author} for {PublicationCost:C2}";
        }
    }

    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T expression);
    }

    public class Specification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _expression;

        // Funktionen kan t.ex vara: 
        // - är en bok dyrare än 3000 att publicera
        // - är en bok papercover?

        public Specification(Func<T, bool> expression)
        {
            this._expression = expression;
        }

        // Anropa funktionen med en parameter, t.ex en bok/sträng/heltal
        public bool IsSatisfiedBy(T expression)
        {
            return this._expression(expression);
        }
    }

    public enum Position
    {
        CEO,
        President,
        CFO,
        DirectorOfPublishing,
        Default
    }

    public interface IEmployee<T>
    {
        void PublishBook(T book);
        void SetSpecification(ISpecification<T> specification);
        void SetSuccessor(IEmployee<T> employee);
    }

    // T är Book i vårat fall. En anställd kan publicera en bok.
    // Den typ som våra anställda ska hantera
    public class Employee<T> : IEmployee<T> where T : IPublishable
    {
        private IEmployee<T> _successor;
        private readonly string _name;
        private ISpecification<T> _specification;
        private readonly Action<T> _publicationAction;
        private readonly Position _position;

        public Employee(string name, Position position, Action<T> publicationAction)
        {
            _name = name;
            _position = position;
            _publicationAction = publicationAction;
        }

        // Kolla om jag har rättigheter att publicera boken genom att koll på min specification
        public bool CanApprove(T book)
        {
            if (_specification != null && book != null)
            {
                return _specification.IsSatisfiedBy(book);
            }
            return false;
        }

        // Alla employees (oavsett position) kan ha en chans att publicera en bok
        public void PublishBook(T book)
        {
            if (CanApprove(book))
            {
                // En employee som har Position=default har inga rättigheter
                if (_position != Position.Default)
                {
                    Logging.Log($"{_position} {_name} approved publication of {book}.");
                }

                // Publicera boken!
                _publicationAction.Invoke(book);
            }
            else
            {
                // Jag har inte rättigheter => skicka till nästa i kedjan (typ min chef)
                // Om inga successors finns => gör inget
                _successor?.PublishBook(book);
            }
        }

        // Sätt vilken behörighet du har och vem som är näst i kedjan

        public void SetSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public void SetSuccessor(IEmployee<T> employee)
        {
            _successor = employee;
        }
    }
}
