
using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Retailers.Intermarche;
using EasyList.Proto.Core.Shopping;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EasyList.Proto.Retailers.Intermarche
{
    public class RetailerShopper : IRetailerShopper
    {
        public IRetailerShoppingSession CreateRetailerShoppingSession(IStore store)
        {
            return new RetailerShoppingSession(store as Store);
        }
    }
}
