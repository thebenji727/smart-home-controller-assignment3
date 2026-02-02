

using System.Collections.ObjectModel;

namespace SmartHomeLib;

public class SmartHomeHub
{
    private readonly List<SmartDevice> _devices = new();

    public IReadOnlyList<SmartDevice> Devices => new ReadOnlyCollection<SmartDevice>(_devices);

    public void AddDevice(SmartDevice device)
    {
        if (device is null)
            throw new ArgumentNullException(nameof(device));

        if (_devices.Any(d => d.DeviceId == device.DeviceId))
            throw new InvalidOperationException($"A device with id '{device.DeviceId}' already exists.");

        _devices.Add(device);
    }

    public bool RemoveDevice(string deviceId)
    {
        var device = _devices.FirstOrDefault(d => d.DeviceId == deviceId);
        if (device is null) return false;
        _devices.Remove(device);
        return true;
    }

    public void TurnOffAll()
    {
        foreach (var device in _devices)
            device.TurnOff();
    }

    public void ApplyModeToAll(string mode)
    {
        foreach (var device in _devices)
            device.ApplyMode(mode);
    }

    public void PrintAllStatuses()
    {
        foreach (var device in _devices)
            Console.WriteLine(device.GetStatus());
    }
}

