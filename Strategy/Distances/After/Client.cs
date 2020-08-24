
using System;
using System.Collections.Generic;

namespace DesignPatterns.Strategy.Distances.After
{
    class Client : IClient
    {
        public IEnumerable<double> CalculateManhattanThenBird(Point p1, Point p2)
        {
            var myservice = new MyMap(new Manhattan());

            yield return myservice.CalculateDistance(p1, p2);

            myservice.ChangeStrategy(new Bird());

            yield return myservice.CalculateDistance(p1, p2); 
            
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

    class MyMap
    {
        IStrategy _strategy;

        public MyMap(IStrategy strategy)
        {
            _strategy = strategy;
        }

        internal double CalculateDistance(Point p1, Point p2)
        {
            return _strategy.Distance(p1, p2);
        }

        internal void ChangeStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}
