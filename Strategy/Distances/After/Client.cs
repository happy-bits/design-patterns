
using System;

namespace DesignPatterns.Strategy.Distances.After
{
    class Client : IClient
    {
        public string[] CalculateManhattanThenBird(Point p1, Point p2)
        {
            var myservice = new MapService(new Manhattan());

            var result1 = myservice.GetDistance(p1, p2);

            myservice.ChangeStrategy(new Bird());

            var result2 = myservice.GetDistance(p1, p2);

            return new[] { result1, result2 };
        }
    }

    interface IStrategy
    {
        double Distance(Point p1, Point p2);
    }

    // Fördel: denna klass handlar bara om manhattan-strategin och blir inte "befläckad" av Bird-strategin
    
    class Manhattan : IStrategy
    {
        public double Distance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p2.Y - p1.Y);
        }
    }


    class Bird : IStrategy
    {
        public double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }

    // Fördel: denna klass är oförändrad när nya stragier tillkommer

    class MapService
    {
        IStrategy _strategy;

        public MapService(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public string GetDistance(Point p1, Point p2)
        {
            return $"Distance between p1 and p2: {_strategy.Distance(p1, p2)}";
        }

        public void ChangeStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}
