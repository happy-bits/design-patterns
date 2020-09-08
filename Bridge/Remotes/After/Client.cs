
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

                Assert.AreEqual(3, remote.GetVolume());
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

                Assert.AreEqual(2, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
            }
            {
                var remote = new AdvancedRemote(new Radio());
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(2, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
            }


        }
    }

    // Abstraction delegerar jobb till "_device"
    class BasicRemote
    {
        protected readonly IDevice _device;

        private int _volume;

        public BasicRemote(IDevice device) => _device = device;

        public bool IsEnabled { get; private set; }

        public void Disable() => IsEnabled = false;

        public void Enable() => IsEnabled = true;

        public int GetVolume() => _volume;

        public void SetVolume(int volume) => _volume = volume;

        public void TogglePower()
        {
            if (IsEnabled)
                Disable();

            Enable();
        }

        public void VolumeUp()
        {
            var old = GetVolume();
            SetVolume(old + 1);
        }
        public void VolumeDown()
        {
            var old = GetVolume();
            SetVolume(old - 1);
        }

        public List<double> Batteries { get; protected set; } = new List<double> { 30 };

        public double RemainingBattery => Batteries.Sum();

        public string DeviceName => _device.Name;


    }

    class AdvancedRemote : BasicRemote
    {
        public AdvancedRemote(IDevice device) : base(device)
        {
        }

        public void Mute() => SetVolume(0);
    }

    // Primitiva operationer
    interface IDevice
    {
        string Name { get; }
        int MaxVolume { get; }
    }

    class Radio : IDevice
    {
        public string Name => "Radio";

        public int MaxVolume => 3;
    }

    class TV : IDevice
    {
        public string Name => "TV";
        public int MaxVolume => 10;
    }
}


