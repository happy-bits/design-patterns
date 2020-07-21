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
        public void not_all_rolls_should_be_the_same()
        {
            var result = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                var numberDice = new Dice6();
                numberDice.Roll();
                result.Add(numberDice.Value);
            }

            // We are really unlucky if this by accident passes
            Assert.IsFalse( result.All(r => r == result[0]) );
        }

        [TestMethod]
        public void all_50_dices_should_be_4()
        {
            var test = new FakeDice6(4);


            var result = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                var numberDice = new FakeDice6(4);
                numberDice.Roll();
                result.Add(numberDice.Value);
            }

            Assert.IsTrue(result.All(r => r == 4));
        }

        [TestMethod]
        public void cant_get_straight_all_the_time()
        {
            var y = new Yatzy();

            int nrOfStraights = 0;

            for (int i = 0; i < 50; i++)
            {
                y.Roll();
                if (y.IsSmallStraight())
                    nrOfStraights++;
            }

            // Hope we don't get only straight
            Assert.IsTrue(nrOfStraights != 50);
        }

        [TestMethod]
        public void should_be_straight_1()
        {
            var y = new Yatzy(
                new FakeDice6(1), 
                new FakeDice6(2), 
                new FakeDice6(3), 
                new FakeDice6(4), 
                new FakeDice6(5)
            );
            y.Roll();

            bool result = y.IsSmallStraight();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void should_be_straight_2()
        {
            var y = new Yatzy(
                new FakeDice6(5),
                new FakeDice6(1),
                new FakeDice6(3),
                new FakeDice6(2),
                new FakeDice6(4)
            );
            y.Roll();

            bool result = y.IsSmallStraight();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void should_not_be_straight_1()
        {
            var y = new Yatzy(
                new FakeDice6(1),
                new FakeDice6(1),
                new FakeDice6(3),
                new FakeDice6(4),
                new FakeDice6(5)
            );
            y.Roll();

            bool result = y.IsSmallStraight();

            Assert.IsFalse(result);
        }

        class Yatzy
        {
            public Dice6[] _dices = new Dice6[5];

            public Yatzy()
            {
                for(int i=0; i<5; i++)
                    _dices[i] = new Dice6();
            }

            public Yatzy(Dice6 d1, Dice6 d2, Dice6 d3, Dice6 d4, Dice6 d5)
            {
                _dices[0] = d1;
                _dices[1] = d2;
                _dices[2] = d3;
                _dices[3] = d4;
                _dices[4] = d5;
            }

            private bool SameNumbers(IEnumerable<int> list1, IEnumerable<int> list2) => !list1.Except(list2).Any();

            internal bool IsSmallStraight()
            {
                var allPossible = new int[] { 1, 2, 3, 4, 5 };
                var diceAsNumbers = _dices.Select(d => d.Value);
                return SameNumbers(allPossible, diceAsNumbers);
            }

            internal bool IsBigStraight()
            {
                var allPossible = new int[] { 2, 3, 4, 5, 6 };
                var diceAsNumbers = _dices.Select(d => d.Value);
                return SameNumbers(allPossible, diceAsNumbers);
            }

            internal void Roll()
            {
                for (int i = 0; i < 5; i++)
                    _dices[i].Roll();
            }
        }

        abstract class Dice<T>
        {
            protected static Random _rnd = new Random();

            public abstract void Roll();
            public abstract int NrOfSides { get; }
            public T Value { get; protected set; } // ev bara kunna nå denna efter att tärningen är kastad
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

            public override void Roll()
            {
                Value = _rnd.Next(Min, Max + 1);
            }
        }

        class Dice6 : NumberDice
        {
            public Dice6() : base(1, 6)
            {
            }
        }

        class FakeDice6 : Dice6
        {
            public FakeDice6(int result) : base()
            {
                Value = result;
            }

            public override void Roll()
            {
            }

        }
        class FakeNumberDice: NumberDice
        {
            public FakeNumberDice(int result, int min, int max):base(min, max)
            {
                Value = result;
            }
            
            public override void Roll()
            {
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

            public override void Roll()
            {
                Value = Sides[_rnd.Next(Sides.Length)];
            }
        }

        class FakeSymbolDice : SymbolDice
        {
            public FakeSymbolDice(string result, params string[] sides):base(sides)
            {
                Value = result;
            }

            public override void Roll()
            {
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
                Value = result;
            }
            public override void Roll()
            {
            }
        }

    }
}
