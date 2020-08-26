using System.Collections.Generic;

namespace DesignPatterns.Memento.Distances
{
    interface IClient
    {
        IEnumerable<double> Calculate();
    }
}