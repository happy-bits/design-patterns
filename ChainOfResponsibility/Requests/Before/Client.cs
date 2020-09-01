namespace DesignPatterns.ChainOfResponsibility.Requests.Before
{
    class Client : IClient
    {
        PageRepository _pageRepository = new PageRepository();

        public void SetupChain()
        {
        }

        public Response GetPage(Request request)
        {
            var response = new Response();

            // Administrator and user "bobo" should have full access

            if (request.User.IsInRole("Administrator") || request.User.Name == "bobo")
            {
                response.Authorized = true;
                response.Page = _pageRepository.GetPageById(request.PageId);
                return response;
            }

            // Is user is not admin + don't belong to Editor => not authenticated

            if (!request.User.IsInRole("Editor"))
            {
                response.Authorized = false;
                return response;
            }

            // Load page, maybe the user have access

            response.Page = _pageRepository.GetPageById(request.PageId);

            // If user belongs to the right type of editor => access

            if (response.Page.PageType == PageType.Politics && request.User.IsInRole("PoliticsEditor"))
            {
                response.Authorized = true;
            }
            else
            {
                response.Authorized = false;
                return response;
            }

            return response;
        }



        // Nedan funkar, men lösningen ovan är "plattare" och kanske enklare att följa

        public Response Authenticate_Old(Request request)
        {
            var response = new Response();

            if (request.User.IsInRole("Administrator") || request.User.Name == "bobo")
            {
                response.Authorized = true;
                response.Page = _pageRepository.GetPageById(request.PageId);
            }
            else
            {

                if (!request.User.IsInRole("Editor"))
                {
                    response.Authorized = false;
                    return response;
                }

                response.Page = _pageRepository.GetPageById(request.PageId);

                if (response.Page.PageType == PageType.Politics && request.User.IsInRole("PoliticsEditor"))
                {
                    response.Authorized = true;
                }
                else
                {
                    response.Authorized = false;
                    return response;
                }
            }

            return response;
        }
    }
}

