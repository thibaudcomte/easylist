using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Retailers.Intermarche
{
    public class Retailer : IRetailer
    {
        public RetailerLocator Locator { get; }
        public RetailerSettingsBase<Retailer, Store> Settings { get; }
        public RetailerShopper Shopper { get; }

        IRetailerLocator IRetailer.Locator => Locator;
        IRetailerSettings IRetailer.Settings => Settings;
        IRetailerShopper IRetailer.Shopper => Shopper;

        public string Name => "Intermarche";

        public Retailer(RetailerSettingsBase<Retailer, Store> retailerSettings)
        {
            Locator = new RetailerLocator(this);
            Settings = retailerSettings;
            Settings.Retailer = this;
            Shopper = new RetailerShopper();
        }
    }
}
