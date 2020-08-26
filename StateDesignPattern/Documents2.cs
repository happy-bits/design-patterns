/*
Own example, sort of, part II

When a Document get published it goes through these states:

    Draft => NeedsReview => Published

Document can always be send to Trash

A document can be reopened from any state. Then it will be in state Draft

*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{

    [TestClass]
    public class Documents2
    {
        static List<string> _log;
        
        public Documents2()
        {
            _log = new List<string>();
        }

        [TestMethod]
        public void Ex1()
        {
            // The client code.
            var document = new Document(new Draft(), "HeaderA", "ContentA");
            document.Publish();
            document.Publish();
            document.Publish();
            document.SendToTrash();
            document.Publish();
            document.ReOpen();

            Assert.AreEqual("HeaderA", document.Header);
            Assert.AreEqual("ContentA", document.Content);

            CollectionAssert.AreEqual(new[] {
                "A new document created",
                "Document is now in state Draft",
                "Document is now in state NeedsReview",
                "Document is now in state Published",
                "Document is already Published, nothing happens",
                "Document is now in state Trash",
                "Can't publish from Trash",
                "Document is now in state Draft",

            },  _log);
        }

        [TestMethod]
        public void Ex2()
        {
            // The client code.
            var document = new Document(new NeedsReview(), "HeaderB", "ContentB");
            document.SendToTrash();
            document.ReOpen();
            document.ReOpen();

            Assert.AreEqual("HeaderB", document.Header);
            Assert.AreEqual("ContentB", document.Content);

            CollectionAssert.AreEqual(new[] {
                "A new document created",
                "Document is now in state NeedsReview",
                "Document is now in state Trash",
                "Document is now in state Draft",
                "Document is already in Draft, nothing happens",
            }, _log);
        }

        // "Context"
        class Document
        {
            private State _state = null;

            public string Header { get; }
            public string Content { get; }

            public Document(State state, string header, string content)
            {
                _log.Add($"A new document created");
                TransitionTo(state);
                Header = header;
                Content = content;
            }

            public void TransitionTo(State state)
            {
                _state = state;
                _state.SetContext(this);
                _log.Add($"Document is now in state {_state.GetType().Name}");
            }

            public void SendToTrash()
            {
                _state.SendToTrash();
            }

            public void Publish()
            {
                _state.Publish();
            }

            public void ReOpen()
            {
                _state.ReOpen();
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

            public void SendToTrash()
            {
                if (this is Trash)
                {
                    _log.Add("Document is already in Trash, nothing happens");
                    return;
                } 
                _context.TransitionTo(new Trash());
            }

            public void ReOpen()
            {
                if (this is Draft)
                {
                    _log.Add("Document is already in Draft, nothing happens");
                    return;
                }
                _context.TransitionTo(new Draft());
            }
        }

        class Draft : State
        {
            public override void Publish()
            {
                _context.TransitionTo(new NeedsReview());
            }
        }

        class NeedsReview : State
        {
            public override void Publish()
            {
                _context.TransitionTo(new Published());
            }
        }

        class Published : State
        {
            public override void Publish()
            {
                _log.Add("Document is already Published, nothing happens");
            }
        }

        class Trash : State
        {
            public override void Publish()
            {
                _log.Add("Can't publish from Trash");
            }
        }
    }
}
