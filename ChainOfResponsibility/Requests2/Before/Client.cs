namespace DesignPatterns.Template.Requests2.Before
{
    class Client : IClient
    {

        public void SetupChain()
        {
        }

        public Response Authenticate(Request request)
        {
            var response = new Response
            {
                Request = request,
                Authenticated=false
            };

            if (request.User.IsInRole("Administrator"))
                response.Authenticated = true;

            if (request.User.Name=="bobo")
                response.Authenticated = true;

            return response;
        }



    }
}

