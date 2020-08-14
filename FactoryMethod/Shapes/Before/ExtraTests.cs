using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.FactoryMethod.Shapes.Before
{
    [TestClass]
    public class ExtraTests
    {
        [TestMethod]
        public void test_SquareCircleFactory()
        {
            var factory = new Creator("SC");

            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
        }

        [TestMethod]
        public void test_TriangleTriangleCircleFactory()
        {
            var factory = new Creator("TTC");

            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Triangle);
        }
    }
}
