using System;
using System.Collections.Generic;

namespace DesignPatterns.Visitor.Exports.Before
{
    class Client : IClient
    {
        public void ConfigureExporter(string exporterName)
        {
            throw new NotImplementedException();
        }

        public string ExportCircle(double x, double y, double radius)
        {
            throw new NotImplementedException();
        }
    }
}
