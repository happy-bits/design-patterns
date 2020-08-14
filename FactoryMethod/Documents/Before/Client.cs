//// No pattern

//using System;
//using System.Collections.Generic;

//namespace DesignPatterns.FactoryMethod.Documents.Before
//{
//    class Client : IClient
//    {
//        public IEnumerable<Shape> Run(int num, string factoryname)
//        {
//            var result = new List<Shape>();

//            var factory = new Factory(factoryname);

//            for (int i = 0; i < num; i++)
//            {
//                result.Add(factory.GetShape());
//            }
//            return result;
//        }

//    }

//    // Nackdel: klassen behöver uppdateras och kommer växa när nya sorters fabriker behövs

//    // Nackdel: det kommer dyka upp metoder och fält som bara är intressanta för vissa fabriker, t.ex "DividibleByThree" behövs här bara i fallet "TriangleTriangleCircle".

//    class Factory
//    {
//        private readonly string _factoryname;
//        private int _counter = 0;

//        private static bool IsEven(int number) => number % 2 == 0;
//        private static bool DividableByThree(int number) => number % 3 == 0;

//        public Factory(string factoryname)
//        {
//            _factoryname = factoryname;
//        }
//        internal Shape GetShape()
//        {
//            _counter++;
//            switch (_factoryname)
//            {
//                case "SquareCircle":

//                    if (IsEven(_counter))
//                        return new Circle();
//                    else
//                        return new Square();

//                case "TriangleTriangleCircle":

//                    if (DividableByThree(_counter))
//                        return new Circle();
//                    else
//                        return new Triangle();

//                default: throw new ArgumentException();
//            }
//        }
//    }


//}
