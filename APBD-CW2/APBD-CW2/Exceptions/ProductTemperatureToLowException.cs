using APBD_CW2.Product;

namespace APBD_CW2.Exceptions;

public class ProductTemperatureToLowException(Product.Product product): Exception($"Temperature cannot be lower than {ProductTemperatureRequirements.MinimumTemperatures[product]}°C for {product}.");