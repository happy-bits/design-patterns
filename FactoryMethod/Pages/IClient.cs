namespace DesignPatterns.Template.Pages
{
    interface IClient
    {
        string[] RenderDashedPage(string header, string body);
        string[] RenderStarPage(string header, string body);
    }
}