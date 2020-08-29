using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Buttons
{
    interface IClient
    {
        IEnumerable<string> DoStuff();
    }
}