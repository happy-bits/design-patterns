using System;
using System.Linq;

namespace DesignPatterns.AbstractFactory.PageFactorys.After
{
    class Client : IClient
    {
        PageFactory _factory;

        public void SetFactory(string factoryname)
        {
            _factory = factoryname switch
            {
                "Round" => new RoundFactory(),
                "Html" => new HtmlFactory(),
                _ => throw new Exception()
            };
        }

        public string[] RenderPage(string headerText, string introText)
        {
            var header = _factory.CreateHeader(headerText);
            var intro = _factory.CreateIntro(introText);

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
    }
}
