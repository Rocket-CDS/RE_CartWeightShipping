using DNNrocketAPI.Components;
using RocketEcommerce.Components;
using RocketEcommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketEcommerce.RE_CartWeightShipping
{
    public class ShippingCalcInterface : ShippingInterface
    {
        private ShipData GetShipData()
        {
            var shipData = new ShipData(PortalUtils.GetPortalId());
            return shipData;
        }
        public override bool Active()
        {
            return GetShipData().Active;
        }

        public override int CalculateShippingCost(CartLimpet cartData)
        {
            var weight = 0;
            var l = cartData.CartItemList;
            foreach (var p in l)
            {
                weight += p.GetSelectedModel().Weight;
            }
            return CalcCost(weight);
        }

        public override int CalculateShippingCost(OrderLimpet orderData)
        {
            var weight = 0;
            var l = orderData.GetCartItemList();
            foreach (var p in l)
            {
                weight += p.GetSelectedModel().Weight;
            }
            return CalcCost(weight);
        }

        private int CalcCost(int weight)
        {
            var shipData = new ShipData(PortalUtils.GetPortalId());
            var cost = shipData.Info.GetXmlPropertyInt("genxml/textbox/defaultcost");
            var datalist = shipData.Info.GetList("range");
            foreach (var rangeInfo in datalist)
            {
                var lowrange = rangeInfo.GetXmlPropertyInt("genxml/textbox/lowrange");
                var highrange = rangeInfo.GetXmlPropertyInt("genxml/textbox/highrange");
                var rangecost = rangeInfo.GetXmlPropertyInt("genxml/textbox/cost");
                if (weight >= lowrange && weight < highrange) cost = rangecost;
            }
            return cost;
        }

        public override string Msg()
        {
            return GetShipData().Msg;
        }

        public override string SelectText()
        {
            return GetShipData().SelectText;
        }

        public override string ShipProvKey()
        {
            return GetShipData().InterfaceKey;
        }

    }
}
