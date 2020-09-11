using System.Collections.Generic;

namespace DesignPatterns.Visitor.Exports
{
    interface IClient
    {
        void ConfigureExporter(string exporterName);
        string ExportCircle(double x, double y, double radius);
    }
}