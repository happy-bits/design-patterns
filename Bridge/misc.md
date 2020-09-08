# Markdown File

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