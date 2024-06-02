using System.Net;
using MiioNet8.Communication;
using MiioNet8.Devices;
using MiioNet8.Protocol;

namespace MiioNet8
{
    public static class DeviceFactory
    {
        public static async Task<T> Create<T>(IPAddress iPAddress, string tokenValue, int port = 54321, CancellationToken cToken = default)
            where T : BaseDevice
        {
            if (!Token.TryParse(tokenValue, out var token))
                throw new Exception();

            if (Activator.CreateInstance(typeof(T), new UdpCommunication(iPAddress, port), token) is not T device)
                throw new Exception();

            if (await device.ConnectAsync(cToken) != CommunicationResult.Success)
                throw new Exception();

            return device;
        }

        public static async Task<T> Create<T>(string iPAddress, string tokenValue, int port = 54321,CancellationToken cToken = default)
            where T : BaseDevice
        {
            if (!IPAddress.TryParse(iPAddress, out var address))
                throw new Exception();

            return await Create<T>(address, tokenValue, port, cToken);
        }
    }
}