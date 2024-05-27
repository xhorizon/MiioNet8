using System.Diagnostics;
using MiioNet8.Devices;

namespace MiioNet8.UnitTests;

public class ChuangMiPlugTest
{
    [Test]
    public async Task ElectricPower()
    {
        var dev = await DeviceFactory.Create<ChuangMiPlugDevice>("192.168.100.154", "xxx");
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