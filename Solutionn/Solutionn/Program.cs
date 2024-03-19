using System;
using System.Collections.Generic;

public interface IHazardNotifier
{
    void NotifyHazard(string containerNumber);
}

public class OverfillException : Exception
{
    public OverfillException(string message) : base(message) { }
}

public abstract class Container
{
    public string SerialNumber { get; protected set; }
    public double Mass { get; protected set; }
    public double Height { get; protected set; }
    public double TareWeight { get; protected set; }
    public double Depth { get; protected set; }
    public double MaxPayload { get; protected set; }

    public Container(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload)
    {
        SerialNumber = serialNumber;
        Mass = mass;
        Height = height;
        TareWeight = tareWeight;
        Depth = depth;
        MaxPayload = maxPayload;
    }

    public abstract void LoadCargo(double cargoMass);
    public abstract void EmptyCargo();
}

public class LiquidContainer : Container, IHazardNotifier
{
    public bool IsHazardous { get; protected set; }
    public double Pressure { get; protected set; }

    public LiquidContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, bool isHazardous, double pressure)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        IsHazardous = isHazardous;
        Pressure = pressure;
    }

    public override void LoadCargo(double cargoMass)
    {
        // WILL COMPLETE LATER
    }

    public override void EmptyCargo()
    {
        // WILL COMPLETE LATER
    }

    public void NotifyHazard(string containerNumber)
    {
        // WILL COMPLETE LATER
    }
}

public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; protected set; }

    public GasContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, double pressure)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        Pressure = pressure;
    }

    public override void LoadCargo(double cargoMass)
    {
        // WILL COMPLETE LATER
    }

    public override void EmptyCargo()
    {
        // WILL COMPLETE LATER
    }

    public void NotifyHazard(string containerNumber)
    {
        // WILL COMPLETE LATER
    }
}

public class RefrigeratedContainer : Container
{
    public Dictionary<string, double> TemperatureSettings { get; protected set; }

    public RefrigeratedContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, Dictionary<string, double> temperatureSettings)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        TemperatureSettings = temperatureSettings;
    }

    public override void LoadCargo(double cargoMass)
    {
        // WILL COMPLETE LATER
    }

    public override void EmptyCargo()
    {
        // WILL COMPLETE LATER
    }
}

public class ContainerShip
{
    public string ShipName { get; protected set; }
    public double MaxSpeed { get; protected set; }
    public int MaxContainerNum { get; protected set; }
    public double MaxWeight { get; protected set; }
    public List<Container> Containers { get; protected set; }

    public ContainerShip(string shipName, double maxSpeed, int maxContainerNum, double maxWeight)
    {
        ShipName = shipName;
        MaxSpeed = maxSpeed;
        MaxContainerNum = maxContainerNum;
        MaxWeight = maxWeight;
        Containers = new List<Container>();
    }

    public void LoadContainer(Container container)
    {
        // WILL COMPLETE LATER
    }

    public void UnloadContainer(Container container)
    {
        // WILL COMPLETE LATER
    }

    public void ReplaceContainer(Container oldContainer, Container newContainer)
    {
        // WILL COMPLETE LATER
    }

    public void PrintShipInfo()
    {
        // WILL COMPLETE LATER
    }

    private double TotalWeight()
    {
        // WILL COMPLETE LATER
        return 0;
    }
}


class Program
{
    static void Main(string[] args)
    {
        try
        {
            LiquidContainer liquidContainer = new LiquidContainer("KON-L-1", 5000, 200, 100, 150, 10000, true, 1.5);
            ContainerShip containerShip = new ContainerShip("Ship 1", 20, 200, 50000);
            containerShip.LoadContainer(liquidContainer);
            containerShip.PrintShipInfo();
        }
        catch (Exception ex) { Console.WriteLine($"An error occurred: {ex.Message}"); }
    }
}
