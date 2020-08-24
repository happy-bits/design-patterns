
using System;
using System.Collections.Generic;

namespace DesignPatterns.TemplateMethod.Distances.Before
{
    class Client : IClient
    {
        public IEnumerable<double> Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
