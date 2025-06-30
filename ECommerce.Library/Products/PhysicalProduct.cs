using ECommerce.Library.Products.Interfaces;
using ECommerceUI.Products;

namespace ECommerce.Library.Products
{
    public class PhysicalProduct : Product, IPhysicalProduct
    {
        private int _stock;

        public PhysicalProduct(int id, string name, string imageUrl, decimal price, string description, int stock) : base(id, name, imageUrl, price, description)
        {
            _stock = stock;
        }

        public int Stock() => _stock;


        //public void DecreaseStock(int quantity)
        //{
        //    if (quantity <= 0)
        //    {
        //        throw new ArgumentException("Quantity can't be zero or negative number");
        //    }
        //    if (quantity > _stock)
        //    {
        //        throw new InvalidOperationException("Not enought stock");
        //    }
        //}
    }
}
