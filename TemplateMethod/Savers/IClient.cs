
namespace DesignPatterns.TemplateMethod.Savers
{
    interface IClient
    {
        Result License(string licensnumber);
        Result Product(string productname);
    }
}