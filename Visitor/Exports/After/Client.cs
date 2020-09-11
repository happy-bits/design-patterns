// adds XML export support to the class hierarchy of geometric shapes.

using System;
using System.Collections.Generic;



namespace DesignPatterns.Visitor.Exports.After
{
    class Client : IClient
    {
        IVisitor _exporter;

        public void ConfigureExporter(string exporterName)
        {
            _exporter = exporterName switch
            {
                "XML" => new XMLExportVisitor(),
                "Json" => new JsonExportVisitor(),
                _ => throw new ArgumentException()
            };
        }

        public string ExportCircle(double x, double y, double radius)
        {
            var circle = new Circle
            {
                X = x,
                Y = y,
                Radius = radius
            };

            return _exporter.VisitCircle(circle);
        }


        // Komponent
        interface IShape
        {
            void Move(double x, double y);
            void Draw();
            void Accept(IVisitor visitor);
        }

        // Konkret komponent


        class Circle : IShape
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Radius { get; set; }

            // En form kan acceptera lite olika visitors (t.ex XmlExport eller JsonExport)
            public void Accept(IVisitor visitor)
            {
                visitor.VisitCircle(this);
            }

            public void Draw() => throw new NotImplementedException();
            public void Move(double x, double y) => throw new NotImplementedException();
        }

        class Dot : IShape
        {
            public void Accept(IVisitor visitor) => throw new NotImplementedException();
            public void Draw() => throw new NotImplementedException();
            public void Move(double x, double y) => throw new NotImplementedException();

        }

        class Rectangle : IShape
        {
            public void Accept(IVisitor visitor) => throw new NotImplementedException();
            public void Draw() => throw new NotImplementedException();
            public void Move(double x, double y) => throw new NotImplementedException();

        }
        class CompoundGraphic : IShape
        {
            public void Accept(IVisitor visitor) => throw new NotImplementedException();
            public void Draw() => throw new NotImplementedException();
            public void Move(double x, double y) => throw new NotImplementedException();
        }

        interface IVisitor
        {
            string VisitDot(Dot dot);
            string VisitCircle(Circle circle);
            string VisitRectangle(Rectangle rectangle);
            string VisitCompoundGraphic(CompoundGraphic compoundGraphic);
        }

        class XMLExportVisitor : IVisitor
        {
            public string VisitCircle(Circle circle) => $"<Circle><X>{circle.X}</X><Y>{circle.Y}</Y><Radius>{circle.Radius}</Radius></Circle>";

            public string VisitCompoundGraphic(CompoundGraphic compoundGraphic) => throw new NotImplementedException();
            public string VisitDot(Dot dot) => throw new NotImplementedException();
            public string VisitRectangle(Rectangle rectangle) => throw new NotImplementedException();
        }

        class JsonExportVisitor : IVisitor
        {
            private const char _left = '{';
            private const char _right = '}';
            public string VisitCircle(Circle circle) => $"{_left}x:{circle.X},y:{circle.Y},radius:{circle.Radius}{_right}";
            public string VisitCompoundGraphic(CompoundGraphic compoundGraphic) => throw new NotImplementedException();
            public string VisitDot(Dot dot) => throw new NotImplementedException();
            public string VisitRectangle(Rectangle rectangle) => throw new NotImplementedException();
        }
    }
}
