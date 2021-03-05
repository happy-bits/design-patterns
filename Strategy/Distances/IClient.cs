
namespace DesignPatterns.Strategy.Distances
{
    interface IClient
    {
        string[] CalculateManhattanThenBird(Point p1, Point p2);
    }
}