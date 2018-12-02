using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS
{
    public class Product
    {
        public string ProductCode { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool IsNull
        {
            get
            {
                return !(!string.IsNullOrWhiteSpace(this.ProductCode) && this.Price >= 0.00 && this.Stock >= 0);
            }
        }
        public bool IsExistsInStock(int orderCount)
        {
            try
            {
                return this.Stock >= orderCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }


    }
}
