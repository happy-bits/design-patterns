using System.Collections.Generic;

namespace DesignPatterns.Strategy.Distances
{
    interface IClient
    {
        IEnumerable<double> Calculate();
    }
}