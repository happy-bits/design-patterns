namespace DesignPatterns.Template.Requests2.After
{
    class Client : IClient
    {
        private AdminRoleHandler _firstHandlerInChain;

        public void SetupChain()
        {
            var adminRoleHandler = new AdminRoleHandler();
            var boboHandler = new BoboHandler();
            var setPageTypeHandler = new SetPageTypeHandler();
            var pageTypeAuthenticationHandler = new PageTypeAuthenticationHandler();

            adminRoleHandler
                .SetNext(boboHandler)
                .SetNext(setPageTypeHandler)
                .SetNext(pageTypeAuthenticationHandler);

            _firstHandlerInChain = adminRoleHandler;
        }

        public Response Authenticate(Request request)
        {
            var response = new Response();
            response.Request = request;

            _firstHandlerInChain.Handle(response);

            return response;
        }

        interface IHandler
        {
            IHandler SetNext(IHandler handler);

            Response Handle(Response request);
        }

        abstract class AbstractHandler : IHandler
        {
            private IHandler _nextHandler;

            public IHandler SetNext(IHandler handler)
            {
                _nextHandler = handler;
                return handler;
            }

            public virtual Response Handle(Response response)
            {
                if (_nextHandler != null)
                {
                    return _nextHandler.Handle(response);
                }
                else
                {
                    return response;
                }
            }
        }

        class AdminRoleHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.Request.User.IsInRole("Administrator"))
                {
                    response.Authenticated = true;
                }
                return base.Handle(response);
            }
        }

        class BoboHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.Request.User.Name == "bobo")
                {
                    response.Authenticated = true;
                }
                return base.Handle(response);
            }
        }

        class SetPageTypeHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.Request.PageId == 100)
                    response.PageType = PageType.Politics;

                return base.Handle(response);
            }
        }

        class PageTypeAuthenticationHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.PageType == PageType.Politics && response.Request.User.IsInRole("PoliticsEditor"))
                    response.Authenticated = true;

                return base.Handle(response);
            }
        }
    }
}

