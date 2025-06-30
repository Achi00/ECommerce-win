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

        // add product in cart, if it exist increase qty
        public void AddProductToCart(IProduct product, int quantity)
        {
            _cart.AddItem(product, quantity);
        }

        // increment cart product qty by 1
        public void IncreaseItemQuantity(IProduct product)
        {
            _cart.IncreaseQuantity(product);
        }

        // decrement cart product qty by 1
        public void DecreaseItemQuantity(IProduct product)
        {
            _cart.DecreaseQuantity(product);
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
