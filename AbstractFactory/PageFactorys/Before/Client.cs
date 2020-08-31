using System;

namespace DesignPatterns.AbstractFactory.PageFactorys.Before
{
    class Client : IClient
    {
        public string[] RenderPage(string header, string intro)
        {
            throw new NotImplementedException();
        }

        public void SetFactory(string factoryname)
        {
            throw new NotImplementedException();
        }
    }
}
