/*
Låter dig ge en ersättning för ett annat objekt.

Proxyn kontrollerar tillgången till original-objektet. Du kan göra något innan eller efter anropet till objektet.

Proxyn har samma interface som service'n, så den kan ersätta det verkliga objektet

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Proxy
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Client client = new Client();

            Console.WriteLine("Client: Executing the client code with a real subject:");
            RealSubject realSubject = new RealSubject();
            client.ClientCode(realSubject);

            Console.WriteLine();

            Console.WriteLine("Client: Executing the same client code with a proxy:");
            Proxy proxy = new Proxy(realSubject);
            client.ClientCode(proxy);
        }

        interface ISubject
        {
            void Request();
        }

        class RealSubject : ISubject
        {
            public void Request()
            {
                Console.WriteLine("RealSubject: Handling Request.");
            }
        }

        // The Proxy has an interface identical to the RealSubject.
        class Proxy : ISubject
        {
            private RealSubject _realSubject;

            public Proxy(RealSubject realSubject)
            {
                _realSubject = realSubject;
            }

            /*
            De vanligaste användsfallen:
            - Lazy loading
            - Caching
            - Access control
            - Loggning
            */
            public void Request()
            {
                if (CheckAccess())           // Kollar access
                {
                    _realSubject.Request();  // Utför den verkliga operationen

                    LogAccess();             // Loggning
                }
            }

            public bool CheckAccess()
            {
                Console.WriteLine("Proxy: Checking access prior to firing a real request.");

                return true;
            }

            public void LogAccess()
            {
                Console.WriteLine("Proxy: Logging the time of request.");
            }
        }

        class Client
        {
            // Om du inte kan skapa interface (som här) så ärver du av klassen istället
            public void ClientCode(ISubject subject)
            {
                // ...

                subject.Request();

                // ...
            }
        }
    }
}
