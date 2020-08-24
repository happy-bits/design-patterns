using System.Collections.Generic;

namespace DesignPatterns.TemplateMethod.Distances
{
    interface IClient
    {
        IEnumerable<double> Calculate();
    }
}