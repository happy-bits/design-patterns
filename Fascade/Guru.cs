// "Göm ful kod bakom en fin tapet"

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Fascade
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Subsystem1 subsystem1 = new Subsystem1();
            Subsystem2 subsystem2 = new Subsystem2();
            Facade facade = new Facade(subsystem1, subsystem2);

            // Användning av fascaden:
            Console.Write(facade.Operation());

        }
    }

    // Facaden har hand om (och gömmer) några komplexa subsystem (ex dåligt skriven kod). O hanterar deras livscykel
    public class Facade
    {
        protected Subsystem1 _subsystem1;

        protected Subsystem2 _subsystem2;

        public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
        {
            _subsystem1 = subsystem1;
            _subsystem2 = subsystem2;
        }

        // Klienten jobbar med denna enkla metod (en nackdel är att klienten inte har tillgång till allt i subsystemen)
        public string Operation()
        {
            string result = "Facade initializes subsystems:\n";
            result += _subsystem1.Operation1();
            result += _subsystem2.Operation1();
            result += "Facade orders subsystems to perform the action:\n";
            result += _subsystem1.OperationN();
            result += _subsystem2.OperationZ();
            return result;
        }
    }

    public class Subsystem1
    {
        public string Operation1()
        {
            return "Subsystem1: Ready!\n";
        }

        public string OperationN()
        {
            return "Subsystem1: Go!\n";
        }
    }

    public class Subsystem2
    {
        public string Operation1()
        {
            return "Subsystem2: Get ready!\n";
        }

        public string OperationZ()
        {
            return "Subsystem2: Fire!\n";
        }
    }

}
