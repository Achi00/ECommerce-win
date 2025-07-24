using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ECommerce.Library.Cart
{
    public class Cart
    {
        // holds cart items
        private readonly List<CartItem> _items = new();
        // validates item based on it type, e.g: PhysicalProduct, DigitalProduct... ect.
        private readonly IValidatorFactory _validatorFactory;

        public Cart(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        // get all items as IProduct readonly list
        public IReadOnlyList<CartItem> AllItems() => _items;

        // return product based on provided generic type
        public IReadOnlyList<T> GetProductsByType<T>() where T : IProduct
        {
            return _items
                .Select(i => i.Product)
                .OfType<T>()
                .ToList();
        }

        // returns product which can be added in cart by quantity
        public IEnumerable<(T Product, int Quantity)> GetProductsWithQuantities<T>() where T : IPhysicalProduct
        {
            return _items
                .Where(item => item.Product is T)
                .Select(item => ((T)item.Product, item.Quantity));
        }

        public decimal TotalPrice() => _items.Sum(i => i.TotalPrice());

        public void AddItem(IProduct product, int quantity, [CallerMemberName] string source = "")
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be higher than 0");
            }
            // finds item in list by id
            var existingItem = FindItemByProduct(product);

            // increase quantity if item is already in cart
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = new CartItem(product, quantity, _validatorFactory, source);
                _items.Add(newItem);
            }
        }

        // removes item fully from cart's list
        public IProduct? RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id() == productId);
            // if success, remove item, decrease item by default 1
            if (item != null)
            {
                _items.Remove(item);
                return item.Product;
            }
            return null;
        }

        // only increase quantity of existing item in cart
        public void IncreaseProductQuantity(IProduct item, [CallerMemberName] string source = "")
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // increase existing item amount by default one
            var existingItem = FindItemByProduct(item);
            if (existingItem != null)
            {
                int newPotentialQuantity = existingItem.Quantity + 1;

                _validatorFactory.ValidateProduct(item, newPotentialQuantity, source);

                existingItem.IncreaseQuantity();
            }
            else
            {
                _validatorFactory.ValidateProduct(item, 1, source);
            }
        }

        // only decrease quantity of existing item in cart
        public void DecreaseProductQuantity(IProduct item)
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // decrease existing item amount by default one
            var existingItem = FindItemByProduct(item);
            if (existingItem == null)
            {
                // item not in cart, nothing to decrease
                return; 
            }

            // if only 1 item left, remove whole item instead of decrement
            if (existingItem.Quantity <= 1)
            {
                RemoveItem(item.Id());
                return;
            }

            // decrease product quantity by one
            existingItem.DecreaseQuantity();
        }


        // find product in list by id
        // not by reference!!! will cause problems
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
