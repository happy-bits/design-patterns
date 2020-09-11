/*
Own example

Small adjustment of Transform

Good: now we don't need to do "new ConcreteComponent" in client code
Bad: we depend on ConcreteComponent
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace DesignPatterns.Decorator
{
    [TestClass]
    public class Transforms2
    {
        [TestMethod]
        public void Ex1()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new RemoveDashesAtEdges();
            string result = decorator.Transform(input);

            Assert.AreEqual("Lorem     ipsum   dolor sit", result);
        }

        [TestMethod]
        public void Ex2()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new RemoveDoubleSpaces();
            string result = decorator.Transform(input);

            Assert.AreEqual("----Lorem ipsum dolor sit---", result);
        }

        [TestMethod]
        public void Ex3()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new AddParagraph();
            string result = decorator.Transform(input);

            Assert.AreEqual("<p>----Lorem     ipsum   dolor sit---</p>", result);
        }

        [TestMethod]
        public void Ex4()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new AddParagraph(new RemoveDashesAtEdges());
            string result = decorator.Transform(input);
            Assert.AreEqual("<p>Lorem     ipsum   dolor sit</p>", result);
        }

        [TestMethod]
        public void Ex5()
        {
            // Same as Ex4 but different order of transformations
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new RemoveDashesAtEdges(new AddParagraph());
            string result = decorator.Transform(input);
            Assert.AreEqual("<p>----Lorem     ipsum   dolor sit---</p>", result);
        }

        [TestMethod]
        public void Ex6()
        {
            string input = "----Lorem     ipsum   dolor sit---";
            var decorator = new AddParagraph(new RemoveDoubleSpaces(new RemoveDashesAtEdges()));
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
            public Decorator()
            {
                _inner = new ConcreteComponent();
            }
            public Decorator(Component inner)
            {
                _inner = inner;
            }

            public override string Transform(string s) => _inner.Transform(s);
        }

        // "Concrete decorator"
        class RemoveDashesAtEdges : Decorator
        {
            public RemoveDashesAtEdges()
            {
            }
            public RemoveDashesAtEdges(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => base.Transform(s).TrimEnd('-').TrimStart('-');
        }

        // "Concrete decorator"
        class RemoveDoubleSpaces : Decorator
        {
            public RemoveDoubleSpaces()
            {
            }
            public RemoveDoubleSpaces(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => Regex.Replace(base.Transform(s), " {2,}", " ");
        }

        // "Concrete decorator"
        class AddParagraph : Decorator
        {
            public AddParagraph()
            {
            }
            public AddParagraph(Component inner) : base(inner)
            {
            }

            public override string Transform(string s) => $"<p>{base.Transform(s)}</p>";
        }

    }
}
