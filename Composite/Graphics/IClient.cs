using System.Collections.Generic;

namespace DesignPatterns.Composite.Graphics
{
    interface IClient
    {
        IEnumerable<string> DrawStuff();
    }
}