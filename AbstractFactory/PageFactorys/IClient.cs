
namespace DesignPatterns.AbstractFactory.PageFactorys
{
    interface IClient
    {
        string[] RenderPage(string header, string intro);
        void SetFactory(string factoryname);
    }
}