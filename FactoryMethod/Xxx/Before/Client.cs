using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx.Before
{
    class Client : IClient
    {
        IEnumerable<string> IClient.DoStuff()
        {
            throw new NotImplementedException();
        }
    }
}
