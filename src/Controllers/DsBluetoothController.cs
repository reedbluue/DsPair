using DsPair.src.Models;
using DsPair.src.Services;
using InTheHand.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DsPair.src.Controllers;

[Route("api/dsbt")]
[ApiController]
public class DsBluetoothController: ControllerBase {

  private readonly DsService _service = new();

  /// <summary>
  /// Retorna uma lista de controles próximos ou pareados
  /// </summary>
  /// <param name="paired">
  /// Quando "false", retorna os dispositivos próximos disponíveis. Caso "true',
  /// retorna os dispositivos pareados.
  /// </param>
  [HttpGet]
  public List<BTDevice> Get([Required] bool paired = false) {
    if(!paired) {
      List<BluetoothDeviceInfo> devices = _service.searchAllDevices();
      return devices.Select(d => new BTDevice(d)).ToList();
    } else {
      List<BluetoothDeviceInfo> devices = _service.getAllPairedDevices();
      return devices.Select(d => new BTDevice(d)).ToList();
    }
  }

  /// <summary>
  /// Envia uma requisição de pareamento ou despareamento de um dispositivo
  /// </summary>
  /// <param name="macAddress">
  /// Endereço MAC do dispositivo
  /// </param>
  /// <param name="pair">
  /// Quando "true", pareia o dispositivo com o mac correspondente. Caso "false',
  /// despareia o dispositivo correspondente.
  /// </param>
  [HttpPost]
  public BTDevice Post([Required] string macAddress, [Required] bool pair) {
    if(pair) {
      BluetoothDeviceInfo device = _service.searchDeviceByMacAddress(macAddress);
      _service.pairDevice(device);
      return new BTDevice(device);
    } else {
      DsService _service = new DsService();
      BluetoothDeviceInfo device = _service.getCachedDeviceByMacAddress(macAddress);
      _service.unpairDevice(device);
      return new BTDevice(device);
    }
  }
}
