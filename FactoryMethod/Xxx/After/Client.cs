
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx.After
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

        abstract class Page
        {
            public string[] RenderPage(string header, string body)
            {
                Box headerBox = CreateBox(header);

                Box bodyBox = CreateBox(body);

                var result = new List<string>();

                result.AddRange(headerBox.Render());
                result.Add("");
                result.Add("");
                result.AddRange(bodyBox.Render());

                return result.ToArray();
            }

            protected abstract Box CreateBox(string text);
        }

        class StarPage : Page
        {
            protected override Box CreateBox(string text)
            {
                return new StarBox(text);
            }
        }


        class DashedPage : Page
        {
            protected override Box CreateBox(string text)
            {
                return new DashedBox(text);
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
