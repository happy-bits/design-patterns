using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.AbstractFactory.Xxx.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            {
                var factory = new RoundFactory();
                factory.Width = 15;
                var result = ClientMethod(factory, "header", "lorem ipsum dolor");
                CollectionAssert.AreEqual(new[] {
                    "╭─────────────╮",
                    "│ HEADER      │",
                    "╰─────────────╯",
                    "╭─────────────╮",
                    "│ lorem ipsum │",
                    "│ dolor       │",
                    "╰─────────────╯"
                }
                , result);
            }

            {
                var factory = new HtmlFactory();
                var result = ClientMethod(factory, "header", "lorem ipsum dolor");
                CollectionAssert.AreEqual(new[] {
                    "<h1>header</h1>",
                    "<p>lorem ispum dolor</p>"
                }
                , result);
            }

        }

        string[] ClientMethod(IPageFactory factory, string headerText, string introText)
        {
            var header = factory.CreateHeader(headerText);
            var intro = factory.CreateIntro(introText);

            string[] a = header.Render();
            string[] b = intro.Render();

            return a.ToList().Concat(b).ToArray();
        }

        abstract class IPageFactory
        {
            public abstract IHeader CreateHeader(string text);
            public abstract IIntro CreateIntro(string text);
            public int Width { get; set; }
        }

        /*

           Konkreta factories producerar en familj av produkter. En familj kan ha flera varianter, 
           men produkterna av en familj är inte kompatibla med produkter i andra familjer

           IAbstractProductA kan vara en stol och ConcreteProductA1 en viktoriansk stol

           "Viktoriansk fabrik"
        */
        class RoundFactory : IPageFactory
        {

            // "Viktoriansk stol"
            public override IHeader CreateHeader(string text)
            {
                return new RoundHeader();
            }

            // "Viktorianskt bord"
            public override IIntro CreateIntro(string text)
            {
                return new RoundIntro();
            }
        }

        /*
           En till fabrik (som t.ex tillverkar moderna möbler eller art-deco-möbler
           "Art-deco fabrik"
        */
        class HtmlFactory : IPageFactory
        {
            // "Art-deco stol"
            public override IHeader CreateHeader(string text)
            {
                return new HtmlHeader();
            }

            // "Art-deco bord"
            public override IIntro CreateIntro(string text)
            {
                return new HtmlIntro();
            }
        }

        // En förmåga som alla stolar har
        interface IHeader
        {
            string[] Render();
        }

        class RoundHeader : IHeader
        {
            public string[] Render()
            {
                throw new NotImplementedException();
            }
        }

        class HtmlHeader : IHeader
        {
            public string[] Render()
            {
                throw new NotImplementedException();
            }
        }

        // En förmåga som alla bord har (de kan också samarbeta med stolarna)
        interface IIntro
        {
            // Product B kan gör sin egen grej
            string[] Render();

            // ...men den kan också samarbeta med ProductA.

            string AnotherUsefulFunctionB(IHeader collaborator);
        }

        class RoundIntro : IIntro
        {
            public string[] Render()
            {
                throw new NotImplementedException();
            }

            public string AnotherUsefulFunctionB(IHeader collaborator)
            {
                var result = collaborator.Render();

                return $"The result of the B1 collaborating with the ({result})";
            }
        }

        class HtmlIntro : IIntro
        {
            public string[] Render()
            {
                throw new NotImplementedException();
            }

            public string AnotherUsefulFunctionB(IHeader collaborator)
            {
                var result = collaborator.Render();

                return $"The result of the B2 collaborating with the ({result})";
            }
        }
    }
}
