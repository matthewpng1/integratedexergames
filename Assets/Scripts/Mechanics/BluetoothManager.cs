using System.Runtime.InteropServices;
using UnityEngine;

public class BluetoothManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    private const string LIBRARY_PATH = "ArduinoBluetoothAPI"; // Windows
#elif UNITY_IOS || UNITY_MACOS
    private const string LIBRARY_PATH = "__Internal"; // macOS/iOS uses '__Internal'
#else
    private const string LIBRARY_PATH = "BluetoothUnityAPI"; // Default
#endif

    [DllImport(LIBRARY_PATH)]
    private static extern bool ConnectToBluetooth(string deviceName);

    [DllImport(LIBRARY_PATH)]
    private static extern bool IsJumpReceived();

    public bool ConnectToDevice(string deviceName)
    {
        return ConnectToBluetooth(deviceName);
    }

    public bool CheckJump()
    {
        return IsJumpReceived();
    }
}
