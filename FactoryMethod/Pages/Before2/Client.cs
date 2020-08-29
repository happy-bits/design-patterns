
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Pages.Before2
{
    class Client : IClient
    {
        public string[] RenderStarPage(string header, string body)
        {
            var page = new StarPage();
            string[] result = page.Render(header, body);

            return result;
        }

        public string[] RenderDashedPage(string header, string body)
        {
            var page = new DashedPage();
            string[] result = page.Render(header, body);
            return result;
        }

        class StarPage
        {
            public string[] Render(string header, string body)
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
            public string[] Render(string header, string body)
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
    }
}
