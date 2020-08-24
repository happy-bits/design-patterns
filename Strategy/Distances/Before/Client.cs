
using System;
using System.Collections.Generic;

namespace DesignPatterns.Strategy.Distances.Before
{
    class Client : IClient
    {
        public IEnumerable<double> CalculateManhattanThenBird(Point p1, Point p2)
        {
            
            var myservice = new MyMap(Strategy.Manhattan);

            yield return myservice.CalculateDistance(p1, p2); 

            myservice.ChangeStrategy(Strategy.Bird);

            yield return myservice.CalculateDistance(p1, p2);
        }
    }


    enum Strategy
    {
        Manhattan, Bird
    }

    class MyMap
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

        public MyMap(Strategy strategy)
        {
            _strategy = strategy;
        }

        public double CalculateDistance(Point p1, Point p2)
        {
            switch (_strategy)
            {
                case Strategy.Manhattan: return ManhattanDistance(p1, p2);
                case Strategy.Bird: return BirdDistance(p1, p2);
            }
            throw new InvalidOperationException(); // Nackdel: denna behövs
        }

        public void ChangeStrategy(Strategy strategy)
        {
            _strategy = strategy;
        }
    }
}
