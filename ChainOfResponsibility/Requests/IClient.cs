namespace DesignPatterns.ChainOfResponsibility.Requests
{
    interface IClient
    {
        Response GetPage(Request request);
        void SetupChain();
    }
}