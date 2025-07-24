using ECommerce.Library.Products.Interfaces;
using ECommerceUI.Products;

namespace ECommerce.Library.Products
{
    public class DigitalProduct : Product, IDigitalProduct
    {
        private string _userEmail;
        private string _downloadLink;
        private double _fileSize;

        public double FileSize { 
            get => _fileSize; 
            private set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("File size cant be 0 or negative");
                }
                else
                {
                    _fileSize = value;
                }
            }
        }
        public DigitalProduct(
            int id,
            string name,
            string imageUrl,
            decimal price,
            string description,
            string userEmail,
            string downloadLink,
            double fileSize
        ) : base(id, name, imageUrl, price, description)
        {
            _userEmail = userEmail;
            _downloadLink = downloadLink;
            _fileSize = fileSize;
        }

        //public double FileSize() => _fileSize;
        // add security in future
        // TODO: add auth and generate hash with product id and user id, generate download link with it which can be opened by owner user
        public string DownloadLink() => _downloadLink;

        public string UserEmail() => _userEmail;

    }
}
