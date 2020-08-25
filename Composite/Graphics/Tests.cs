using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Composite.Graphics
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new After.Client()
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var result = client.DrawStuff();

                AssertEqualCollection(new[] {
                    "Drawing dot on position (3,4)",
                    "Drawing circle with radius 100 on position (20,30)",

                    "Drawing dot on position (500,600)",
                    "Drawing circle with radius 100 on position (500,600)",
                    "Drawing circle with radius 77 on position (500,600)",
                    "Drawing dot on position (500,600)",

                    }, result);
            }
        }
    }

    abstract class Graphic
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }

        public virtual void Move(double x, double y)
        {
            X = x;
            Y = y;
        }
        public abstract void Draw();
    }


    class Dot : Graphic
    {
        public EventHandler<string> OnDraw { get; set; }

        public Dot(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override void Draw()
        {
            OnDraw?.Invoke(this, $"Drawing dot on position ({X},{Y})");
        }
    }

    class Circle : Graphic
    {
        public EventHandler<string> OnDraw { get; set; }


        public Circle(double x, double y, double radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public double Radius { get; }

        public override void Draw()
        {
            OnDraw?.Invoke(this, $"Drawing circle with radius {Radius} on position ({X},{Y})");
        }
    }





}
