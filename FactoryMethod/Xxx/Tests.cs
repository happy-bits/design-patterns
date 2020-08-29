using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Template.Xxx
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new After.Client(), 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var result = client.RenderStarPage("header", "body");

                CollectionAssert.AreEqual(new[] { 
                    "**********",
                    "* header *",
                    "**********",
                    "",
                    "",
                    "********",
                    "* body *",
                    "********",
                }

                , result);
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {
                var result = client.RenderDashedPage("header", "body");

                CollectionAssert.AreEqual(new[] {
                    "----------",
                    "- header -",
                    "----------",
                    "",
                    "",
                    "--------",
                    "- body -",
                    "--------",
                }

                , result);
            }
        }
    }

    abstract class Box
    {
        public Box(string text) => Text = text;

        public string Text { get; }

        public abstract string[] Render();
    }

    class StarBox : Box
    {
        public StarBox(string text) : base(text)
        {
        }

        public override string[] Render()
        {
            int nrOfStars = Text.Length + 4;
            var result = new List<string>
            {
                new string('*', nrOfStars),
                $"* {Text} *",
                new string('*', nrOfStars)
            };

            return result.ToArray();
        }
    }

    class DashedBox : Box
    {
        public DashedBox(string text) : base(text)
        {
        }

        public override string[] Render()
        {
            int nrOfStars = Text.Length + 4;
            var result = new List<string>
            {
                new string('-', nrOfStars),
                $"- {Text} -",
                new string('-', nrOfStars)
            };

            return result.ToArray();
        }
    }
}
