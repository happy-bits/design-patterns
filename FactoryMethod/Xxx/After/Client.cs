
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx.After
{
    class Client : IClient
    {
        public string[] RenderStarPage(string header, string body)
        {
            var creator = new StarPage();
            string[] result = creator.RenderPage(header, body);

            return result;
        }

        public string[] RenderDashedPage(string header, string body)
        {
            var creator = new DashedPage();
            string[] result = creator.RenderPage(header, body);
            return result;
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


    }
}
