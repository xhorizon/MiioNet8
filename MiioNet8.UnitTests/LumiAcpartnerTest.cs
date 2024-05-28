using System.Diagnostics;
using MiioNet8.Devices;

namespace MiioNet8.UnitTests;

public class LumiAcpartnerTest
{
    [Test]
    public async Task ElectricPower()
    {
        var dev = await DeviceFactory.Create<LumiAcpartnerDevice>("192.168.100.172", "ad763f91b5d5928149faf66da44736ae");
        var t = 10;
        var sw = new Stopwatch();
        while (t > 0)
        {
            t--;
            sw.Start();
            var k = await dev.ElectricPowerAsync();
            sw.Stop();
            await TestContext.Progress.WriteLineAsync($"power({sw.ElapsedMilliseconds})> {k}");
            sw.Reset();
            Assert.IsTrue(k > 0);
            Thread.Sleep(1000);
        }
        
    }
}