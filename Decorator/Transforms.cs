/*
Own example
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace DesignPatterns.Decorator
{
    [TestClass]
    public class Transforms
    {
        [TestMethod]
        public void Ex1()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new RemoveDashesAtEdges(component);
            string result = decorator.Transform(input);

            Assert.AreEqual("Lorem     ipsum   dolor sit", result);
        }

        [TestMethod]
        public void Ex2()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new RemoveDoubleSpaces(component);
            string result = decorator.Transform(input);

            Assert.AreEqual("----Lorem ipsum dolor sit---", result);
        }

        [TestMethod]
        public void Ex3()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new AddParagraph(component);
            string result = decorator.Transform(input);

            Assert.AreEqual("<p>----Lorem     ipsum   dolor sit---</p>", result);
        }

        [TestMethod]
        public void Ex4()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new AddParagraph(new RemoveDashesAtEdges(component));
            string result = decorator.Transform(input);
            Assert.AreEqual("<p>Lorem     ipsum   dolor sit</p>", result);
        }

        [TestMethod]
        public void Ex5()
        {
            // Same as Ex4 but different order of transformations
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new RemoveDashesAtEdges(new AddParagraph(component));
            string result = decorator.Transform(input);
            Assert.AreEqual("<p>----Lorem     ipsum   dolor sit---</p>", result);
        }

        [TestMethod]
        public void Ex6()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var component = new ConcreteComponent();
            var decorator = new AddParagraph(new RemoveDoubleSpaces(new RemoveDashesAtEdges(component)));
            string result = decorator.Transform(input);

            Assert.AreEqual("<p>Lorem ipsum dolor sit</p>", result);
        }

        abstract class Component
        {
            public abstract string Transform(string s);
        }

        class ConcreteComponent : Component
        {
            public override string Transform(string s) => s;
        }

        abstract class Decorator : Component
        {
            protected Component _inner;
            public Decorator(Component inner)
            {
                _inner = inner;
            }

            public override string Transform(string s) => _inner.Transform(s);
        }

        // "Concrete decorator"
        class RemoveDashesAtEdges : Decorator
        {
            public RemoveDashesAtEdges(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => base.Transform(s).TrimEnd('-').TrimStart('-');
        }

        // "Concrete decorator"
        class RemoveDoubleSpaces : Decorator
        {
            public RemoveDoubleSpaces(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => Regex.Replace(base.Transform(s), " {2,}", " ");
        }

        // "Concrete decorator"
        class AddParagraph : Decorator
        {
            public AddParagraph(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => $"<p>{base.Transform(s)}</p>";
        }

    }
}
