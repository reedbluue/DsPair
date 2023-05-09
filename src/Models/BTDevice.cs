using InTheHand.Net.Sockets;

namespace DsPair.src.Models;

public class BTDevice {

  public string name { get; }
  public string macAddress { get; }

  public BTDevice(BluetoothDeviceInfo device) {
    name = device.DeviceName;
    macAddress = device.DeviceAddress.ToString();
  }
}
