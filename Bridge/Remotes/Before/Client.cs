
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

                Assert.AreEqual(30, remote.GetVolume());
                Assert.AreEqual("TV", remote.DeviceName);
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
            }
            {
                var remote = new BasicRadioRemote();
                remote.VolumeUp();
                remote.VolumeUp();
                Assert.AreEqual(20, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
            }
            {
                var remote = new AdvancedRadioRemote();
                remote.VolumeUp();
                remote.VolumeUp();
                Assert.AreEqual(20, remote.GetVolume());
                Assert.AreEqual("Radio", remote.DeviceName);
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

        public abstract string DeviceName { get; }

    }

    class BasicTvRemote : BasicRemote
    {
        public override string DeviceName => "TV";
    }


    class BasicRadioRemote : BasicRemote
    {
        public override string DeviceName => "Radio";
    }

    class AdvancedTvRemote : BasicRemote
    {
        public override string DeviceName => "TV";
        public void Mute() => SetVolume(0);
    }

    class AdvancedRadioRemote : BasicRemote
    {
        public override string DeviceName => "Radio";
        public void Mute() => SetVolume(0);
    }




    //class ExtraBatteriesRemote : Remote
    //{
    //    public ExtraBatteriesRemote()
    //    {
    //        Batteries.Add(30);
    //    }

    //    public void Mute() => SetVolume(0);
    //}



}
