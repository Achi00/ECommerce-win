using ECommerce.Library.Products.Interfaces;
using ECommerceUI.Products;

namespace ECommerce.Library.Products
{
    public class PhysicalProduct : Product, IPhysicalProduct
    {
        private int _stock;

        private double _weight;
        public double WeightInKg 
        { 
            get => _weight;  
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Product weight cannot be negative", nameof(value));
                }
                else
                {
                    _weight = value;
                }
            }
        }


        public PhysicalProduct(int id, string name, string imageUrl, decimal price, string description, int stock, double weightInKg) : base(id, name, imageUrl, price, description)
        {
            _stock = stock;
            WeightInKg = weightInKg;
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
