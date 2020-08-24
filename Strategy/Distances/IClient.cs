using System.Collections.Generic;

namespace DesignPatterns.Strategy.Distances
{
    interface IClient
    {
        IEnumerable<double> CalculateManhattanThenBird(Point p1, Point p2);
    }
}