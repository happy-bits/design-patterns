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
    public class Dices2
    {

        [TestMethod]
        public void Ex1()
        {
            var client = new Client();
            client.Run("fake");
        }


        [TestMethod]
        public void Ex2()
        {
            var client = new Client();
            client.Run("real");
        }

        class Client
        {
            public void Run(string environment)
            {
                var diceFactory = new DiceFactory(environment);
                Dice d6 = diceFactory.GetDice6();
                Dice d10 = diceFactory.GetDice10();
            }
        }

        abstract class Dice
        {
            protected static Random _rnd = new Random();

            public abstract void Roll();
            public abstract int NrOfSides { get; }
            public int Value { get; protected set; }
        }

        class NumberDice : Dice
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

        class Dice10 : NumberDice
        {
            public Dice10() : base(1, 6)
            {
            }
        }

        class FakeDice10 : Dice10
        {
            public FakeDice10(int result) : base()
            {
                Value = result;
            }

            public override void Roll()
            {
            }

        }


        class DiceFactory
        {
            private string environment;

            public DiceFactory(string environment)
            {
                this.environment = environment;
            }

            public Dice GetDice10()
            {
                switch(environment)
                {
                    case "fake": return new FakeDice10(3);
                    case "real": return new Dice10();
                    default:throw new Exception();
                }
            }

            public Dice GetDice6()
            {
                switch (environment)
                {
                    case "fake": return new FakeDice6(3);
                    case "real": return new Dice6();
                    default: throw new Exception();
                }
            }
        }


    }
}
