// (own example, sort of, part I)
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{

    [TestClass]
    public class Documents
    {
        static List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            // The client code.
            var context = new Document(new Draft());
            context.Publish();
            context.Publish();
            context.Publish();

            CollectionAssert.AreEqual(new[] {
                "Document created with state Draft",
                "Document is now in state Moderation",
                "Document is now in state Published",
                "Nothing happened"
            },  _log);
        }

        // "Context"
        class Document
        {
            // A reference to the current state of the Context.
            private State _state = null;

            public Document(State state)
            {
                _log.Add($"Document created with state {state.GetType().Name}");
                TransitionTo(state);
            }

            // The Context allows changing the State object at runtime.
            public void TransitionTo(State state)
            {
                _state = state;
                _state.SetContext(this);
            }

            // The Context delegates part of its behavior to the current State object.
            public void Publish()
            {
                _state.Publish();
            }

        }

        abstract class State
        {
            protected Document _context;

            public void SetContext(Document context)
            {
                _context = context;
            }

            public abstract void Publish();
        }

        class Draft : State
        {
            public override void Publish()
            {
                _context.TransitionTo(new Moderation());
                _log.Add($"Document is now in state Moderation");
            }
        }

        class Moderation : State
        {
            public override void Publish()
            {
                _context.TransitionTo(new Published());
                _log.Add($"Document is now in state Published");
            }
        }

        class Published : State
        {
            public override void Publish()
            {
                _log.Add("Nothing happened");
            }
        }


    }
}
