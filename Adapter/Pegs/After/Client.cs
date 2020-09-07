// Adapter med arv
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Adapter.Pegs.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            // SquarePage har sidan = 2
            // RoundPeg har radien 1 och alltså diametern 2

            var squarePeg = new SquarePeg(2);
            var adapter = new SquarePegAdapter(squarePeg);

            {
                var roundHole = new RoundHole(1);
                var roundPeg = new RoundPeg(1);
                Assert.IsTrue(roundHole.Fits(roundPeg));
            }

            {
                var roundHole = new RoundHole(1);
                var roundPeg = new RoundPeg(1.001);
                Assert.IsFalse(roundHole.Fits(roundPeg));
            }

            {
                var roundHole = new RoundHole(1);
                //roundHole.Fits(squarePeg); // detta går ej
                Assert.IsFalse(roundHole.Fits(adapter));
            }
            {
                var roundHole = new RoundHole(Math.Sqrt(2));
                Assert.IsTrue(roundHole.Fits(adapter));
            }
            {
                var roundHole = new RoundHole(Math.Sqrt(2) - 0.0001);
                Assert.IsFalse(roundHole.Fits(adapter));
            }

        }
    }

    // Får ej ändra (Oförändrad)
    class RoundHole
    {
        public RoundHole(double radius)
        {
            Radius = radius;
        }

        public double Radius { get; }

        public bool Fits(RoundPeg peg) => peg.Radius <= Radius;
    }

    // (Oförändrad)
    class SquarePeg
    {

        public SquarePeg(double width)
        {
            Width = width;
        }

        public double Width { get; }
    }

    // Behöver ändra Radius till "virtual"
    class RoundPeg 
    {
        public RoundPeg(double radius)
        {
            Radius = radius;
        }

        public virtual double Radius { get; } 
    }

    // Ny klass som gör att squarepeg beter sig som roundpeg
    class SquarePegAdapter : RoundPeg
    {
        private readonly SquarePeg _squarePeg;

        public SquarePegAdapter(SquarePeg squarePeg):base(0)
        {
            _squarePeg = squarePeg;
        }

        public override double Radius => _squarePeg.Width * Math.Sqrt(2) / 2;
    }
}
