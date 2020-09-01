using System.Collections.Generic;

namespace DesignPatterns.Template.Requests2
{
    interface IClient
    {
        Response Authenticate(Request request);
        void SetupChain();
    }
}