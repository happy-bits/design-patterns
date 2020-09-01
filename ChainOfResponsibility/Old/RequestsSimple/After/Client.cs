namespace DesignPatterns.Template.Requests.After
{
    class Client : IClient
    {
        private AdminRoleHandler _firstHandlerInChain;

        public void SetupChain()
        {
            var adminRoleHandler = new AdminRoleHandler();
            var boboHandler = new BoboHandler();

            adminRoleHandler.SetNext(boboHandler);

            _firstHandlerInChain = adminRoleHandler;
        }

        public Response Authenticate(Request request)
        {
            var response = _firstHandlerInChain.Handle(request);

            return response;
        }

        interface IHandler
        {
            IHandler SetNext(IHandler handler);

            Response Handle(Request request);
        }

        abstract class AbstractHandler : IHandler
        {
            private IHandler _nextHandler;

            public IHandler SetNext(IHandler handler)
            {
                _nextHandler = handler;
                return handler;
            }

            public virtual Response Handle(Request request)
            {
                if (_nextHandler != null)
                {
                    return _nextHandler.Handle(request);
                }
                else
                {
                    return new Response(false);
                }
            }
        }

        class AdminRoleHandler : AbstractHandler
        {
            public override Response Handle(Request request)
            {
                if (request.User.IsInRole("Administrator"))
                {
                    return new Response(true);
                }
                else
                {
                    return base.Handle(request);
                }
            }
        }

        class BoboHandler : AbstractHandler
        {
            public override Response Handle(Request request)
            {
                if (request.User.Name=="bobo")
                {
                    return new Response(true);
                }
                else
                {
                    return base.Handle(request);
                }
            }
        }


    }
}

