
// Hmmmm.... denna blev tyvärr bättre...

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Yatzys_NoPattern
    {

        [TestMethod]
        public void fake_yatzy_should_always_return_the_same()
        {
            var c = new Client();

            var result = c.RollDices(new FakeYatzy()); 
            CollectionAssert.AreEqual(new[] {
                "1,1,1,1,1",
                "2,2,2,2,2",
                "3,3,3,3,3",
            }, result);

        }

        [TestMethod]
        public void should_give_three_rolls()
        {
            var c = new Client();
            var result = c.RollDices(new Yatzy());
            Assert.AreEqual(3, result.Count);
        }   

        abstract class YatzyBase
        {
            abstract public DiceRoll Roll();
        }

        class Yatzy : YatzyBase
        {
            static Random _rnd = new Random();

            public override DiceRoll Roll() => new DiceRoll(
                    _rnd.Next(1, 7),
                    _rnd.Next(1, 7),
                    _rnd.Next(1, 7),
                    _rnd.Next(1, 7),
                    _rnd.Next(1, 7)
                    );
        }

        class FakeYatzy : YatzyBase
        {
            private readonly DiceRoll[] _diceRolls = new DiceRoll[] {
                new DiceRoll(1,1,1,1,1),
                new DiceRoll(2,2,2,2,2),
                new DiceRoll(3,3,3,3,3),
            };
            private int _index = 0;

            public override DiceRoll Roll() => _diceRolls[_index++ % _diceRolls.Length];
        }


        class Dice
        {
            public Dice(int i)
            {
                if (i < 1 || i > 6)
                    throw new ArgumentException();
                Value = i;
            }
            public int Value { get; }
        }

        class DiceRoll
        {
            public DiceRoll(int d1, int d2, int d3, int d4, int d5)
            {
                Value = new Dice[] { new Dice(d1), new Dice(d2), new Dice(d3), new Dice(d4), new Dice(d5) };
            }

            public DiceRoll(Dice d1, Dice d2, Dice d3, Dice d4, Dice d5)
            {
                Value = new Dice[] { d1, d2, d3, d4, d5 };
            }

            public Dice[] Value { get; }

            public override string ToString() => string.Join(",", Value.Select(d => d.Value));
        }

        // "Client"
        class Client
        {
            public List<string> RollDices(YatzyBase yatzy)
            {
                List<string> rolls = new List<string>();

                DiceRoll result;

                result = yatzy.Roll();
                rolls.Add(result.ToString());

                result = yatzy.Roll();
                rolls.Add(result.ToString());

                result = yatzy.Roll();
                rolls.Add(result.ToString());

                return rolls;

            }
        }
    }
}
