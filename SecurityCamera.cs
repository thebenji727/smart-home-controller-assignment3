namespace SmartHomeLib;

public class SecurityCamera : SmartDevice
{
    public bool IsRecording { get; private set; }
    public int StorageCapacityMB { get; }  // read-only after construction
    public int StorageUsedMB { get; private set; } // 0..capacity

    // Simple formula: MB per minute. You can change this, but keep enforcement.
    private const int MbPerMinute = 5;

    public SecurityCamera(string deviceId, string name, int storageCapacityMB)
        : base(deviceId, name)
    {
        if (storageCapacityMB <= 0)
            throw new ArgumentException("Storage capacity must be greater than 0.", nameof(storageCapacityMB));

        StorageCapacityMB = storageCapacityMB;
        StorageUsedMB = 0;
        IsRecording = false;
    }

    public void StartRecording()
    {
        if (!IsOnline || !IsPoweredOn)
            throw new InvalidOperationException("Camera must be online and powered on to start recording.");

        if (StorageUsedMB >= StorageCapacityMB)
            throw new InvalidOperationException("No storage available to start recording.");

        IsRecording = true;
    }

    public void StopRecording()
    {
        IsRecording = false;
    }

    public void SimulateRecording(int minutes)
    {
        if (minutes <= 0)
            throw new ArgumentException("Minutes must be greater than 0.", nameof(minutes));

        if (!IsRecording)
            throw new InvalidOperationException("Camera must be recording to simulate recording.");

        // Compute new storage usage
        checked
        {
            int additional = minutes * MbPerMinute;
            int newUsed = StorageUsedMB + additional;

            if (newUsed > StorageCapacityMB)
                throw new InvalidOperationException("Recording would exceed storage capacity.");

            StorageUsedMB = newUsed;
        }
    }

    public override void TurnOff()
    {
        // reasonable behavior: turning off stops recording
        StopRecording();
        base.TurnOff();
    }

    public override void SetOnline(bool online)
    {
        base.SetOnline(online);
        // base will force power off if offline; stop recording too
        if (!IsOnline)
            StopRecording();
    }

    public override void ApplyMode(string mode)
    {
        if (string.IsNullOrWhiteSpace(mode)) return;

        if (mode.Trim().Equals("Night", StringComparison.OrdinalIgnoreCase))
        {
            // Requirement: "Night" starts recording (if allowed)
            // "if allowed" means: online + powered on + storage available
            if (!IsRecording && IsOnline && IsPoweredOn && StorageUsedMB < StorageCapacityMB)
                IsRecording = true; // same effect as StartRecording, but without throwing
        }
    }

    public override string GetStatus()
    {
        return $"[Camera] {Name} ({DeviceId}) | Online={IsOnline} | Power={IsPoweredOn} | Recording={IsRecording} | Storage={StorageUsedMB}/{StorageCapacityMB}MB";
    }
}
