using ECommerce.Library.Products.Interfaces;

namespace ECommerce.Library.Cart.Controllers
{
    public interface ICartController
    {
        void AddProductToCart(IProduct product, int quantity);
        IReadOnlyList<CartItem> CartItems();
        decimal CartTotal();
        void RemoveProductFromCart(IProduct product);
    }
}