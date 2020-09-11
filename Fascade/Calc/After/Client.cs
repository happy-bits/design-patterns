/*
 Good:
 - Client code is easier to understand
 - Since BigBallOfMudCalculator is not used the client code don't by accident call crazy methods
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Fascade.Calc.After
{
    class Client : IClient
    {
        public void AddThreeAndSubtractOne()
        {
            // "Client code"
            var calculator = new CalculatorFascade();
            calculator.Increase();
            calculator.Increase();
            calculator.Increase();
            calculator.Decrease();

            double result = calculator.Value;

            Assert.AreEqual(2, result);
        }
    }


    // Exercise: create "CalculatorFascade"

    class CalculatorFascade
    {
        private readonly BigBallOfMudCalculator _mud;

        public CalculatorFascade() => _mud = new BigBallOfMudCalculator(0, 0, "", null);

        public double Value => _mud.DoStuff5();

        public void Decrease() => _mud.RemoveFromXx();

        public void Increase() => _mud.PushMyNumberUp();
    }

    // The following two classes is written by somebody else. 
    // You're not allowed to change these (this might break other parts of you system)

    class BigBallOfMudCalculator
    {
        double _xx;
        private double _a;
        private double _b;
        private string _c;
        private readonly AnotherBallOfMud _another;

        public BigBallOfMudCalculator(double a, double b, string c, AnotherBallOfMud another)
        {
            _a = a;
            _b = b;
            _c = c;
            _another = another;
        }

        public void PushMyNumberUp()
        {
            _xx++;
        }

        public void RemoveFromXx()
        {
            _xx--;
        }

        public void Multiply(int x)
        {
            _xx *= x;
        }

        public void DoStuff1()
        {
            _a = _a * _b;
        }
        public void DoStuff2()
        {
            _another.DoOtherStuff();
        }
        public void DoStuff3()
        {
            _c = "aaaaa";
        }
        public string DoStuff4()
        {
            return _c;
        }
        public double DoStuff5()
        {
            return _xx;
        }
        public double DoStuff6()
        {
            throw new System.Exception();
        }
        public double DoStuff7()
        {
            throw new System.Exception();
        }
    }

    class AnotherBallOfMud
    {
        public AnotherBallOfMud(int a, int b, int c)
        {

        }

        public void DoOtherStuff()
        {
        }
    }
}
