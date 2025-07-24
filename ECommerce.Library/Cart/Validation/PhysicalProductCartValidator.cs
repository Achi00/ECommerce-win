using ECommerce.Library.Exceptions;
using ECommerce.Library.Products.Interfaces;


namespace ECommerce.Library.Cart.Validation
{
    public class PhysicalProductCartValidator : ICartValidator<IPhysicalProduct>
    {
        public bool Validate(IPhysicalProduct product, int quantity, string source = "unknown")
        {
            if (product is not IPhysicalProduct physical)
            {
                throw new ArgumentException("Invalid product type for physical validator.");
            }
            if (product.Stock() >= quantity)
            {
                return true;
            }
            else
            {
                throw new ProductStockException(
                 productId: product.Id(),
                 productName: product.Name(),
                 availableStock: product.Stock(),
                 attemptedQuantity: quantity,
                 sourceOperation: source
             );
            }
        }
    }
}
