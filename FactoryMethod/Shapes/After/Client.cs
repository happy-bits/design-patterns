// Factory method

using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Shapes.After
{
    class Client: IClient
    {
        readonly Dictionary<string, ShapeFactory> _factories = new Dictionary<string, ShapeFactory>
        {
            ["SquareCircle"] = new SquareCircleFactory(),
            ["TriangleTriangleCircle"] = new TriangleTriangleCircleFactory()
        };

        public IEnumerable<Shape> Run(int num, string factoryname)
        {
            ShapeFactory factory = _factories[factoryname]; // alternativ: SelectFactoryByName(factoryname);

            var result = new List<Shape>();

            for (int i = 0; i < num; i++)
            {
                result.Add(factory.GetShape());
            }
            return result;
        }

        // Det blir fortfarande en switch sats, men den är enklare, och omvandlar bara från en sträng till en fabrik
        private static ShapeFactory SelectFactoryByName(string factoryname) => factoryname switch
        {
            "SquareCircle" => new SquareCircleFactory(),
            "TriangleTriangleCircle" => new TriangleTriangleCircleFactory(),
            _ => throw new ArgumentException()
        };
    }

    abstract class ShapeFactory
    {
        protected int _counter = 0;

        public Shape GetShape()
        {
            _counter++;
            return CreateShape();
        }

        // Factory method

        abstract protected Shape CreateShape();
    }

    // Denna klass är kortare än "Factory" och kommer inte växa när fler fabriker behövs (bra)
    // Denna klass innehåller bara det som är relevant, inte t.ex DividableByThree (bra)
    class SquareCircleFactory : ShapeFactory
    {
        private static bool IsEven(int number) => number % 2 == 0;

        protected override Shape CreateShape()
        {
            if (IsEven(_counter))
                return new Circle();
            return new Square();
        }
    }

    class TriangleTriangleCircleFactory : ShapeFactory
    {
        private static bool DividableByThree(int number) => number % 3 == 0;

        protected override Shape CreateShape()
        {
            if (DividableByThree(_counter))
                return new Circle();
            return new Triangle();
        }
    }


}
