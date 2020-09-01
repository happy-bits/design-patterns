namespace DesignPatterns.Template.Requests.Before
{
    class Client : IClient
    {

        public void SetupChain()
        {
        }

        public Response Authenticate(Request request)
        {
            if (request.User.IsInRole("Administrator"))
                return new Response(true);

            if (request.User.Name=="bobo")
                return new Response(true);

            return new Response(false);
        }



    }
}

