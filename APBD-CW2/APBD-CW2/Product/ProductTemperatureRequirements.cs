namespace APBD_CW2.Product;

public static class ProductTemperatureRequirements
{
    public static readonly Dictionary<Product, int> MinimumTemperatures = new Dictionary<Product, int>
    {
        { Product.Bananas, 13 },
        { Product.Chocolate, 18 },
        { Product.Fish, 2 },
        { Product.Meat, -15 },
        { Product.IceCream, -18 },
        { Product.FrozenPizza, -30 },
        { Product.Cheese, 7 },
        { Product.Sausages, 5 },
        { Product.Butter, 20 },
        { Product.Eggs, 19 }
    };
}