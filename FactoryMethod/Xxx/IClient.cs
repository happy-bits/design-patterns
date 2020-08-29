using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx
{
    interface IClient
    {
        string[] RenderDashedPage(string header, string body);
        string[] RenderStarPage(string header, string body);
    }
}