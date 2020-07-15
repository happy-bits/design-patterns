﻿/*
From https://refactoring.guru/design-patterns/decorator/csharp/example

Client jobbar med en Component. I detta fall har komponenten bara en metod (Operation)

Flera Decorator's kan omsluta varandra och tillslut en Component.
När Client kör Operation på den yttersta Decoratorn så utförs alla (som en rysk docka)
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Decorator
{
    [TestClass]
    public class GuruConceptual
    {
        static readonly Queue<string> _log = new Queue<string>();

        [TestMethod]
        public void Ex1()
        {
            Client client = new Client();

            var simple = new ConcreteComponent();

            // Client: I get a simple component
            client.ClientCode(simple);
            Assert.AreEqual("RESULT: ConcreteComponent", _log.Dequeue());

            // ...as well as decorated ones.
            //
            // Note how decorators can wrap not only simple components but the
            // other decorators as well.
            ConcreteDecoratorA decorator1 = new ConcreteDecoratorA(simple);
            ConcreteDecoratorB decorator2 = new ConcreteDecoratorB(decorator1);

            // Client: Now I've got a decorated component
            client.ClientCode(decorator2);
            Assert.AreEqual("RESULT: ConcreteDecoratorB(ConcreteDecoratorA(ConcreteComponent))", _log.Dequeue());

            Assert.AreEqual(0, _log.Count);

        }

        // The base Component interface defines operations that can be altered by
        // decorators.
        public abstract class Component
        {
            public abstract string Operation();
        }

        // Concrete Components provide default implementations of the operations.
        // There might be several variations of these classes.
        class ConcreteComponent : Component
        {
            public override string Operation()
            {
                return "ConcreteComponent";
            }
        }

        // The base Decorator class follows the same interface as the other
        // components. The primary purpose of this class is to define the wrapping
        // interface for all concrete decorators. The default implementation of the
        // wrapping code might include a field for storing a wrapped component and
        // the means to initialize it.
        abstract class Decorator : Component
        {
            protected Component _component;

            public Decorator(Component component)
            {
                _component = component;
            }

            // (denna behövs inte)
            public void SetComponent(Component component)
            {
                _component = component;
            }

            // The Decorator delegates all work to the wrapped component.
            public override string Operation()
            {
                if (_component != null)
                {
                    return _component.Operation();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        // Concrete Decorators call the wrapped object and alter its result in some
        // way.
        class ConcreteDecoratorA : Decorator
        {
            public ConcreteDecoratorA(Component comp) : base(comp)
            {
            }

            // Decorators may call parent implementation of the operation, instead
            // of calling the wrapped object directly. This approach simplifies
            // extension of decorator classes.
            public override string Operation()
            {
                return $"ConcreteDecoratorA({base.Operation()})";
            }
        }

        // Decorators can execute their behavior either before or after the call to
        // a wrapped object.
        class ConcreteDecoratorB : Decorator
        {
            public ConcreteDecoratorB(Component comp) : base(comp)
            {
            }

            public override string Operation()
            {
                return $"ConcreteDecoratorB({base.Operation()})";
            }
        }

        public class Client
        {
            // The client code works with all objects using the Component interface.
            // This way it can stay independent of the concrete classes of
            // components it works with.
            public void ClientCode(Component component)
            {
                _log.Enqueue("RESULT: " + component.Operation());
            }
        }

    }
}