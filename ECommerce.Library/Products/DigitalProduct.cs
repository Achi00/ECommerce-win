using ECommerce.Library.Products.Interfaces;
using ECommerceUI.Products;

namespace ECommerce.Library.Products
{
    public class DigitalProduct : Product, IDigitalProduct
    {
        private string _userEmail;
        private string _downloadLink;
        private decimal _fileSize;
        public DigitalProduct(
            int id,
            string name,
            string imageUrl,
            decimal price,
            string description,
            string userEmail,
            string downloadLink,
            decimal fileSize
        ) : base(id, name, imageUrl, price, description)
        {
            _userEmail = userEmail;
            _downloadLink = downloadLink;
            _fileSize = fileSize;
        }

        public decimal FileSize() => _fileSize;
        // add security in future
        // TODO: add auth and generate hash with product id and user id, generate download link with it which can be opened by owner user
        public string DownloadLink() => _downloadLink;

    }
}
