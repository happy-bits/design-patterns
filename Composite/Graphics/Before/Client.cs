
using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite.Graphics.Before
{
    class Client : IClient
    {
        IEnumerable<string> IClient.DrawStuff()
        {
            throw new NotImplementedException();
        }
    }
}
