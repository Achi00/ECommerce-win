namespace ECommerce.Library.Products.Interfaces
{
    public interface IDigitalProduct : IProduct
    {
        string DownloadLink();
        decimal FileSize();
    }
}