using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.AbstractFactory.PageFactorys
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
                {
                    client.SetFactory("Round");
                    var result = client.RenderPage("Header", "Lorem ipsum dolor");

                    CollectionAssert.AreEqual(new[] {
                        "╭────────╮",
                        "│ HEADER │",
                        "╰────────╯",
                        "Lorem ipsum dolor",
                    }
                    , result);
                }
                {
                    client.SetFactory("Html");
                    var result = client.RenderPage("Header", "Lorem ipsum dolor");

                    CollectionAssert.AreEqual(new[] {
                        "<h1>Header</h1>",
                        "<p>Lorem ipsum dolor</p>"
                    }
                    , result);
                }

                client.SetFactory("Html");
                var result2 = client.RenderPage("Header", "Lorem ipsum dolor");
            }
        }

    }

    abstract class Header
    {
        public Header(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public abstract string[] Render();
    }

    class RoundHeader : Header
    {
        public RoundHeader(string text) : base(text)
        {
        }
        public override string[] Render()
        {
            var lines = new string('─', Text.Length + 2);
            var result = new List<string>
                {
                    $"╭{lines}╮",
                    $"│ {Text.ToUpper()} │",
                    $"╰{lines}╯",
                };

            return result.ToArray();
        }
    }

    class HtmlHeader : Header
    {
        public HtmlHeader(string text) : base(text)
        {
        }

        public override string[] Render()
        {
            return new[] { $"<h1>{Text}</h1>" };
        }
    }

    abstract class Intro
    {
        public Intro(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public abstract string[] Render();
    }

    class RoundIntro : Intro
    {
        public RoundIntro(string text) : base(text)
        {
        }

        public override string[] Render()
        {
            return new[] { Text };
        }
    }

    class HtmlIntro : Intro
    {
        public HtmlIntro(string text) : base(text)
        {
        }

        public override string[] Render()
        {
            return new[] { $"<p>{Text}</p>" };
        }
    }
}
