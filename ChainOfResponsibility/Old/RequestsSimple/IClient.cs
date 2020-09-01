using System.Collections.Generic;

namespace DesignPatterns.Template.Requests
{
    interface IClient
    {
        Response Authenticate(Request request);
        void SetupChain();
    }
}