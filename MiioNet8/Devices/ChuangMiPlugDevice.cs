using MiioNet8.Commands;
using MiioNet8.Communication;
using MiioNet8.Interfaces;

namespace MiioNet8.Devices;

/// <summary>
/// Mi Smart Power Plug 2 (Wi-Fi and Bluetooth Gateway)
/// </summary>
public class ChuangMiPlugDevice : GenericDevice
{
    public override string Model { get; protected set; } = "chuangmi.plug.212a01";

    public ChuangMiPlugDevice(ICommunication communication, IToken token) : base(communication, token)
    {
    }

    /// <summary>
    /// Electric power
    /// </summary>
    /// <remarks>unit: watt</remarks>
    /// <returns></returns>
    public async Task<double?> ElectricPowerAsync()
    {
        try
        {
            if (!GetSpecServiceProperty("Power Consumption", "electric power", out var p4) || p4 == null)
            {
                return null;
            }

            var cmd = BuildGetPropertiesCommand([p4]);
            var power = await GetPropertyAsync<int>(cmd);
            return power / 100.0;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool?> PowerState()
    {
        if (!GetSpecServiceProperty("Switch", "Switch Status", out var p6) || p6 == null)
        {
            //bool r-w-n
            return null;
        }

        var cmd = BuildGetPropertiesCommand([p6]);
        return await GetPropertyAsync<bool>(cmd);
    }

#if DEBUG
    private async Task GetAllPropAsync()
    {
        var pp = new List<ISpecServiceProperty>();
        if (GetSpecServiceProperty("Device Information", "Device Manufacturer", out var p1) && p1 != null)
        {
            //string r
            pp.Add(p1);
        }

        if (GetSpecServiceProperty("Device Information", "Device Model", out var p2) && p2 != null)
        {
            //string r
            pp.Add(p2);
        }

        if (GetSpecServiceProperty("Device Information", "Device Serial Number", out var p3) && p3 != null)
        {
            //string r
            pp.Add(p3);
        }

        if (GetSpecServiceProperty("Device Information", "Current Firmware Version", out var p4) && p4 != null)
        {
            //string r
            pp.Add(p4);
        }

        if (GetSpecServiceProperty("Switch", "Switch Status", out var p6) && p6 != null)
        {
            //bool r-w-n
            pp.Add(p6);
        }

        if (GetSpecServiceProperty("Switch", "Temperature", out var p7) && p7 != null)
        {
            //uint8 r-n
            pp.Add(p7);
        }

        if (GetSpecServiceProperty("Switch", "Working Time", out var p8) && p8 != null)
        {
            //uint32 r-n
            pp.Add(p8);
        }


        if (GetSpecServiceProperty("Indicator Light", "Switch Status", out var p9) && p9 != null)
        {
            //bool r-w-n
            pp.Add(p9);
        }


        if (GetSpecServiceProperty("Power Consumption", "Power Consumption", out var p10) && p10 != null)
        {
            //u32 r-n
            pp.Add(p10);
        }

        if (GetSpecServiceProperty("Power Consumption", "Electric Current", out var p11) && p11 != null)
        {
            //u16 r-n
            pp.Add(p11);
        }

        if (GetSpecServiceProperty("Power Consumption", "Voltage", out var p12) && p12 != null)
        {
            //u16 r-n
            pp.Add(p12);
        }

        if (GetSpecServiceProperty("Power Consumption", "Electric Power", out var p13) && p13 != null)
        {
            //u32 r-n
            pp.Add(p13);
        }

        if (GetSpecServiceProperty("imilab-timer", "on-duration", out var p14) && p14 != null)
        {
            //u32 r-w-n
            pp.Add(p14);
        }

        if (GetSpecServiceProperty("imilab-timer", "off-duration", out var p15) && p15 != null)
        {
            //u32 r-w-n
            pp.Add(p15);
        }

        if (GetSpecServiceProperty("imilab-timer", "countdown", out var p16) && p16 != null)
        {
            //u32 r-w-n
            pp.Add(p16);
        }

        if (GetSpecServiceProperty("imilab-timer", "task-switch", out var p17) && p17 != null)
        {
            //bool r-w-n
            pp.Add(p17);
        }

        if (GetSpecServiceProperty("imilab-timer", "countdown-info", out var p18) && p18 != null)
        {
            //bool r-n
            pp.Add(p18);
        }

        var cmd = BuildGetPropertiesCommand(pp);
        var res = await GetPropertiesAsync(cmd);
        if (res.Item1 != CommunicationResult.Success || res.Item2 == null || res.Item2.Count == 0)
        {
            throw new Exception("can not get properities");
        }
    }
#endif


    private static GetPropertiesCommand BuildGetPropertiesCommand(IEnumerable<ISpecServiceProperty> properties)
    {
        var k = properties.Select(s => (s.Description.ToLower().Replace(' ', '-'), s.Service.Id, s.Id));
        return new GetPropertiesCommand(k);
    }
}