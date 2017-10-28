namespace EasyList.Proto.Core.Retailers
{
    public interface IStore
    {
        string Name { get; }
        string Address { get; }
        string City { get; }
        string ZipCode { get; }
        double Latitude { get; }
        double Longitude { get; }
        IRetailer Retailer { get; }
        string CartUrl { get; }
    }
}
