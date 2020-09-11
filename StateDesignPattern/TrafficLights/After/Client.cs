/*
 
 Own example with traffic lights

 Fördel:
 - Koden som rör t.ex Red är inom samma klass (texten "Rött" och Render)
 - Behöver inte throw new exception (som i förra exemplet)

 Nackdel
 - Mer kod, mer komplext
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.StateDesignPattern.TrafficLights.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var light = new TrafficLight(Material.LED, new Red());

            Assert.AreEqual("Rött", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Rött och gult", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Grönt", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚪",
                "⚫",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Gult", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Rött", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            Assert.AreEqual("LED", light.Material.ToString());
        }
    }



    //    var r = new Red();
    //    // r.Next();  // <---- kommer ge nullexception (helt ok)

    public enum Material
    {
        Halogen, LED
    }
    // "Context"
    public class TrafficLight
    {
        private State _state;

        public TrafficLight(Material material, State state)
        {
            TransitionTo(state);
            Material = material;
        }

        public Material Material { get; }

        public void Next() => _state.Next();
        public string[] Render() => _state.Render();

        public void TransitionTo(State state)
        {
            _state = state;
            _state.SetContext(this);
        }

        public string Describe => _state.Describe;

    }

    // "State"
    public abstract class State
    {
        protected TrafficLight _light;

        public abstract void Next();

        public abstract string[] Render();

        public abstract string Describe { get; }

        public void SetContext(TrafficLight light)
        {
            _light = light;
        }
    }

    public class Red : State
    {
        public override string Describe => "Rött";

        public override void Next()
        {
            _light.TransitionTo(new RedYellow());
        }

        public override string[] Render() => new[] {
                "⚫",
                "⚪",
                "⚪",
            };
    }

    public class RedYellow : State
    {
        public override string Describe => "Rött och gult";

        public override void Next()
        {
            _light.TransitionTo(new Green());
        }

        public override string[] Render() => new[] {
                "⚫",
                "⚫",
                "⚪",
            };

    }

    public class Green : State
    {
        public override string Describe => "Grönt";

        public override void Next()
        {
            _light.TransitionTo(new Yellow());
        }

        public override string[] Render() => new[] {
                "⚪",
                "⚪",
                "⚫",
            };

    }

    public class Yellow : State
    {
        public override string Describe => "Gult";

        public override void Next()
        {
            _light.TransitionTo(new Red());
        }

        public override string[] Render() => new[] {
                "⚪",
                "⚫",
                "⚪",
            };

    }
}
