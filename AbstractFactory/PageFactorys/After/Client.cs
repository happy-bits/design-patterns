using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.AbstractFactory.PageFactorys.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            {
                var factory = new RoundFactory();
                var result = ClientMethod(factory, "header", "lorem ipsum dolor");
                CollectionAssert.AreEqual(new[] {
                    "╭────────╮",
                    "│ HEADER │",
                    "╰────────╯",
                    "╭───────────────────╮",
                    "│ lorem ipsum dolor │",
                    "╰───────────────────╯"
                }
                , result);

            }

            {
                var factory = new HtmlFactory();
                var result = ClientMethod(factory, "header", "lorem ipsum dolor");
                CollectionAssert.AreEqual(new[] {
                    "<h1>header</h1>",
                    "<p>lorem ipsum dolor</p>"
                }
                , result);
            }

        }

        string[] ClientMethod(PageFactory factory, string headerText, string introText)
        {
            var header = factory.CreateHeader(headerText);
            var intro = factory.CreateIntro(introText);

            string[] a = header.Render();
            string[] b = intro.Render();

            return a.ToList().Concat(b).ToArray();
        }

        abstract class PageFactory
        {
            public abstract Header CreateHeader(string text);
            public abstract Intro CreateIntro(string text);
        }

        class RoundFactory : PageFactory
        {
            public override Header CreateHeader(string text)
            {
                return new RoundHeader(text);
            }
            public override Intro CreateIntro(string text)
            {
                return new RoundIntro(text);
            }
        }

        class HtmlFactory : PageFactory
        {
            public override Header CreateHeader(string text)
            {
                return new HtmlHeader(text);
            }

            public override Intro CreateIntro(string text)
            {
                return new HtmlIntro(text);
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
                var lines = new string('─', Text.Length + 2);
                var result = new List<string>
                {
                    $"╭{lines}╮",
                    $"│ {Text} │",
                    $"╰{lines}╯",
                };

                return result.ToArray();
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
}
