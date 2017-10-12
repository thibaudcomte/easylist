namespace EasyList.Proto.Core.Retailers
{
    public interface IStore
    {
        int Id { get; }
        string Name { get; }
        string Address { get; }
        string City { get; }
        string ZipCode { get; }
        IRetailer Retailer { get; }
        string CartUrl { get; }
    }
}
