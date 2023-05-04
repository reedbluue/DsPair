namespace DsPair.src.Enums;
enum ErrorStatus
{
    FatalError = -1,
    InvalidFlag = -2,
    InvalidMode = -3,
    MissingMode = -4,
    MissingArgument = -5,
    FailOnScanBluetoothDevices = -6,
    BluetoothDeviceNotFinded = -7,
    FailOnHidServiceStart = -8,
    BluetoothPairFailed = -9,
    BluetoothUnpairFailed = -10,
    FailOnBluetoothDeviceUpdate = -11,
    WithoutPairedDevices = -12,
    DeviceIsNotPaired = -13,
    FailOnReturnCachedBluetoothDevices = -14,
}