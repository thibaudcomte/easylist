﻿using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Retailers.Intermarche
{
    public abstract class RetailerSettingsBase : RetailerSettingsBase<Store>
    {
        public Retailer Retailer { get; internal set; }
    }
}
