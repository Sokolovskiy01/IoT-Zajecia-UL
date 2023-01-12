
# Azure IoT Case Study project

Project Documentation file: documentation.pdf

## Project depencencies

### Project.sln:
```
Microsoft.Azure.Devices
Microsoft.Azure.Devices.Client
Projekt.VirtualDevice
```

### Projekt.FunctionApps.sln:
```
Microsoft.Azure.Devices
Microsoft.Azure.WebJobs.Extensions.ServiceBus
Microsoft.NET.Sdk.Functions
```

## Project startup

Open and run `Projekt/Projekt.sln` for reading and sending data, and `Projekt.FunctionApps/Projekt.FunctionApps.sln` to call function on triggers.

## Startup requirements

"IoTSim" Opc UA server with 6 device simulations

## Configuration files

### Projekt.sln
Properties/Resources.resx

### Projekt.FunctionApps/Projekt.FunctionApps.sln
Properties/Resources.resx
local.settings.json
