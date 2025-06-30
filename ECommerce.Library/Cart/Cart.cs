using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using ECommerceUI.Products;
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
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be higher than 0");
            }
            var existingItem = FindItemByProduct(product);

            // increase quantity if item aldeady in cart
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
            // if success, remove item, decrease item by default 1
            if (item != null)
            {
                _items.Remove(item);
                item.DecreaseQuantity();
                return item.Product;
            }
            return null;
        }

        // only increase quantity of existing item in cart
        public void IncreaseQuantity(IProduct item)
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // increase existing item anoumt by default one
            var existingItem = FindItemByProduct(item);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity();
            }
        }

        // only decrease quantity of existing item in cart
        public void DecreaseQuantity(IProduct item)
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // decrease existing item amoumt by default one
            var existingItem = FindItemByProduct(item);
            if (existingItem == null)
            {
                return; // Item not in cart, nothing to decrease
            }

            existingItem.DecreaseQuantity();

            // if item count is 1 after decrement remove it from cart
            if (existingItem.Quantity == 1)
            {
                RemoveItem(item.Id());
            }
        }

        private CartItem? FindItemByProduct(IProduct product)
        {
            if (product == null)
            {
                throw new ArgumentException("Cannot find product");
            }
            // compare by id and not product refference
            return _items.FirstOrDefault(i => i.Product.Id == product.Id);
        }
    }
}
