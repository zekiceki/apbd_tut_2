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
    private double cargoMass;

    public LiquidContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, bool isHazardous, double pressure)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        IsHazardous = isHazardous;
        Pressure = pressure;
    }

    public override void LoadCargo(double cargoMass)
    {
        if (cargoMass > MaxPayload)
            throw new OverfillException($"Cargo mass ({cargoMass}kg) exceeds the maximum payload ({MaxPayload}kg) of container {SerialNumber}.");

        if (IsHazardous && cargoMass > MaxPayload * 0.5)
            throw new OverfillException($"Hazardous cargo cannot exceed 50% of the container's capacity. Container: {SerialNumber}");

        this.cargoMass = cargoMass;
    }

    public override void EmptyCargo()
    {
        this.cargoMass = 0;
    }

    public void NotifyHazard(string containerNumber)
    {
        Console.WriteLine($"Hazardous situation detected in container {containerNumber}");
    }
}

public class GasContainer : Container, IHazardNotifier
{
    public double Pressure { get; protected set; }
    private double cargoMass;

    public GasContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, double pressure)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        Pressure = pressure;
    }

    public override void LoadCargo(double cargoMass)
    {
        if (cargoMass > MaxPayload)
            throw new OverfillException($"Cargo mass ({cargoMass}kg) exceeds the maximum payload ({MaxPayload}kg) of container {SerialNumber}.");

        this.cargoMass = cargoMass;
    }

    public override void EmptyCargo()
    {
        this.cargoMass *= 0.95; 
    }

    public void NotifyHazard(string containerNumber)
    {
        Console.WriteLine($"Hazardous situation detected in container {containerNumber}");
    }
}

public class RefrigeratedContainer : Container
{
    public Dictionary<string, double> TemperatureSettings { get; protected set; }
    private double cargoMass;

    public RefrigeratedContainer(string serialNumber, double mass, double height, double tareWeight, double depth, double maxPayload, Dictionary<string, double> temperatureSettings)
        : base(serialNumber, mass, height, tareWeight, depth, maxPayload)
    {
        TemperatureSettings = temperatureSettings;
    }

    public override void LoadCargo(double cargoMass)
    {
        if (cargoMass > MaxPayload)
            throw new OverfillException($"Cargo mass ({cargoMass}kg) exceeds the maximum payload ({MaxPayload}kg) of container {SerialNumber}.");

        this.cargoMass = cargoMass;
    }

    public override void EmptyCargo()
    {
        this.cargoMass = 0;
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
        if (Containers.Count >= MaxContainerNum)
        {
            Console.WriteLine($"Cannot load container. Maximum container limit ({MaxContainerNum}) reached on ship {ShipName}.");
            return;
        }

        double totalWeight = TotalWeight() + container.Mass;
        if (totalWeight > MaxWeight)
        {
            Console.WriteLine($"Cannot load container {container.SerialNumber}. Maximum weight limit ({MaxWeight}kg) reached on ship {ShipName}.");
            return;
        }

        Containers.Add(container);
        Console.WriteLine($"Container {container.SerialNumber} loaded on ship {ShipName}.");
    }

    public void UnloadContainer(Container container)
    {
        if (Containers.Remove(container))
            Console.WriteLine($"Container {container.SerialNumber} unloaded from ship {ShipName}.");
        else
            Console.WriteLine($"Container {container.SerialNumber} not found on ship {ShipName}.");
    }

    public void ReplaceContainer(Container oldContainer, Container newContainer)
    {
        int index = Containers.IndexOf(oldContainer);
        if (index != -1)
        {
            Containers[index] = newContainer;
            Console.WriteLine($"Container {oldContainer.SerialNumber} replaced with {newContainer.SerialNumber} on ship {ShipName}.");
        }
        else
        {
            Console.WriteLine($"Container {oldContainer.SerialNumber} not found on ship {ShipName}. Replacement failed.");
        }
    }

    public void PrintShipInfo()
    {
        Console.WriteLine($"Ship: {ShipName}");
        Console.WriteLine($"Max Speed: {MaxSpeed} knots");
        Console.WriteLine($"Max Container Capacity: {MaxContainerNum}");
        Console.WriteLine($"Max Weight Capacity: {MaxWeight} tons");

        Console.WriteLine("Containers on board:");
        foreach (var container in Containers)
        {
            Console.WriteLine($"- {container.SerialNumber}");
        }
    }

    private double TotalWeight()
    {
        double totalWeight = 0;
        foreach (var container in Containers)
        {
            totalWeight += container.Mass;
        }
        return totalWeight;
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

