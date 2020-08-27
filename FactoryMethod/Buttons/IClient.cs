using System.Collections.Generic;

namespace DesignPatterns.Template.Buttons
{
    interface IClient
    {
        IEnumerable<string> DoStuff();
    }
}