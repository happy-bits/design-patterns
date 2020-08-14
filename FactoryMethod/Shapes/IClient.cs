using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Shapes
{
    interface IClient
    {
        IEnumerable<Shape> Run(int num, string factoryname);
    }
}