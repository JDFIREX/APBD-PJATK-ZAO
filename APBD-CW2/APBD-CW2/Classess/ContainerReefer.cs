using APBD_CW2.Exceptions;
using APBD_CW2.Interfaces;
using APBD_CW2.Product;

namespace APBD_CW2.Classess;

public class ContainerReefer: Container, IHazardNotifier
{
    private int _temperature;
    public Product.Product Product { get; set; }

    public ContainerReefer(
        int weight,
        int height,
        int containerWeight,
        int depth,
        string serialNumber,
        int maxWeight,
        Product.Product product,
        int temperature
    ): base(weight, height, containerWeight, depth, serialNumber, maxWeight)
    {
        Product = product;
        Temperature = temperature;
    }
    
    public int Temperature
    {
        get => _temperature;
        set
        {
            if (ProductTemperatureRequirements.MinimumTemperatures.ContainsKey(Product) && value < ProductTemperatureRequirements.MinimumTemperatures[Product])
            {
                throw new ProductTemperatureToLowException(Product);
            }
            _temperature = value;
        }
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