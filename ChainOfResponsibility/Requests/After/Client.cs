/*
 Fördel: 
 - de olika enskilda delarna i logiken kan testas
 - lätt att ta bort eller byta ordning på handlers (eller dynamiskt skjuta in något i mitten)

 Nackdel:
 - Betydligt mer kod (2.5ggr mer) och fler klasser. Kan bli svårare att läsa
 
 */
namespace DesignPatterns.ChainOfResponsibility.Requests.After
{
    class Client : IClient
    {
        private AdminGetFullAccess _firstHandlerInChain;

        public void SetupChain()
        {
            var adminRoleHandler = new AdminGetFullAccess();
            var demandEditorRole = new DemandEditorRole();
            var loadPage = new LoadPage();
            var correctRoleGivesAccess = new CorrectRoleGivesAccess();

            adminRoleHandler
                .SetNext(demandEditorRole)
                .SetNext(loadPage)
                .SetNext(correctRoleGivesAccess);

            _firstHandlerInChain = adminRoleHandler;
        }

        public Response GetPage(Request request)
        {
            var response = new Response();
            _firstHandlerInChain.Handle(request, response);

            return response;
        }
    }

    abstract class AbstractHandler
    {
        private AbstractHandler _nextHandler;
        protected readonly PageRepository _pageRepository = new PageRepository();

        public AbstractHandler SetNext(AbstractHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public (Request, Response) Next(Request request, Response response)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(request, response);
            }
            else
            {
                return (request, response);
            }
        }

        public abstract (Request, Response) Handle(Request request, Response response);
    }

    class AdminGetFullAccess : AbstractHandler
    {
        public override (Request, Response) Handle(Request request, Response response)
        {
            if (request.User.IsInRole("Administrator") || request.User.Name == "bobo")
            {
                response.Authorized = true;
                response.Page = _pageRepository.GetPageById(request.PageId);
                return (request, response);
            }

            return Next(request, response);
        }
    }

    class DemandEditorRole : AbstractHandler
    {
        public override (Request, Response) Handle(Request request, Response response)
        {
            if (!request.User.IsInRole("Editor"))
            {
                response.Authorized = false;
                return (request, response);
            }
            return Next(request, response);
        }
    }

    class LoadPage : AbstractHandler
    {
        public override (Request, Response) Handle(Request request, Response response)
        {
            response.Page = _pageRepository.GetPageById(request.PageId);

            return Next(request, response);
        }
    }

    class CorrectRoleGivesAccess : AbstractHandler
    {
        public override (Request, Response) Handle(Request request, Response response)
        {
            if (response.Page.PageType == PageType.Politics && request.User.IsInRole("PoliticsEditor"))
            {
                response.Authorized = true;
            }
            else
            {
                response.Authorized = false;
                return (request, response);
            }

            return Next(request, response);
        }
    }
}

