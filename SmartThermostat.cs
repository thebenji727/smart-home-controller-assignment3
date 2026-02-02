namespace SmartHomeLib;

public class SmartThermostat : SmartDevice
{
    public int Temperature { get; private set; } // 50-90

    public SmartThermostat(string deviceId, string name, int initialTemp = 70)
        : base(deviceId, name)
    {
        if (initialTemp < 50 || initialTemp > 90)
            throw new ArgumentException("Temperature must be between 50 and 90.", nameof(initialTemp));

        Temperature = initialTemp;
    }

    public void SetTemperature(int temp)
    {
        if (temp < 50 || temp > 90)
            throw new ArgumentException("Temperature must be between 50 and 90.", nameof(temp));

        if (!IsPoweredOn)
            throw new InvalidOperationException("Thermostat must be powered on to change temperature.");

        Temperature = temp;
    }

    public override void ApplyMode(string mode)
    {
        if (string.IsNullOrWhiteSpace(mode)) return;

        if (mode.Trim().Equals("Night", StringComparison.OrdinalIgnoreCase))
        {
            // Requirement: only if powered on
            if (IsPoweredOn)
                Temperature = 65;
        }
    }

    public override string GetStatus()
    {
        return $"[Thermostat] {Name} ({DeviceId}) | Online={IsOnline} | Power={IsPoweredOn} | Temp={Temperature}";
    }
}
