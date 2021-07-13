using DNNrocketAPI;
using DNNrocketAPI.Components;
using RocketEcommerce.Components;
using Simplisity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketEcommerce.RE_CartWeightShipping
{
    public class ShipData
    {
        private const string _entityTypeCode = "cartweightship";
        private const string _tableName = "RocketEcommerce";
        private string _guidKey;
        private DNNrocketController _objCtrl;
        public ShipData(string siteGuid)
        {
            var portalid = PortalUtils.GetPortalIdBySiteKey(siteGuid);
            PortalShop = new PortalShopLimpet(portalid, DNNrocketUtils.GetCurrentCulture());
            _guidKey = siteGuid + "_rocketecommerce_" + _entityTypeCode;
            _objCtrl = new DNNrocketController();
            Info = _objCtrl.GetByGuidKey(portalid,-1, _entityTypeCode, _guidKey, "", _tableName);
            if (Info == null)
            {
                var portalId = PortalUtils.GetPortalIdBySiteKey(siteGuid);
                Info = new SimplisityInfo();
                Info.TypeCode = _entityTypeCode;
                Info.GUIDKey = _guidKey;
                Info.PortalId = portalId;
            }
        }
        public void Save(SimplisityInfo postInfo)
        {
            Info.XMLData = postInfo.XMLData;
            ValidateAndUpdate();
            LogUtils.LogTracking("Save - UserId: " + UserUtils.GetCurrentUserId() + " " + postInfo.XMLData, "cartweightship");
        }
        public int ValidateAndUpdate()
        {
            Validate();
            return Update();
        }
        public void Validate()
        {
            var lp = 1;
            var costList = Info.GetList("range");
            foreach (var cost in costList)
            {
                Info.SetXmlPropertyInt("genxml/range/genxml[" + lp + "]/textbox/cost", PortalShop.CurrencyConvertCents(cost.GetXmlProperty("genxml/textbox/cost")).ToString());
                lp += 1;
            }
        }

        public int Update()
        {
            return _objCtrl.Update(Info, _tableName);
        }
        public void Delete()
        {
            if (Info.ItemID > 0)
            {
                _objCtrl.Delete(Info.ItemID);
                LogUtils.LogTracking("DELETE - UserId: " + UserUtils.GetCurrentUserId() + " " + Info.ItemID, "cartweightship");
            }
        }
        public bool DebugMode
        {
            get { return Info.GetXmlPropertyBool("genxml/checkbox/debugmode"); }
            set { Info.SetXmlProperty("genxml/checkbox/debugmode", value.ToString()); }
        }
        public bool Active
        {
            get { return Info.GetXmlPropertyBool("genxml/checkbox/active"); }
            set { Info.SetXmlProperty("genxml/checkbox/active", value.ToString()); }
        }
        public PortalShopLimpet PortalShop { get; set; }
        public SimplisityInfo Info { get; set; }


    }
}
