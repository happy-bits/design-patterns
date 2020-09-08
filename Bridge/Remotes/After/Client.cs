
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Bridge.Remotes.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            {
                var tv = new TV();
                var remote = new BasicRemote(tv); // en kombination av abstraction-implementation
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(30, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
            }

            {
                var remote = new AdvancedRemote(new TV());
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.Mute();

                Assert.AreEqual(0, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
            }

            {
                var remote = new BasicRemote(new Radio());
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(20, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
            }
            {
                var remote = new AdvancedRemote(new Radio());
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(20, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
            }


        }
    }

    // Abstraction delegerar jobb till "_device"
    class BasicRemote
    {
        protected readonly Device _device;

        private int _volume;

        public BasicRemote(Device device) => _device = device;

        public bool IsEnabled { get; private set; }

        public void Disable() => IsEnabled = false;

        public void Enable() => IsEnabled = true;

        public int GetVolume() => _volume;

        public void SetVolume(int percentage) => _volume = percentage;

        public void TogglePower()
        {
            if (IsEnabled)
                Disable();

            Enable();
        }

        public void VolumeUp()
        {
            var old = GetVolume();
            SetVolume(old + 10);
        }
        public void VolumeDown()
        {
            var old = GetVolume();
            SetVolume(old - 10);
        }

        public List<double> Batteries { get; protected set; } = new List<double> { 30 };

        public double RemainingBattery => Batteries.Sum();

        public string DeviceName => _device.Name;
    }

    class AdvancedRemote : BasicRemote
    {
        public AdvancedRemote(Device device) : base(device)
        {
        }

        public void Mute() => SetVolume(0);
    }

    // Primitiva operationer
    abstract class Device
    {
        public abstract string Name { get; }
    }

    class Radio : Device
    {
        public override string Name => "Radio";
    }

    class TV : Device
    {
        public override string Name => "TV";
    }
}


//class ExtraBatteriesRemote : BasicRemote
//{
//    public ExtraBatteriesRemote(Device device) : base(device)
//    {
//        Batteries.Add(30);
//    }

//    public void Mute() => _device.SetVolume(0);
//}
//{

//    var remote = new BasicRemote(new TV());
//    var extra = new ExtraBatteriesRemote(new TV());

//    Assert.AreEqual(30, remote.RemainingBattery);
//    Assert.AreEqual(60, extra.RemainingBattery);
//}
//{

//    var remote = new BasicRemote(new Radio());
//    var extra = new ExtraBatteriesRemote(new Radio());

//    Assert.AreEqual(30, remote.RemainingBattery);
//    Assert.AreEqual(60, extra.RemainingBattery);
//}