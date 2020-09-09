﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Prototype.Shapes.Before2
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var r = new Rectangle
            {
                Width = 5,
                Height = 6,
                Color = "blue",
                X = 7,
                Y = 8
            };

            Shape r2 = r.Clone();

            r.Width += 1000;
            r.Height += 1000;
            r.Color += 1000;
            r.X += 1000;
            r.Y += 1000;

            // Verifiera att "r" har ändrats men inte "r2"

            Assert.AreEqual(r.Width, ((Rectangle)r2).Width + 1000);
            Assert.AreEqual(r.Height, ((Rectangle)r2).Height + 1000);
            Assert.AreEqual(r.Color, ((Rectangle)r2).Color + 1000);
            Assert.AreEqual(r.X, ((Rectangle)r2).X + 1000);
            Assert.AreEqual(r.Y, ((Rectangle)r2).Y + 1000);

            Circle c = new Circle
            {
                Radius = 5
            };

            Shape c2 = c.Clone();

            c.Radius += 1000;

            // Verifiera att "c" har ändrats men inte "c2"

            Assert.AreEqual(c.Radius, ((Circle)c2).Radius + 1000);
        }
    }

    abstract class Shape
    {
        public string Color { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public abstract Shape Clone();
    }

    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public override Shape Clone()
        {
            return new Rectangle
            {
                Width = Width,
                Height = Height,

                // Nackdel: kod upprepar sig (i Circle)
                X=X,
                Y=Y,
                Color=Color
            };
        }
    }

    class Circle : Shape
    {
        public double Radius { get; set; }

        public override Shape Clone()
        {
            return new Circle
            {
                Radius = Radius,
                X = X,
                Y = Y,
                Color = Color
            };
        }
    }
}
