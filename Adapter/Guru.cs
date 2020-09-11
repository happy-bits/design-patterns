/*

"Tillåt ett objekt som inte passar att användas med extern kod"

Tillåter objekt med inkompatibla interface att samarbeta

Adapterns fångar anrop från ett objekt och transformerar den till ett format som andra objektet känner till

Används ofta i system som är baserad på gammal kod.

Adaptern är en mellanliggande klass som översätter mellan din kod och tredjepartskod (eller nån gammal kod du inte vill röra)

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Adapter
{
    [TestClass]

    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Adaptee adaptee = new Adaptee();
            ITarget target = new Adapter(adaptee);

            Console.WriteLine("Adaptee interface is incompatible with the client.");
            Console.WriteLine("But with adapter client can call it's method.");

            // Vi låtsas att klienten inte kan anropa "adaptee" direkt
            // ... utan måste gå via "target"

            Console.WriteLine(target.GetRequest());
        }

        // Domän-specifikt interface som används av klientkoden
        interface ITarget
        {
            string GetRequest();
        }

        // "Adaptee" behöver anpassning innan client-koden kan använda den
        class Adaptee
        {
            public string GetSpecificRequest()
            {
                return "Specific request.";
            }
        }

        // "Adapter" gör "Adaptee" kompatibelt med "Targets" interface

        class Adapter : ITarget
        {
            private readonly Adaptee _adaptee;

            public Adapter(Adaptee adaptee)
            {
                _adaptee = adaptee;
            }

            public string GetRequest()
            {
                return $"This is '{_adaptee.GetSpecificRequest()}'";
            }
        }
    }
}
