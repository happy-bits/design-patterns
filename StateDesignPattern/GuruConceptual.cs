using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{
    /*
     Ett Context har ett (abstrakt) State
     Ett (konkret) state kan ändra context'ets state (i runtime)

     Varje State har ett antal metoder med samma signatur (handlers)
         
         */
    [TestClass]
    public class GuruConceptual
    {
        static List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            // The client code.
            var context = new Context(new ConcreteStateA());
            context.Request1();
            context.Request2();

            CollectionAssert.AreEqual(new[] {
                "Context: Transition to ConcreteStateA.",
                "ConcreteStateA handles request1.",
                "ConcreteStateA wants to change the state of the context.",
                "Context: Transition to ConcreteStateB.",
                "ConcreteStateB handles request2.",
                "ConcreteStateB wants to change the state of the context.",
                "Context: Transition to ConcreteStateA."
            },  _log);
        }

        // The Context defines the interface of interest to clients. It also
        // maintains a reference to an instance of a State subclass, which
        // represents the current state of the Context.
        class Context
        {
            // A reference to the current state of the Context.
            private State _state = null;

            public Context(State state)
            {
                TransitionTo(state);
            }

            // The Context allows changing the State object at runtime.
            public void TransitionTo(State state)
            {
                _log.Add($"Context: Transition to {state.GetType().Name}.");
                _state = state;
                _state.SetContext(this);
            }

            // The Context delegates part of its behavior to the current State
            // object.
            public void Request1()
            {
                _state.Handle1();
            }

            public void Request2()
            {
                _state.Handle2();
            }
        }

        // The base State class declares methods that all Concrete State should
        // implement and also provides a backreference to the Context object,
        // associated with the State. This backreference can be used by States to
        // transition the Context to another State.
        abstract class State
        {
            protected Context _context;

            public void SetContext(Context context)
            {
                _context = context;
            }

            public abstract void Handle1();

            public abstract void Handle2();
        }

        // Concrete States implement various behaviors, associated with a state of
        // the Context.
        class ConcreteStateA : State
        {
            public override void Handle1()
            {
                _log.Add("ConcreteStateA handles request1.");
                _log.Add("ConcreteStateA wants to change the state of the context.");
                _context.TransitionTo(new ConcreteStateB());
            }

            public override void Handle2()
            {
                _log.Add("ConcreteStateA handles request2.");
            }
        }

        class ConcreteStateB : State
        {
            public override void Handle1()
            {
                _log.Add("ConcreteStateB handles request1.");
            }

            public override void Handle2()
            {
                _log.Add("ConcreteStateB handles request2.");
                _log.Add("ConcreteStateB wants to change the state of the context.");
                _context.TransitionTo(new ConcreteStateA());
            }
        }


    }
}
