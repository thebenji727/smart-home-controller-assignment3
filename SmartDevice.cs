

namespace SmartHomeLib;

public abstract class SmartDevice
{
    public string DeviceId { get; }
    public string Name { get; private set; }
    public bool IsOnline { get; private set; }
    public bool IsPoweredOn { get; private set; }

    protected SmartDevice(string deviceId, string name)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("DeviceId cannot be blank.", nameof(deviceId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be blank.", nameof(name));

        DeviceId = deviceId;
        Name = name;
        IsOnline = false;
        IsPoweredOn = false;
    }

    public void SetOnline(bool online)
    {
        IsOnline = online;

        // Optional: if device goes offline, force power off
        if (!IsOnline)
            IsPoweredOn = false;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be blank.", nameof(newName));

        Name = newName.Trim();
    }

    public virtual void TurnOn()
    {
        if (!IsOnline)
            throw new InvalidOperationException("Device must be online to power on.");

        IsPoweredOn = true;
    }

    public virtual void TurnOff()
    {
        IsPoweredOn = false;
    }

    public virtual void ApplyMode(string mode)
    {
        // Default: do nothing. Subclasses override.
    }

    public abstract string GetStatus();
}

