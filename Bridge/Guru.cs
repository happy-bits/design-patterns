/*

"Splitta en klass i två andra klasser"

https://refactoring.guru/design-patterns/bridge
 Dela en stor klass (eller flera nära relaterade klasser) till två separata hierarkier - abstraktion och implementation - som kan utvecklas oberoende av varandra

Den ena hierarkin (som ofta kallas Abstraction) får en referent till ett objekt i den andra hierarkin (Implementation)

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Bridge
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Client client = new Client();

            Abstraction abstraction;
            // Klientkoden jobbar här med en viss uppsättning av abstraktion-implementation-kombination
            abstraction = new Abstraction(new ConcreteImplementationA());
            client.ClientCode(abstraction);

            Console.WriteLine();

            abstraction = new ExtendedAbstraction(new ConcreteImplementationB());
            client.ClientCode(abstraction);
        }

    }

    /*

        Abstract: Base operation with:
        ConcreteImplementationA: The result in platform A.

        ExtendedAbstraction: Extended operation with:
        ConcreteImplementationA: The result in platform B.
    */

        // Abstraktionen definerare en "hög-nivå-operation" baserad på de primitiva operationerna
    class Abstraction
    {
        protected IImplementation _implementation;

        public Abstraction(IImplementation implementation)
        {
            _implementation = implementation;
        }

        public virtual string Operation()
        {
            // Det riktiga jobbet delegeras till implementationen
            return $"Abstract: Base operation with:\n{_implementation.OperationImplementation()}";
        }
    }

    // Här utökas Abstraction
    class ExtendedAbstraction : Abstraction
    {
        public ExtendedAbstraction(IImplementation implementation) : base(implementation)
        {
        }

        public override string Operation()
        {
            return $"ExtendedAbstraction: Extended operation with:\n{_implementation.OperationImplementation()}";
        }
    }

    public interface IImplementation
    {
        // Normalt en "primitiv operation"
        string OperationImplementation();
    }

    class ConcreteImplementationA : IImplementation
    {
        public string OperationImplementation()
        {
            return "ConcreteImplementationA: The result in platform A.\n";
        }
    }

    class ConcreteImplementationB : IImplementation
    {
        public string OperationImplementation()
        {
            return "ConcreteImplementationA: The result in platform B.\n";
        }
    }

    class Client
    {
        // Klientkoden ska bara vara beroende av Abstraction-klassen. 
        // Detta leder till att klienten kan klara av stödja vilken abstrakt-implementation-kombination som helst
        public void ClientCode(Abstraction abstraction)
        {
            Console.Write(abstraction.Operation());
        }
    }

}
