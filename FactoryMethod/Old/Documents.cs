/*
 
FACTORY METHOD - (own example)

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Documents
    {

        /*
         
        I would probably use a Theory here and pass in the different kinds of creators (html, markdown) as separate [InlineData] calls.
        If this is meant to be a demo of Factory Method I worry that the Element types distract from that and could perhaps just be replaced with strings. They seem to work well but they're not needed to demonstrate the factory concept.
             */
        [TestMethod]
        public void Ex1()
        {
            var c = new Client();

            var result = c.ClientCode("html");

            CollectionAssert.AreEqual(new[] {
                "<h1>HeaderA</h1>\n<body>BodyA</body>",
                "<h1>HeaderB</h1>\n<body>BodyB</body>",
                "<body>BodyC</body>\n<h1>HeaderC</h1>",
            }, result);
        }

        [TestMethod]
        public void Ex2()
        {
            var c = new Client();

            var result = c.ClientCode("markdown");

            CollectionAssert.AreEqual(new[] {
                "# HeaderA\nBodyA",
                "# HeaderB\nBodyB",
                "BodyC\n# HeaderC",
            }, result);
        }

        // "Creator"
        abstract class DocumentCreator
        {
            public bool Ascending { get; set; } = true;   // extra egenskaper
            public string Delimiter { get; set; } = "\n"; // extra egenskaper

            protected abstract IEnumerable<Element> Create(string header, string body);

            internal string Render(string header, string body) // extra logik
            {
                var elements = Create(header, body);

                if (!Ascending)
                    elements = elements.Reverse();

                return string.Join(Delimiter, elements.Select(e => e.Render()));
            }

        }

        // "Concrete Creator" 
        class HtmlCreator : DocumentCreator
        {
            protected override IEnumerable<Element> Create(string header, string body)
            {
                yield return new HtmlHeader(header);
                yield return new HtmlBody(body);
            }
        }

        // "Concrete Creator" 
        class MarkdownCreator : DocumentCreator
        {
            protected override IEnumerable<Element> Create(string header, string body)
            {
                yield return new MarkdownHeader(header);
                yield return new MarkdownBody(body);
            }
        }

        // "Product"
        abstract class Element
        {
            public Element(string content) => Content = content;
            public string Content { get; }
            public abstract string Render();
        }

        // "Concrete Product"
        class HtmlHeader : Element
        {
            public HtmlHeader(string content) : base(content) { }
            public override string Render() => $"<h1>{Content}</h1>";
        }

        // "Concrete Product"
        class HtmlBody : Element
        {
            public HtmlBody(string content) : base(content) { }
            public override string Render() => $"<body>{Content}</body>";
        }

        // "Concrete Product"
        class MarkdownHeader : Element
        {
            public MarkdownHeader(string content) : base(content) { }
            public override string Render() => $"# {Content}";
        }

        // "Concrete Product"
        class MarkdownBody : Element
        {
            public MarkdownBody(string content) : base(content) { }
            public override string Render() => Content;
        }

        // "Client"
        class Client
        {
            public List<string> ClientCode(string type)
            {
                DocumentCreator creator = GetCreator(type);

                List<string> result = new List<string>();

                string result1 = creator.Render("HeaderA", "BodyA");
                string result2 = creator.Render("HeaderB", "BodyB");

                creator.Ascending = false;
                string result3 = creator.Render("HeaderC", "BodyC");

                result.Add(result1);
                result.Add(result2);
                result.Add(result3);

                return result;
            }

            private static DocumentCreator GetCreator(string type) => type switch
            {
                "markdown" => new MarkdownCreator(),
                "html" => new HtmlCreator(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
