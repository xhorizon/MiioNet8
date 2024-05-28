using System.Text.Json;
using MiioNet8.Commands;
using MiioNet8.Communication;
using MiioNet8.Interfaces;

namespace MiioNet8.Devices;

/// <summary>
/// MiHome air conditioner partner 2
/// </summary>
/// <remarks>chinese: 米家空调伴侣2</remarks>
public class LumiAcpartnerDevice : GenericDevice
{
    public override string Model { get; protected set; } = "lumi.acpartner.mcn02";

    public LumiAcpartnerDevice(ICommunication communication, IToken token) : base(communication, token)
    {
    }

    public async Task<double?> ElectricPowerAsync()
    {
        try
        {
            var cmd = new GetPropertiesCommand(["load_power"],
                "get_prop");
            var res = await GetRawPropertiesAsync(cmd);
            if (res is { Item1: CommunicationResult.Success, Item2.Count: >= 1 })
            {
                return res.Item2[0].Deserialize<double>();
            }

            return 0;
        }
        catch
        {
            return null;
        }
    }
}