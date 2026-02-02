using SmartHomeLib;

var hub = new SmartHomeHub();

var light = new SmartLight("light-001", "Living Room Light");
var thermostat = new SmartThermostat("thermo-001", "Hall Thermostat");
var cam = new SecurityCamera("cam-001", "Front Door Cam", storageCapacityMB: 500);

hub.AddDevice(light);
hub.AddDevice(thermostat);
hub.AddDevice(cam);

Console.WriteLine("SmartHomeConsole starting...\n");

// Bring devices online
light.SetOnline(true);
thermostat.SetOnline(true);
cam.SetOnline(true);

// Power on
light.TurnOn();
thermostat.TurnOn();
cam.TurnOn();

// Change settings (requires powered on)
light.SetBrightness(80);
light.SetColor("Warm White");
thermostat.SetTemperature(72);

// Camera recording simulation
cam.StartRecording();
cam.SimulateRecording(10); // 10 minutes

Console.WriteLine("=== Before Mode ===");
hub.PrintAllStatuses();

hub.ApplyModeToAll("Night");

Console.WriteLine("\n=== After Night Mode ===");
hub.PrintAllStatuses();

hub.TurnOffAll();

Console.WriteLine("\n=== After TurnOffAll ===");
hub.PrintAllStatuses();
