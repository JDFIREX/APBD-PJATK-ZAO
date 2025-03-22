using System.Text.RegularExpressions;
using APBD_CW2.Exceptions;

namespace APBD_CW2.Classess;

public abstract class Container {
    private int _weight;
    private string _serialNumber;

    public int Height { get; set; }
    public int ContainerWeight { get; set; }
    public int Depth { get; set; }
    public int MaxWeight { get; set; }

    public Container(
        int weight,
        int height,
        int containerWeight,
        int depth,
        string serialNumber,
        int maxWeight
    )
    {
        Height = height;
        ContainerWeight = containerWeight;
        Depth = depth;
        MaxWeight = maxWeight;
        Weight = weight;
        SerialNumber = serialNumber;
    }
    
    public virtual int Weight
    {
        get => _weight;
        set
        {
            if (value > MaxWeight)
            {
                throw new ContainerMaxWeightException();
            }
            _weight = value;
        }
    }

    public string SerialNumber
    {
        get => _serialNumber;
        set
        {
            var pattern = @"^KON-[A-Z]{1}-\d{1}$";
            if (!Regex.IsMatch(value, pattern))
            {
                throw new ContainerInValidSerialNumberException();
            }
            
            _serialNumber = value;
        }
    }

    public virtual void EmptyContainer()
    {
        _weight = 0;
    }
    
}