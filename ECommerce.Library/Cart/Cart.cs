using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Cart
{
    public class Cart
    {
        private readonly List<CartItem> _items = new();
        private readonly IValidatorFactory _validatorFactory;

        public Cart(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public IReadOnlyList<CartItem> AllItems() => _items.AsReadOnly();

        public decimal TotalPrice() => _items.Sum(i => i.TotalPrice());

        public void AddItem(IProduct product, int quantity)
        {

            // check if item exists
            var existingItem = _items.FirstOrDefault(i => i.Product.Equals(product));

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = new CartItem(product, quantity, _validatorFactory);
                _items.Add(newItem);
            }
        }

        public void RemoveItem(IProduct product)
        {

        }

        
    }
}
