namespace APBD_CW2.Classess;

public class ContainerShip
{
    public ICollection<Container> Containers { get; set; }
    public int MaxSpeed { get; set; }
    public int Limit { get; set; }
    public int MaxWeight { get; set; }

    public ContainerShip(ICollection<Container> containers, int maxSpeed, int limit, int maxWeight)
    {
        Containers = containers;
        MaxSpeed = maxSpeed;
        Limit = limit;
        MaxWeight = maxWeight;
    }
}