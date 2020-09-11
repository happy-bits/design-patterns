
using System;
using System.Collections.Generic;

namespace DesignPatterns.Visitor.Exports.Before
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

        public object CreateCircle(double x, double y, double radius) =>
            new Circle
            {
                X = x,
                Y = y,
                Radius = radius
            };

        public object CreateDot(double x, double y) =>
            new Dot
            {
                X = x,
                Y = y
            };

        public IEnumerable<string> ExportShapes(params object[] shapes)
        {
            foreach (object shape in shapes)
            {
                IShape typedShape = (IShape)shape;

                // Nackdel: denna switch-sats
                string result = typedShape switch
                {
                    Circle circle => _exporter.VisitCircle(circle),
                    Dot dot => _exporter.VisitDot(dot),
                    _ => throw new NotImplementedException()
                };

                yield return result;
            }
        }
    }
    // Komponent 

    interface IShape
    {
        void Move(double x, double y);
        void Draw();
    }

    // Konkret komponent

    class Circle : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }

        public void Draw() => throw new NotImplementedException();
        public void Move(double x, double y) => throw new NotImplementedException();
    }

    class Dot : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }

        public void Draw() => throw new NotImplementedException();
        public void Move(double x, double y) => throw new NotImplementedException();
    }

    interface IVisitor
    {
        string VisitDot(Dot dot);
        string VisitCircle(Circle circle);
    }

    class XMLExportVisitor : IVisitor
    {
        public string VisitCircle(Circle circle) => $"<Circle><X>{circle.X}</X><Y>{circle.Y}</Y><Radius>{circle.Radius}</Radius></Circle>";
        public string VisitDot(Dot dot) => $"<Dot><X>{dot.X}</X><Y>{dot.Y}</Y></Dot>";
    }

    class JsonExportVisitor : IVisitor
    {
        private const char _left = '{';
        private const char _right = '}';

        public string VisitCircle(Circle circle) => $"{_left}x:{circle.X},y:{circle.Y},radius:{circle.Radius}{_right}";
        public string VisitDot(Dot dot) => $"{_left}x:{dot.X},y:{dot.Y}{_right}";
    }
}
