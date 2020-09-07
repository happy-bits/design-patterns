using System;
using System.Collections.Generic;

namespace DesignPatterns.Adapter.Pegs.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            throw new NotImplementedException();
        }
    }
}
