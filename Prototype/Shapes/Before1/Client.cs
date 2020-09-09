
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.Prototype.Shapes.Before1
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

            Rectangle r2 = r.Clone();

            r.Width += 1000;
            r.Height += 1000;
            r.Color += 1000;
            r.X += 1000;
            r.Y += 1000;

            // Verifiera att "r" har ändrats men inte "r2"

            Assert.AreEqual(r.Width, r2.Width + 1000);
            Assert.AreEqual(r.Height, r2.Height + 1000);
            Assert.AreEqual(r.Color, r2.Color + 1000);
            Assert.AreEqual(r.X, r2.X + 1000);
            Assert.AreEqual(r.Y, r2.Y + 1000);

            Circle c = new Circle
            {
                Radius = 5
            };

            Circle c2 = c.Clone();

            c.Radius += 1000;

            // Verifiera att "c" har ändrats men inte "c2"

            Assert.AreEqual(c.Radius, c2.Radius + 1000);

            // Nackdel: om vi har en lista av "Shapes" så vet vi inte att Clone-metoden finns
        }
    }

    abstract class Shape
    {
        public string Color { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        // Fördel: vi får ut rätt typ direkt (och behöver inte casta däruppe)
        public Rectangle Clone()
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

        public Circle Clone()
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
