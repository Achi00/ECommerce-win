using ECommerce.Library.Products.Interfaces;

namespace ECommerce.Library.Cart.Validation
{
    public interface IValidatorFactory
    {
        ICartValidator<T> GetValidator<T>() where T : IProduct;
        void ValidateProduct(IProduct product, int quantity);
    }
}