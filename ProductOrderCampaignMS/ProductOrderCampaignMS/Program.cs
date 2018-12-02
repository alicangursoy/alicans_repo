using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (args[1] == null)
                {
                    throw new ArgumentNullException();
                }
                string inputFileName = args[1];
                ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
                productOrderCampaignMS.Run(inputFileName);
                Console.WriteLine("Press any key To Exit!");
                Console.ReadKey();
            }
            
            catch(ArgumentNullException argNullEx)
            {
                Console.WriteLine("Input file name should not be null.");
            }
            catch (ArgumentOutOfRangeException argEx)
            {
                Console.WriteLine("You must enter the input file name including filepath.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
            }
        }

        
    }
}
