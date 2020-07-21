// Eget exempel

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Lab
    {
        [TestMethod]
        public void probably_not_all_50_dices_will_give_same()
        {
            var result = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                var numberDice = new NumberDice(1, 6);
                result.Add(numberDice.Roll());
            }

            // We are really unlucky if this by accident passes
            Assert.IsFalse( result.All(r => r == result[0]) );
        }

        [TestMethod]
        public void all_50_dices_should_be_4()
        {
            var result = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                var numberDice = new FakeNumberDice(4, 1, 6);
                result.Add(numberDice.Roll());
            }

            Assert.IsTrue(result.All(r => r == 4));
        }

        abstract class Dice<T>
        {
            protected static Random _rnd = new Random();

            public abstract T Roll();
            public abstract int NrOfSides { get; }
        }

        class NumberDice : Dice<int>
        {
            public NumberDice(int min, int max)
            {
                if (min > max)
                    throw new ArgumentException();

                Min = min;
                Max = max;
            }

            public int Min { get; }
            public int Max { get; }

            public override int NrOfSides => Min - Max + 1;

            public override int Roll()
            {
                return _rnd.Next(Min, Max + 1);
            }
        }

        class FakeNumberDice: NumberDice
        {
            public FakeNumberDice(int result, int min, int max):base(min, max)
            {
                Result = result;
            }

            public int Result { get; }

            public override int Roll()
            {
                return Result;
            }
        }

        class SymbolDice : Dice<string>
        {
            public SymbolDice(params string[] sides)
            {
                if (sides == null || !sides.Any())
                    throw new ArgumentException();

                Sides = sides;
            }

            public string[] Sides { get; }

            public override int NrOfSides => Sides.Count();

            public override string Roll()
            {
                return Sides[_rnd.Next(Sides.Length)];
            }
        }

        class FakeSymbolDice : SymbolDice
        {
            public FakeSymbolDice(string result, params string[] sides):base(sides)
            {
                Result = result;
            }

            public string Result { get; }

            public override string Roll()
            {
                return Result;
            }
        }

        class UniqueSymbolDice : SymbolDice
        {
            public UniqueSymbolDice(params string[] sides):base(sides)
            {
                if (sides.Distinct().Count() != sides.Count())
                    throw new ArgumentException();
            }
        }


        class FakeUniqueSymbolDice : UniqueSymbolDice
        {
            public FakeUniqueSymbolDice(string result, params string[] sides) : base(sides)
            {
                Result = result;
            }

            public string Result { get; }
        }

    }
}
