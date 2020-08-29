
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            {
                var creator = new StarPage();
                string[] result = creator.RenderPage("header", "body");
            }

            {
                var creator = new DashedPage();
                string[] result = creator.RenderPage("header", "body");
            }
        }

        class StarPage
        {
            public string[] RenderPage(string header, string body)
            {
                // nackdel: upprepning av kod (det enda som skiljer är vilken "produkt" som skapas)
                Box headerBox = new StarBox(header);

                Box bodyBox = new StarBox(body);

                var result = new List<string>();

                result.AddRange(headerBox.Render());
                result.Add("");
                result.Add("");
                result.AddRange(bodyBox.Render());

                return result.ToArray();
            }
        }

        class DashedPage
        {
            public string[] RenderPage(string header, string body)
            {
                Box headerBox = new DashedBox(header);

                Box bodyBox = new DashedBox(body);

                var result = new List<string>();

                result.AddRange(headerBox.Render());
                result.Add("");
                result.Add("");
                result.AddRange(bodyBox.Render());

                return result.ToArray();
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
                throw new NotImplementedException();
            }
        }

        class DashedBox : Box
        {
            public DashedBox(string text) : base(text)
            {
            }

            public override string[] Render()
            {
                throw new NotImplementedException();
            }
        }
    }
}
