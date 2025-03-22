using APBD_CW2.Interfaces;

namespace APBD_CW2.Classess;

public class ContainerGas: Container, IHazardNotifier
{
    private int Pressure { get; set; }

    public ContainerGas(
        int weight,
        int height,
        int containerWeight,
        int depth,
        string serialNumber,
        int maxWeight,
        int pressure
    ): base(weight, height, containerWeight, depth, serialNumber, maxWeight)
    {
        Pressure = pressure;
    }

    public void notify(string messsage)
    {
        Console.WriteLine(messsage);
    }

    public override void EmptyContainer()
    {
        base.Weight = (int)(base.MaxWeight * 0.05);
    }
    
}