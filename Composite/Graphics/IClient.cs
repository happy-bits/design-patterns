using System.Collections.Generic;

namespace DesignPatterns.Composite.Graphics
{
    interface IClient
    {
        Queue<string> DrawStuff();
    }
}