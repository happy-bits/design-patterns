
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Bridge.Remotes.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            {
                var remote = new BasicTvRemote();
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();

                Assert.AreEqual(3, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
                Assert.AreEqual(10, remote.MaxVolume);

            }

            {
                var remote = new AdvancedTvRemote();
                remote.TogglePower();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.Mute();

                Assert.AreEqual(0, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
                Assert.AreEqual(10, remote.MaxVolume);
            }
            {
                var remote = new BasicRadioRemote();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                remote.VolumeUp();
                Assert.AreEqual(3, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
                Assert.AreEqual(3, remote.MaxVolume);
            }
            {
                var remote = new AdvancedRadioRemote();
                remote.VolumeUp();
                remote.VolumeUp();
                Assert.AreEqual(2, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
                Assert.AreEqual(3, remote.MaxVolume);
            }

        }
    }

    abstract class BasicRemote
    {
        private int _volume;

        public bool IsEnabled { get; private set; }

        public void Disable() => IsEnabled = false;

        public void Enable() => IsEnabled = true;

        public int GetVolume() => _volume;

        public void SetVolume(int volume)
        {
            _volume = volume;
            _volume = Math.Max(0, _volume);
            _volume = Math.Min(MaxVolume, _volume);
        }

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

        public abstract string DeviceName { get; }

        public abstract int MaxVolume { get;  }

    }

    // Nackdel: upprepning av kod (devicename + maxvolume)

    class BasicTvRemote : BasicRemote
    {
        public override string DeviceName => "TV";
        public override int MaxVolume => 10;
    }


    class BasicRadioRemote : BasicRemote
    {
        public override string DeviceName => "Radio";
        public override int MaxVolume => 3;
    }

    // Kan låta dessa två klasser ärva från "AdvancedBase" men de kan samtidigt inte ärva från "TvBase" eller "RadioBase"

    class AdvancedTvRemote : BasicRemote
    {
        public override string DeviceName => "TV";
        public void Mute() => SetVolume(0);
        public override int MaxVolume => 10;

    }

    class AdvancedRadioRemote : BasicRemote
    {
        public override string DeviceName => "Radio";
        public void Mute() => SetVolume(0);
        public override int MaxVolume => 3;
    }

}
