using System;

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

        abstract class AbstractHandler 
        {
            private AbstractHandler _nextHandler;

            public AbstractHandler SetNext(AbstractHandler handler)
            {
                _nextHandler = handler;
                return handler;
            }

            public Response Next(Response response)
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

            public abstract Response Handle(Response request);
        }

        class AdminRoleHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.Request.User.IsInRole("Administrator"))
                {
                    response.Authenticated = true;
                }
                return Next(response);
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
                return Next(response);
            }
        }

        class SetPageTypeHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.Request.PageId == 100)
                    response.PageType = PageType.Politics;

                return Next(response);
            }
        }

        class PageTypeAuthenticationHandler : AbstractHandler
        {
            public override Response Handle(Response response)
            {
                if (response.PageType == PageType.Politics && response.Request.User.IsInRole("PoliticsEditor"))
                    response.Authenticated = true;

                return Next(response);
            }
        }
    }
}

