
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
                var remote = new Remote(tv); // en kombination av abstraction-implementation
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                
                Assert.AreEqual(30, tv.GetVolume());
            }

            {
                var tv = new TV();
                var remote = new AdvancedRemote(tv);
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.Mute();

                Assert.AreEqual(0, tv.GetVolume());
            }
            {
                
                var remote = new Remote(new TV());
                var extra = new ExtraBatteriesRemote(new TV());

                Assert.AreEqual(30, remote.RemainingBattery);
                Assert.AreEqual(60, extra.RemainingBattery);
            }
            {
                var radio = new Radio();
                var remote = new Remote(radio);
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(20, radio.GetVolume());

            }

        }
    }

    // Abstraction delegerar jobb till "_device"
    class Remote
    {
        protected readonly IDevice _device;

        public Remote(IDevice device) => _device = device;

        public void TogglePower()
        {
            if (_device.IsEnabled)
                _device.Disable();

            _device.Enable();
        }

        public void VolumeUp() 
        {
            var old = _device.GetVolume();
            _device.SetVolume(old + 10);
        }
        public void VolumeDown()
        {
            var old = _device.GetVolume();
            _device.SetVolume(old - 10);
        }

        public List<double> Batteries { get; protected set; } = new List<double> { 30 };

        public double RemainingBattery => Batteries.Sum();
    }

    class AdvancedRemote : Remote
    {
        public AdvancedRemote(IDevice device) : base(device)
        {
        }

        public void Mute() => _device.SetVolume(0);
    }

    class ExtraBatteriesRemote : Remote
    {
        public ExtraBatteriesRemote(IDevice device) : base(device)
        {
            Batteries.Add(30);
        }

        public void Mute() => _device.SetVolume(0);
    }

    // Primitiva operationer
    interface IDevice
    {
        bool IsEnabled { get; }
        void Enable();
        void Disable();
        int GetVolume();
        void SetVolume(int percentage);
    }

    class TV : IDevice
    {
        private int _volume;

        public bool IsEnabled { get; private set; }

        public void Disable() => IsEnabled = false;

        public void Enable() => IsEnabled = true;

        public int GetVolume() => _volume;

        public void SetVolume(int percentage) => _volume = percentage;
    }

    class Radio : IDevice
    {
        private int _volume;

        public bool IsEnabled { get; private set; }

        public void Disable() => IsEnabled = false;

        public void Enable() => IsEnabled = true;

        public int GetVolume() => _volume;

        public void SetVolume(int percentage) => _volume = percentage;
    }
}
