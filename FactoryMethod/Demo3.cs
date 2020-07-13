/*
 
FACTORY METHOD - design pattern

Two types of "DocumentCreator":
- HtmlCreator
- MarkdownCreator

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Demo3
    {
        static readonly List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            var c = new Client();

            c.ClientCode(new MockYatzyGameCreator());
            CollectionAssert.AreEqual(new[] {
                "1,1,1,1,1",
                "2,2,2,2,2",
                "3,3,3,3,3",
            }, _log);

            c.ClientCode(new YatzyCreator());

            Assert.AreEqual(6, _log.Count);


        }

        // "Creator"
        abstract class YatzyCreatorBase
        {

            private YatzyGameBase _game;

            internal abstract YatzyGameBase Create();

            internal DiceRoll Roll()
            {
                _game ??= Create();
                var result = _game.Roll();
                return result;
            }
        }

        // "Concrete Creator" 
        class YatzyCreator : YatzyCreatorBase
        {
            internal override YatzyGameBase Create() => new YatzyGame();
        }


        // "Concrete Creator" 
        class MockYatzyGameCreator : YatzyCreatorBase
        {
            internal override YatzyGameBase Create() => new MockYatzyGame();
        }


        // "Product"
        abstract class YatzyGameBase
        {
            public abstract DiceRoll Roll();
        }

        class YatzyGame : YatzyGameBase
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

        class MockYatzyGame : YatzyGameBase
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
            public void ClientCode(YatzyCreatorBase creator)
            {
                DiceRoll result;

                result = creator.Roll();
                _log.Add(result.ToString());

                result = creator.Roll();
                _log.Add(result.ToString());

                result = creator.Roll();
                _log.Add(result.ToString());

            }
        }
    }
}
