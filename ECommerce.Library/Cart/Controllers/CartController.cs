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
        public CartController(IValidatorFactory validatorFactory)
        {
            _cart = new Cart(validatorFactory);
        }

        public void AddProductToCart(IProduct product, int quantity)
        {
            _cart.AddItem(product, quantity);
        }

        // remove item by id
        public IProduct? RemoveProductFromCart(int productId)
        {
           var output = _cart.RemoveItem(productId);

            if (output == null)
            {
                throw new InvalidOperationException($"Could not remove item with id: {productId}");
            }

            return output;
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
        // get physical items
        public IReadOnlyList<IPhysicalProduct> GetPhysicalProducts()
        {
            return _cart.GetProductsByType<IPhysicalProduct>();
        }

        public decimal CartTotal()
        {
            return _cart.TotalPrice();
        }
    }
}
