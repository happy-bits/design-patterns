
using System.Collections.Generic;

namespace DesignPatterns.Visitor.Exports
{
    interface IClient
    {
        void ConfigureExporter(string exporterName);
        object CreateCircle(double x, double y, double radius);
        object CreateDot(double x, double y);
        IEnumerable<string> ExportShapes(params object[] shapes);
    }
}