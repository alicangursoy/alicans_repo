using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS
{
    public class Campaign
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
        public string CheckCampaignExpiration(int elapsedTimeInHours)
        {
            try
            {
                return this.Duration <= elapsedTimeInHours ? "Ended" : "Active";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public bool IsNull
        {
            get
            {
                return !(!string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.ProductCode)
                    && this.Duration >= 0 && this.PriceManipulationLimit >= 0 && this.TargetSalesCount >= 0);
            }
        }
    }
}
