
using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite.Graphics.After
{
    class Client : IClient
    {

        public IEnumerable<string> DrawStuff()
        {
            List<string> _events = new List<string>();

            void AddToEventLog(object sender, string e)
            {
                _events.Add(e);
            }

            /*

             compound0___________
             |                   |     
             compound1__         compound2_____
             |          |        |             |
             dot        circle   compound3     dot2
                                 |
                                 circle2
             */

            // Setup

            var dot = new Dot(3, 4);
            var dot2 = new Dot(0, 0);
            var circle = new Circle(5, 6, 100);
            var circle2 = new Circle(55, 66, 77);

            var compound0 = new CompoundGraphic();
            var compound1 = new CompoundGraphic();
            var compound2 = new CompoundGraphic();
            var compound3 = new CompoundGraphic();

            compound1.Add(dot, circle);
            compound2.Add(compound3, dot2);
            compound3.Add(circle2);
            compound0.Add(compound1, compound2);

            dot.OnDraw += AddToEventLog;
            dot2.OnDraw += AddToEventLog;
            circle.OnDraw += AddToEventLog;
            circle2.OnDraw += AddToEventLog;

            // Action

            dot.Draw();
            circle.Move(20, 30);
            circle.Draw();

            compound0.Move(500, 600);
            compound0.Draw();

            return _events;
        }

        

        class CompoundGraphic : Graphic
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
