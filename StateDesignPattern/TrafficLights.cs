/*
 
 Own example with traffic lights

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{
    [TestClass]
    public class TrafficLights
    {
        static Queue<string> _log = new Queue<string>();

        [TestMethod]
        public void Ex1()
        {
            // "Client code"
            var light = new TrafficLight(new Red());
            Assert.AreEqual("Traffic is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state RedYellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Green", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚪",
                "⚫",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Yellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            Assert.AreEqual(0, _log.Count);
        }

        [TestMethod]
        public void Ex2()
        {
            var r = new Red();
            // r.Next();  // <---- kommer ge nullexception (helt ok)
        }

        // "Context"
        internal class TrafficLight
        {
            private State _state;

            internal TrafficLight(State state)
            {
                TransitionTo(state);
            }

            internal void Next() => _state.Next();
            internal string[] Render() => _state.Render();

            internal void TransitionTo(State state)
            {
                _state = state;
                _state.SetContext(this);
                _log.Enqueue($"Traffic is in state {_state.GetType().Name}");
            }
        }

        // "State"
        internal abstract class State
        {
            protected TrafficLight _light;

            internal abstract void Next();

            internal abstract string[] Render();

            internal void SetContext(TrafficLight light)
            {
                _light = light;
            }
        }

        internal class Red : State
        {
            internal override void Next()
            {
                _light.TransitionTo(new RedYellow());
            }

            internal override string[] Render() => new[] {
                "⚫",
                "⚪",
                "⚪",
            };
        }

        internal class RedYellow: State
        {
            internal override void Next()
            {
                _light.TransitionTo(new Green());
            }

            internal override string[] Render() => new[] {
                "⚫",
                "⚫",
                "⚪",
            };

        }

        internal class Green: State
        {
            internal override void Next()
            {
                _light.TransitionTo(new Yellow());
            }

            internal override string[] Render() => new[] {
                "⚪",
                "⚪",
                "⚫",
            };

        }

        internal class Yellow: State
        {
            internal override void Next()
            {
                _light.TransitionTo(new Red());
            }

            internal override string[] Render() => new[] {
                "⚪",
                "⚫",
                "⚪",
            };
        }
    }
}
