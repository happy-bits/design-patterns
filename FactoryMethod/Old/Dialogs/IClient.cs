using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Dialogs
{
    interface IClient
    {
        IEnumerable<string> DoStuff();
    }
}