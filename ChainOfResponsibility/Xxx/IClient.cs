using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx
{
    interface IClient
    {
        Response Authenticate(Request request);
        void SetupChain();
    }
}