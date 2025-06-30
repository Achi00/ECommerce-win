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

        // get all items as IProduct
        public IReadOnlyList<CartItem> AllItems() => _items;

        // return product based on provided generic type

        public IReadOnlyList<T> GetProductsByType<T>() where T : IProduct
        {
            return _items
                .Select(i => i.Product)
                .OfType<T>()
                .ToList();
        }

        public decimal TotalPrice() => _items.Sum(i => i.TotalPrice());

        public void AddItem(IProduct product, int quantity)
        {
            var existingItem = _items.FirstOrDefault(i => i.Product.Equals(product));

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                _items.Add(new CartItem(product, quantity, _validatorFactory));
            }
        }

        public IProduct? RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id() == productId);
            if (item == null)
            {
                return null;
            }
            _items.Remove(item);
            return item.Product;
        }
    }
}
