using ECommerce.Library.Products.Interfaces;


namespace ECommerce.Library.Cart.Validation
{
    public class PhysicalProductCartValidator : ICartValidator<IPhysicalProduct>
    {
        public bool Validate(IPhysicalProduct product, int quantity)
        {
            if (product.Stock() >= quantity)
            {
                return true;
            }
            else
            {
                throw new InvalidOperationException("Not enought stock");
            }
        }
    }
}
