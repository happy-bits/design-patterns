/*
Academic example

Inspiration from
https://refactoring.guru/design-patterns/abstract-factory/csharp/example

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DesignPatterns.AbstractFactory
{
    [TestClass]
    public class GuruConceptual
    {
        [TestMethod]
        public void victorian()
        {
            var result = Client.Run(new VictorianFurnitureFactory());

            CollectionAssert.AreEqual(new[] {
                    $"Price for chair1: 100",
                    $"Price for chair2: 100",
                    $"Price for table: 1000",
                    $"Number of chairs to get discount: 4",
                    $"Price for table with chair1 and chair2: 1200"
                }, result);
        }

        [TestMethod]
        public void artdeco()
        {
            var result = Client.Run(new ArtdecoFurnitureFactory());

            CollectionAssert.AreEqual(new[] {
                    $"Price for chair1: 50",
                    $"Price for chair2: 50",
                    $"Price for table: 500",
                    $"Number of chairs to get discount: 2",
                    $"Price for table with chair1 and chair2: 580" // 20% discount óf chairs
                }, result);
        }

        // Exercise: create the classes below

        interface IFurnitureFactory
        {
            IChair CreateChair();
            AbstractTable CreateTable();
        }

        class VictorianFurnitureFactory : IFurnitureFactory
        {
            public IChair CreateChair()
            {
                return new VictorianChair();
            }

            public AbstractTable CreateTable()
            {
                return new VictorianTable();
            }
        }

        class ArtdecoFurnitureFactory : IFurnitureFactory
        {
            public IChair CreateChair()
            {
                return new ArtdecoChair();
            }

            public AbstractTable CreateTable()
            {
                return new ArtdecoTable();
            }
        }

        interface IChair
        {
            public decimal Price { get; }
        }

        class VictorianChair : IChair
        {
            public decimal Price => 100;
        }

        class ArtdecoChair : IChair
        {
            public decimal Price => 50;
        }

        abstract class AbstractTable
        {
            public abstract decimal Price { get; }

            public abstract int NrOfChairsToGetDiscount { get; }

            public static decimal ChairDiscount => 0.2M;

            public decimal PriceWithChairs(params IChair[] chairs)
            {
                // rabatt ges oavsett vilken typ av stol som ges

                var chairPrice = chairs.Sum(c => c.Price);
                
                if(chairs.Length>=NrOfChairsToGetDiscount) 
                    chairPrice *=  (1 - ChairDiscount);

                return chairPrice + Price;
            }

        }

        class VictorianTable : AbstractTable
        {
            public override decimal Price => 1000;
            public override int NrOfChairsToGetDiscount => 4;
        }

        class ArtdecoTable : AbstractTable
        {
            public override decimal Price => 500;
            public override int NrOfChairsToGetDiscount => 2;
        }

        class Client
        {
            public static string[] Run(IFurnitureFactory factory)
            {
                var chair1 = factory.CreateChair();
                var chair2 = factory.CreateChair();
                var table = factory.CreateTable();


                return new[] {
                    $"Price for chair1: {chair1.Price}",
                    $"Price for chair2: {chair2.Price}",
                    $"Price for table: {table.Price}",
                    $"Number of chairs to get discount: {table.NrOfChairsToGetDiscount}",
                    $"Price for table with chair1 and chair2: {(int)table.PriceWithChairs(chair1,chair2)}"
                };
            }
        }
    }
}
