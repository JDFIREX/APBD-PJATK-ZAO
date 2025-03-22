using APBD_CW2.Interfaces;

namespace APBD_CW2.Classess;

public class ContainerLiquid: Container, IHazardNotifier
{
    private bool Dangerous { get; set; }

    public ContainerLiquid(
        int weight,
        int height,
        int containerWeight,
        int depth,
        string serialNumber,
        int maxWeight,
        bool dangerous
    ): base(weight, height, containerWeight, depth, serialNumber, maxWeight)
    {
        Dangerous = dangerous;
    }

    public void notify(string messsage)
    {
        Console.WriteLine(messsage);
    }
    
    public override int Weight
    {
        get => base.Weight;
        set
        {
            if (Dangerous && value > MaxWeight * 0.5)
            {
                notify("Warning: Dangerous liquid exceeds 50% of max weight!");
            }

            if (!Dangerous && value > MaxWeight * 0.9)
            {
                notify("Warning: liquid exceeds 90% of max weight!");
            }
            
            base.Weight = value;
        }
    }
    
}