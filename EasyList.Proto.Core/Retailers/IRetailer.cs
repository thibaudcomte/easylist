namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailer
    {
        string Name { get; }
        IRetailerLocator Locator { get; }
        IRetailerSettings Settings { get; }
        IRetailerShopper Shopper { get; }
    }
}
