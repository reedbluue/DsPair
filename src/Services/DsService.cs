using DsPair.src.Enums;
using DsPair.src.Exceptions;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace DsPair.src.Services;
class DsService
{

    private readonly BluetoothClient _client = new();
    private readonly Guid _hidGuid = new("00001124-0000-1000-8000-00805f9b34fb");

    public List<BluetoothDeviceInfo> searchAllDevices()
    {
        List<BluetoothDeviceInfo> devices;

        try
        {
            devices = _client.DiscoverDevices().ToList().Where(d => d.DeviceName.Equals("Wireless Controller")).ToList();
        }
        catch
        {
            throw new StatusException(ErrorStatus.FailOnScanBluetoothDevices);
        }

        Console.WriteLine("Finded devices:");
        devices.ForEach(d => Console.WriteLine(_printDevice(d)));
        Console.WriteLine();

        if (devices.Count <= 0) throw new StatusException(ErrorStatus.BluetoothDeviceNotFinded);
        return devices;
    }

    public List<BluetoothDeviceInfo> getAllPairedDevices()
    {
        List<BluetoothDeviceInfo> devices;
        try
        {
            devices = _client.PairedDevices.ToList().Where(d => d.DeviceName.Equals("Wireless Controller")).ToList();
        }
        catch (StatusException e)
        {
            throw new StatusException(ErrorStatus.FailOnReturnCachedBluetoothDevices);
        }

        if (devices.Count <= 0) throw new StatusException(ErrorStatus.WithoutPairedDevices);

        Console.WriteLine("Paired devices:");
        devices.ForEach(d => Console.WriteLine(_printDevice(d)));
        Console.WriteLine();

        return devices;
    }

    public BluetoothDeviceInfo searchDeviceByMacAddress(string mac)
    {
        string macAddress = mac.Trim().Replace("-", "").Replace(":", "");
        BluetoothDeviceInfo? device = searchAllDevices().Find(d => d.DeviceAddress.ToString().Equals(macAddress)); ;
        if (device == default) throw new StatusException(ErrorStatus.BluetoothDeviceNotFinded);

        Console.WriteLine("Device finded: " + _printDevice(device) + "\n");

        return device;
    }

    public BluetoothDeviceInfo getCachedDeviceByMacAddress(string mac)
    {
        string macAddress = mac.Trim().Replace("-", "").Replace(":", "");
        BluetoothDeviceInfo? device = getAllPairedDevices().Find(d => d.DeviceAddress.ToString().Equals(macAddress));
        if (device == default) throw new StatusException(ErrorStatus.DeviceIsNotPaired);

        Console.WriteLine("Paired device finded: " + _printDevice(device) + "\n");

        return device;
    }

    public void pairDevice(BluetoothDeviceInfo device)
    {
        Console.WriteLine("Pairing the device: " + _printDevice(device) + "\n");

        bool res = BluetoothSecurity.PairRequest(device.DeviceAddress, null);
        if (!res) throw new StatusException(ErrorStatus.BluetoothPairFailed);

        try
        {
            device.SetServiceState(_hidGuid, true);
        }
        catch
        {
            throw new StatusException(ErrorStatus.FailOnHidServiceStart);
        }

        for (int i = 0; i < 5; i++)
        {
            if (_checkIfServiceHasStarted(device)) return;
            Thread.Sleep(1000);
        }

        throw new StatusException(ErrorStatus.BluetoothPairFailed);
    }

    public void unpairDevice(BluetoothDeviceInfo device)
    {
        Console.WriteLine("Unpairing the device: " + _printDevice(device) + "\n");

        bool res = BluetoothSecurity.RemoveDevice(device.DeviceAddress);
        if (!res) throw new StatusException(ErrorStatus.BluetoothUnpairFailed);

        for (int i = 0; i < 5; i++)
        {
            if (!_checkIfServiceHasStarted(device))
            {
                Thread.Sleep(5000);
                return;
            }
            Thread.Sleep(1000);
        }

        throw new StatusException(ErrorStatus.BluetoothUnpairFailed);
    }

    private bool _checkIfServiceHasStarted(BluetoothDeviceInfo device)
    {
        try
        {
            device.Refresh();
        }
        catch
        {
            throw new StatusException(ErrorStatus.FailOnBluetoothDeviceUpdate);
        }

        List<Guid> servicos = device.InstalledServices.ToList().Where(guid => guid.Equals(_hidGuid)).ToList();

        if (servicos.Count > 0) return true;
        return false;
    }

    private string _printDevice(BluetoothDeviceInfo device)
    {
        return device.DeviceName + " [" + device.DeviceAddress + "]";
    }

}