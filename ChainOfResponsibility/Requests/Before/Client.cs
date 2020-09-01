using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Requests.Before
{
    class Client : IClient
    {
        public Response Authenticate(Request request)
        {
            throw new NotImplementedException();
        }

        public void SetupChain()
        {
            throw new NotImplementedException();
        }
    }
}
