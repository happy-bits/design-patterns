
namespace DesignPatterns.TemplateMethod.Distances
{
    interface IClient
    {
        Result License(string licensnumber);
        Result Product(string productname);
    }
}