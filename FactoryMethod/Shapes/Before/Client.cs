// No pattern

using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Shapes.Before
{
    class Client : IClient
    {
        public IEnumerable<Shape> Run(int num, string factoryname)
        {
            var result = new List<Shape>();

            var creator = new Creator(factoryname);

            for (int i = 0; i < num; i++)
            {
                result.Add(creator.GetShape());
            }
            return result;
        }

    }

    // Nackdel: klassen behöver uppdateras och kommer växa när nya sorters fabriker behövs

    // Nackdel: ex DividibleByThree behövs bara i fallet "TTC".

    class Creator
    {
        private readonly string _factoryname;
        private int _counter = 0;

        private static bool IsEven(int number) => number % 2 == 0;
        private static bool DividableByThree(int number) => number % 3 == 0;

        public Creator(string factoryname)
        {
            _factoryname = factoryname;
        }
        internal Shape GetShape()
        {
            _counter++;
            switch (_factoryname)
            {
                case "SC":

                    if (IsEven(_counter))
                        return new Circle();
                    else
                        return new Square();

                case "TTC":

                    if (DividableByThree(_counter))
                        return new Circle();
                    else
                        return new Triangle();

                default: throw new ArgumentException();
            }
        }
    }


}
