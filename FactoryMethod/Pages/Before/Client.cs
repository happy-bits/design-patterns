
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Pages.Before
{
    class Client : IClient
    {
        public string[] RenderStarPage(string header, string body)
        {
            var page = new Page();
            // Nackdel: risk att skicka in någon typ (ex star eller dashed) som inte är tillåten 
            string[] result = page.Render("star", header, body);

            return result;
        }

        public string[] RenderDashedPage(string header, string body)
        {
            var page = new Page();
            string[] result = page.Render("dashed", header, body);
            return result;
        }

        class Page
        {
            public string[] Render(string type, string header, string body)
            {
                Box headerBox = GetBox(type, header);

                Box bodyBox = GetBox(type, body);

                var result = new List<string>();

                result.AddRange(headerBox.Render());
                result.Add("");
                result.Add("");
                result.AddRange(bodyBox.Render());

                return result.ToArray();
            }

            // Nackdel: när nya boxar kommer ut måste denna uppdateras
            private static Box GetBox(string type, string header) =>

                type switch
                {
                    "star" => new StarBox(header),
                    "dashed" => new DashedBox(header),
                    _ => throw new ArgumentException()
                };

        }

    }
}
