
using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite.Graphics.After
{
    class Client : IClient
    {
        private static Queue<string> _events = new Queue<string>();

        public Queue<string> DrawStuff()
        {
            var dot = new Dot(3,4);
            dot.Draw();

            var circle = new Circle(5, 6, 100);
            circle.Move(20, 30);
            circle.Draw();


            /*
            
             root________________
             |                   |     
             compound1__         compound2_____
             |          |        |             |
             dot        circle   compound3     dot2
                                 |
                                 circle2
             */
            var dot2 = new Dot(0, 0);

            var circle2 = new Circle(55, 66, 77);

            var compound1 = new CompoundGraphic();
            var compound2 = new CompoundGraphic();
            var compound3 = new CompoundGraphic();

            compound1.Add(dot, circle);
            compound2.Add(compound3, dot2);
            compound3.Add(circle2);

            var root = new CompoundGraphic();
            root.Add(compound1, compound2);

            root.Move(500, 600);
            root.Draw();
            return _events;
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


        class Dot: Graphic
        {
            public Dot(double x, double y)
            {
                X = x;
                Y = y;
            }

            public override void Draw()
            {
                _events.Enqueue($"Drawing dot on position ({X},{Y})");
            }
        }

        class Circle: Graphic
        {
            public Circle(double x, double y, double radius)
            {
                X = x;
                Y = y;
                Radius = radius;
            }

            public double Radius { get; }

            public override void Draw()
            {
                _events.Enqueue($"Drawing circle with radius {Radius} on position ({X},{Y})");
            }
        }

        class CompoundGraphic: Graphic
        {
            List<Graphic> _children = new List<Graphic>();

            public void Add(params Graphic[] graphics) => _children.AddRange(graphics);

            public void Remove(Graphic graphics) => _children.Remove(graphics);

            public override void Move(double x, double y)
            {
                foreach (var child in _children)
                {
                    child.Move(x, y);
                }
            }

            public override void Draw()
            {
                foreach (var child in _children)
                {
                    child.Draw();
                }
            }
        }
    }
}
