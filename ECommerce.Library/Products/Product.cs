using ECommerce.Library.Products.Interfaces;

namespace ECommerceUI.Products
{
    public abstract class Product : IProduct
    {
        private int _id;
        private string _name;
        private string _imageUrl;
        private decimal _price;
        private string _description;

        protected Product(int id, string name, string imageUrl, decimal price, string description)
        {
            _id = id;
            _name = name;
            _imageUrl = imageUrl;
            _price = price;
            _description = description;
        }

        public int Id() => _id;

        public string Name() => _name;

        public string Image() => _imageUrl;

        public string Description() => _description;

        // in case of discount override
        public virtual decimal Price() => _price;
    }
}
