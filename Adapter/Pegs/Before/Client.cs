
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Adapter.Pegs.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            // SquarePage har sidan = 2
            // RoundPeg har radien 1 och alltså diametern 2

            var squarePeg = new SquarePeg(2);

            // Triviala test
            {
                var roundHole = new RoundHole(1);
                var roundPeg = new RoundPeg(1);
                Assert.IsTrue(roundHole.Fits(roundPeg));
            }

            // Triviala test
            {
                var roundHole = new RoundHole(1);
                var roundPeg = new RoundPeg(1.001);
                Assert.IsFalse(roundHole.Fits(roundPeg));
            }

            {
                var roundHole = new RoundHole(1);
                Assert.IsFalse(roundHole.Fits(squarePeg));
            }
            {
                var roundHole = new RoundHole(Math.Sqrt(2));
                Assert.IsTrue(roundHole.Fits(squarePeg));
            }
            {
                var roundHole = new RoundHole(Math.Sqrt(2) - 0.0001);
                Assert.IsFalse(roundHole.Fits(squarePeg));
            }

        }
    }

    /*
     Uppgift:

        RoundHole ska även kunna acceptera "SquarePeg" i "Fits"-metoden. Men du får inte ändra denna (eller någon annan) klass
        Detta pga någon annan har skrivit koden (ex tredjepart)
    */

    // Får ej ändra (någon annan skrivit klassen)
    class RoundHole
    {
        public RoundHole(double radius)
        {
            Radius = radius;
        }

        public double Radius { get; }

        public bool Fits(RoundPeg peg) => peg.Radius <= Radius;

        // Nackdel: vi måste skapa denna metod (och vi kanske inte kan modifiera RoundHole)
        public bool Fits(SquarePeg peg) => peg.Width * Math.Sqrt(2) / 2 <= Radius;
    }
}
