
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
            // Fördel: det krävs faktiskt färre klasser här + om fler devices dyker upp så behöver vi inte ändra i andra klasser
            {
                var tv = new TV();
                var remote = new BasicRemote(tv); // en kombination av abstraction-implementation
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(3, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
                Assert.AreEqual(10, tv.MaxVolume);
            }

            {
                var tv = new TV();
                var remote = new AdvancedRemote(tv);
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.Mute();

                Assert.AreEqual(0, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
                Assert.AreEqual(10, tv.MaxVolume);
            }

            {
                var radio = new Radio();
                var remote = new BasicRemote(radio);
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                Assert.AreEqual(3, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
                Assert.AreEqual(3, radio.MaxVolume);
            }
            {
                var radio = new Radio();
                var remote = new AdvancedRemote(radio);
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(2, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
                Assert.AreEqual(3, radio.MaxVolume);
            }


        }
    }

    // Abstraction delegerar jobb till "_device"
    class BasicRemote
    {
        // Samma

        private int _volume;
        public bool IsEnabled { get; private set; }
        public void Disable() => IsEnabled = false;
        public void Enable() => IsEnabled = true;
        public int GetVolume() => _volume;
        
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

        // Olika:

        protected readonly IDevice _device;
        public BasicRemote(IDevice device) => _device = device;  // Konstruktorn kräver att en "device" skickas in

        public string DeviceName => _device.Name;

        public void SetVolume(int volume)
        {
            _volume = volume;
            _volume = Math.Max(0, _volume);
            _volume = Math.Min(_device.MaxVolume, _volume); // skillnad
        }
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


