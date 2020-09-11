using System.Collections.Generic;

namespace DesignPatterns.Template.Pages.Before3
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

        abstract class Page
        {
            // Nackdel: okej lösning men känns lite onaturlig
            protected static string[] Render(Box headerBox, Box bodyBox)
            {
                var result = new List<string>();
                result.AddRange(headerBox.Render());
                result.Add("");
                result.Add("");
                result.AddRange(bodyBox.Render());
                return result.ToArray();
            }

            abstract public string[] Render(string header, string body);
        }

        class StarPage: Page
        {
            public override string[] Render(string header, string body)
            {
                // Nackdel: en viss upprepning (behöver ange att det är typen StarBox båda gångerna)
                Box headerBox = new StarBox(header);
                Box bodyBox = new StarBox(body);
                return Render(headerBox, bodyBox);
            }

        }

        class DashedPage: Page
        {
            public override string[] Render(string header, string body)
            {
                Box headerBox = new DashedBox(header);
                Box bodyBox = new DashedBox(body);
                return Render(headerBox, bodyBox);
            }

        }
    }
}
