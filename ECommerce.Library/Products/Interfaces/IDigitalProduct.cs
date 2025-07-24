namespace ECommerce.Library.Products.Interfaces
{
    public interface IDigitalProduct : IProduct
    {
        double FileSize { get; }
        string DownloadLink();
        string UserEmail();
    }
}