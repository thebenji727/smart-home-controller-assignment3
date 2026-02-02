namespace SmartHomeLib;

public class SmartLight : SmartDevice
{
    public int Brightness { get; private set; } // 0-100
    public string Color { get; private set; }   // non-blank

    public SmartLight(string deviceId, string name, int initialBrightness = 100, string initialColor = "White")
        : base(deviceId, name)
    {
        // allow construction defaults, but still validate
        if (initialBrightness < 0 || initialBrightness > 100)
            throw new ArgumentException("Brightness must be between 0 and 100.", nameof(initialBrightness));

        if (string.IsNullOrWhiteSpace(initialColor))
            throw new ArgumentException("Color cannot be blank.", nameof(initialColor));

        Brightness = initialBrightness;
        Color = initialColor.Trim();
    }

    public void SetBrightness(int value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentException("Brightness must be between 0 and 100.", nameof(value));

        if (!IsPoweredOn)
            throw new InvalidOperationException("Light must be powered on to change brightness.");

        Brightness = value;
    }

    public void SetColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException("Color cannot be blank.", nameof(color));

        if (!IsPoweredOn)
            throw new InvalidOperationException("Light must be powered on to change color.");

        Color = color.Trim();
    }

    public override void ApplyMode(string mode)
    {
        if (string.IsNullOrWhiteSpace(mode)) return;

        if (mode.Trim().Equals("Night", StringComparison.OrdinalIgnoreCase))
        {
            // Requirement: only if powered on
            if (IsPoweredOn)
                Brightness = 10;
        }
    }

    public override string GetStatus()
    {
        return $"[Light] {Name} ({DeviceId}) | Online={IsOnline} | Power={IsPoweredOn} | Brightness={Brightness} | Color={Color}";
    }
}
