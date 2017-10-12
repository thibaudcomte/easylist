using EasyList.Proto.Core.Intermarche;
using EasyList.Proto.Core.Storage.LocalStorage;

namespace EasyList.Proto.Core.Retailers.Intermarche
{
    public class Retailer : IRetailer
    {
        public RetailerLocator Locator { get; }
        public RetailerSettings Settings { get; }
        public RetailerShopper Shopper { get; }

        IRetailerLocator IRetailer.Locator => Locator;
        IRetailerSettings IRetailer.Settings => Settings;
        IRetailerShopper IRetailer.Shopper => Shopper;

        public string Name => "Intermarche";

        public Retailer()
        {
            Locator = new RetailerLocator(this);
            Settings = new RetailerSettings(Locator, new LocalStorageReaderWriter(Name));
            Shopper = new RetailerShopper();
        }
    }
}
