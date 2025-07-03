using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;

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
                existingItem.AddQuantity(quantity);
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
                item.RemoveQuantity();
                return item.Product;
            }
            return null;
        }

        // only increase quantity of existing item in cart
        public void IncreaseProductQuantity(IProduct item)
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // increase existing item anoumt by default one
            var existingItem = FindItemByProduct(item);
            if (existingItem != null)
            {
                existingItem.AddQuantity();
            }
        }

        // only decrease quantity of existing item in cart
        public void DecreaseProductQuantity(IProduct item)
        {
            if (item is null)
            {
                throw new ArgumentException("Product is missing");
            }
            // decrease existing item amoumt by default one
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
            existingItem.RemoveQuantity();

            
        }


        // fint product in list by id
        // not by refference!!! will cause problems
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
