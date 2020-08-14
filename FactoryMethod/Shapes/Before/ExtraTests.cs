using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.FactoryMethod.Shapes.Before
{
    [TestClass]
    public class ExtraTests
    {
        [TestMethod]
        public void test_SquareCircleFactory()
        {
            var factory = new Factory("SquareCircle");

            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
        }

        [TestMethod]
        public void test_TriangleTriangleCircleFactory()
        {
            var factory = new Factory("TriangleTriangleCircle");

            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Triangle);
        }
    }
}
