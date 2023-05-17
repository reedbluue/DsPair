using DsPair.src.Exceptions;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace DsPair.src.Services;

class DsService {

  private readonly BluetoothClient _client = new BluetoothClient();
  private readonly Guid _hidGuid = new Guid("00001124-0000-1000-8000-00805f9b34fb");

  public Boolean isAdapterOn() {
    try {
      if(!BluetoothRadio.Default.Mode.Equals(RadioMode.PowerOff)) return true;
    } catch { return false; }
    return false;
  }

  public List<BluetoothDeviceInfo> searchAllDevices() {
    List<BluetoothDeviceInfo> devices;

    try {
      devices = _client.DiscoverDevices().ToList().Where(d => d.DeviceName.Equals("Wireless Controller")).ToList();
    } catch {
      throw new StatusException("Fail on scan the bluetooth devices.", System.Net.HttpStatusCode.InternalServerError);
    }

    return devices;
  }

  public List<BluetoothDeviceInfo> getAllPairedDevices() {
    List<BluetoothDeviceInfo> devices;
    try {
      devices = _client.PairedDevices.ToList().Where(d => d.DeviceName.Equals("Wireless Controller")).ToList();
    } catch(StatusException e) {
      throw new StatusException("Fail on return cached bluetooth devices.", System.Net.HttpStatusCode.InternalServerError);
    }

    return devices;
  }

  public BluetoothDeviceInfo searchDeviceByMacAddress(string mac) {
    string macAddress = mac.Trim().Replace("-", "").Replace(":", "");
    BluetoothDeviceInfo device = searchAllDevices().Find(d => d.DeviceAddress.ToString().Equals(macAddress.ToUpper())); ;
    if(device == default) throw new StatusException("Bluetooth device not finded.", System.Net.HttpStatusCode.NotFound);
    return device;
  }

  public BluetoothDeviceInfo getCachedDeviceByMacAddress(string mac) {
    string macAddress = mac.Trim().Replace("-", "").Replace(":", "");
    BluetoothDeviceInfo device = getAllPairedDevices().Find(d => d.DeviceAddress.ToString().Equals(macAddress.ToUpper()));
    if(device == default) throw new StatusException("Device is not paired.", System.Net.HttpStatusCode.NotFound);
    return device;
  }

  public void pairDevice(BluetoothDeviceInfo device) {
    bool res = BluetoothSecurity.PairRequest(device.DeviceAddress, null);
    if(!res) throw new StatusException("Bluetooth pair failed.", System.Net.HttpStatusCode.InternalServerError);

    try {
      device.SetServiceState(_hidGuid, true);
    } catch {
      throw new StatusException("Fail on hid service start", System.Net.HttpStatusCode.InternalServerError);
    }

    Thread.Sleep(3000);

    for(int i = 0; i < 5; i++) {
      if(_checkIfServiceHasStarted(device)) return;
      Thread.Sleep(1000);
    }

    throw new StatusException("Bluetooth pair failed", System.Net.HttpStatusCode.InternalServerError);
  }

  public void unpairDevice(BluetoothDeviceInfo device) {
    bool res = BluetoothSecurity.RemoveDevice(device.DeviceAddress);
    if(!res) throw new StatusException("Bluetooth unpair failed", System.Net.HttpStatusCode.InternalServerError);

    Thread.Sleep(3000);

    for(int i = 0; i < 5; i++) {
      if(!_checkIfServiceHasStarted(device)) {
        Thread.Sleep(1000);
        return;
      }
    }

    throw new StatusException("Bluetooth unpair failed.", System.Net.HttpStatusCode.InternalServerError);
  }

  private bool _checkIfServiceHasStarted(BluetoothDeviceInfo device) {
    try {
      device.Refresh();
    } catch {
      throw new StatusException("Fail on bluetooth device update.", System.Net.HttpStatusCode.InternalServerError);
    }

    List<Guid> servicos = device.InstalledServices.ToList().Where(guid => guid.Equals(_hidGuid)).ToList();

    if(servicos.Count > 0) return true;
    return false;
  }
};