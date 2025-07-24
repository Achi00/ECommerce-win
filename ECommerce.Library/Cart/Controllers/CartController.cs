using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Cart.Controllers
{
    public class CartController
    {
        private readonly Cart _cart;
        // injects propriate dependency which is necessary for validate product depend on it's type
        public CartController(IValidatorFactory validatorFactory)
        {
            _cart = new Cart(validatorFactory);
        }

        // add product in cart, if it exist increase qty
        public void AddProductToCart(IProduct product, int quantity)
        {
            _cart.AddItem(product, quantity);
        }

        // increment cart product qty by 1
        public void IncreaseItemQuantity(IProduct product)
        {
            _cart.IncreaseProductQuantity(product);
        }

        // decrement cart product qty by 1
        public void DecreaseItemQuantity(IProduct product)
        {
            _cart.DecreaseProductQuantity(product);
        }

        // remove item by id
        public RemovedProductDto RemoveProductFromCart(int productId)
        {
           var output = _cart.RemoveItem(productId);

            if (output == null)
            {
                throw new InvalidOperationException($"Could not remove item with id: {productId}");
            }

            return new RemovedProductDto(
                        ProductId: productId,
                        Name: output.Name(),
                        Image: output.Image(),
                        Price: output.Price()
            );
        }

        // get all items
        public IReadOnlyList<CartItem> CartItems()
        {
            return _cart.AllItems();
        }

        // get digital items
        public IReadOnlyList<IDigitalProduct> GetDigitalProducts()
        {
            return _cart.GetProductsByType<IDigitalProduct>();
        }
        // get physical items as IProduct
        public IReadOnlyList<IPhysicalProduct> GetPhysicalProducts()
        {
            return _cart.GetProductsByType<IPhysicalProduct>();
        }

        // get physical items with there quantity in cart
        public IEnumerable<(T Product, int Quantity)> GetProductsWithQuantities<T>() where T : IProduct
        {
            return CartItems()
                .Where(item => item.Product is T)
                .Select(item => ((T)item.Product, item.Quantity));
        }

        public decimal CartTotal()
        {
            return _cart.TotalPrice();
        }
    }

    public record RemovedProductDto(int ProductId, string Name, string Image, decimal Price);
}
