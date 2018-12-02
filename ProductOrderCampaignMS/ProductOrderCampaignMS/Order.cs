using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS
{
    public class Order
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public double ProductCurrentPrice { get; set; }
        public double Turnover {
            get
            {
                return this.Quantity * this.ProductCurrentPrice;
            }
        }
        public bool IsNull {
            get
            {
                return !(!string.IsNullOrWhiteSpace(this.ProductCode) && this.Quantity >= 0 && this.ProductCurrentPrice > 0);
            }
        }
    }
}
