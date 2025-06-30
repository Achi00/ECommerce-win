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

        public void RemoveProductFromCart(IProduct product)
        {
            _cart.RemoveItem(product);
        }

        public IReadOnlyList<CartItem> CartItems()
        {
            return _cart.AllItems();
        }

        public decimal CartTotal()
        {
            return _cart.TotalPrice();
        }
    }
}
