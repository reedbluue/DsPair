using DsPair.src.Enums;
using DsPair.src.Exceptions;
using DsPair.src.Services;
using InTheHand.Net.Sockets;
using System.Diagnostics;

namespace DsPair.src.Controllers;
abstract class BtController
{

    private static readonly DsService _service = new();
    public static ProgramStatus pairAllDs()
    {
        List<BluetoothDeviceInfo> devices = _service.searchAllDevices();
        int count = devices.Count;
        devices.ForEach(d =>
        {
            try
            {
                _service.pairDevice(d);
                count--;
            }
            catch
            {
                _service.unpairDevice(d);
            }
        });
        if (count == 0) return ProgramStatus.AllDevicesPaired;
        return ProgramStatus.PartialDevicesPaired;
    }

    public static ProgramStatus unpairAllDs()
    {
        List<BluetoothDeviceInfo> devices = _service.getAllPairedDevices();
        int count = devices.Count;
        devices.ForEach(d =>
        {
            try
            {
                _service.unpairDevice(d);
                count--;
            }
            catch (StatusException e)
            {
                Console.WriteLine(e);
            }
        });
        if (count == 0) return ProgramStatus.AllDevicesUnpaired;
        return ProgramStatus.PartialDevicesUnpaired;
    }

    public static ProgramStatus pairDsFromMac(string[] args)
    {
        if (args.Length < 2) throw new StatusException(ErrorStatus.MissingArgument);
        BluetoothDeviceInfo device = _service.searchDeviceByMacAddress(args[1]);
        try
        {
            _service.pairDevice(device);
        }
        catch
        {
            _service.unpairDevice(device);
        }
        return ProgramStatus.DevicePaired;
    }

    public static ProgramStatus unpairDsFromMac(string[] args)
    {
        if (args.Length < 2) throw new StatusException(ErrorStatus.MissingArgument);
        BluetoothDeviceInfo device = _service.getCachedDeviceByMacAddress(args[1]);
        _service.unpairDevice(device);
        return ProgramStatus.DeviceUnpaired;
    }
}
