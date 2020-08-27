using System.Collections.Generic;

namespace DesignPatterns.Template.Dialogs
{
    interface IClient
    {
        IEnumerable<string> DoStuff();
    }
}