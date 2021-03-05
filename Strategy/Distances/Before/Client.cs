
using System;

namespace DesignPatterns.Strategy.Distances.Before
{
    class Client : IClient
    {
        public string[] CalculateManhattanThenBird(Point p1, Point p2)
        {
            
            var myservice = new MapService(Strategy.Manhattan);

            var result1 = myservice.GetDistance(p1, p2); 

            myservice.ChangeStrategy(Strategy.Bird);

            var result2 = myservice.GetDistance(p1, p2);

            return new[] { result1, result2 };
        }
    }


    enum Strategy
    {
        Manhattan, Bird
    }

    class MapService
    {
        Strategy _strategy;

        // Nackdel: denna klass växer så fort en ny strategi behövs

        public double BirdDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public double ManhattanDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p2.Y - p1.Y);
        }

        public MapService(Strategy strategy)
        {
            _strategy = strategy;
        }

        public string GetDistance(Point p1, Point p2)
        {
            var distance = _strategy switch
            {
                Strategy.Manhattan => ManhattanDistance(p1, p2),
                Strategy.Bird => BirdDistance(p1, p2),
                _ => throw new InvalidOperationException() // Nackdel: denna behövs
            };

            return $"Distance between p1 and p2: {distance}";
            
        }

        public void ChangeStrategy(Strategy strategy)
        {
            _strategy = strategy;
        }
    }
}
