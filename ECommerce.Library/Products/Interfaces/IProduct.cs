namespace ECommerce.Library.Products.Interfaces
{
    public interface IProduct
    {
        int Id();
        decimal Price();
        string Image();
        string Name();
        string Description();
    }

}