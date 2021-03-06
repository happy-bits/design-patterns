﻿// Fördel: vi vet att fabriken finns, den är typad, så vi behöver inte skicka in en sträng

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.FactoryMethod.Shapes.After
{
    [TestClass]
    public class ExtraTests
    {
        [TestMethod]
        public void test_SquareCircleFactory()
        {
            // Tydligare än new Creator("SquareCircle") (bra)
            // Ingen risk att råka ta fel
            var factory = new SquareCircleFactory(); 

            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
        }

        [TestMethod]
        public void test_TriangleTriangleCircleFactory()
        {
            var factory = new TriangleTriangleCircleFactory(); 

            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Triangle);
        }
    }
}
