using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS
{
    public class ProductOrderCampaignMS
    {
        private int ElapsedTimeInHours { get; set; }
        private string ErrorMessageFromCreatingOrder { get; set; }
        private string ErrorMessageFromCreatingCampaign { get; set; }
        private List<Product> _productList;
        public List<Product> ProductList
        {
            get
            {
                if (this._productList == null)
                {
                    this._productList = new List<Product>();
                }
                return this._productList;
            }
        }
        private List<Order> _orderList;
        public List<Order> OrderList
        {
            get
            {
                if (this._orderList == null)
                {
                    this._orderList = new List<Order>();
                }
                return this._orderList;
            }
        }

        private List<Campaign> _campaignList;
        public List<Campaign> CampaignList
        {
            get
            {
                if (this._campaignList == null)
                {
                    this._campaignList = new List<Campaign>();
                }
                return this._campaignList;
            }
        }

        public void Run(string inputFileName)
        {
            try
            {
                this.ElapsedTimeInHours = 0;
                this.ErrorMessageFromCreatingOrder = string.Empty;
                using (StreamReader fr = new StreamReader(inputFileName))
                {
                    string line = string.Empty;
                    while ((line = fr.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
                    {
                        string[] lineElements = line.Split(' ');
                        if (lineElements == null || lineElements.Length < 2)
                        {
                            throw new IOException("Input string must has two words seperated by space character.");
                        }
                        if (!lineElements[0].Equals("create_product") && !lineElements[0].Equals("get_product_info")
                            && !lineElements[0].Equals("create_order") && !lineElements[0].Equals("create_campaign")
                            && !lineElements[0].Equals("get_campaign_info") && !lineElements[0].Equals("increase_time"))
                        {
                            throw new IOException("Input string was not in a correct format.");
                        }
                        else
                        {
                            switch (lineElements[0])
                            {
                                case "create_product":
                                    if (lineElements.Length != 4)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: create_product PRODUCTCODE PRICE STOCK");
                                    }
                                    Product product = this.CreateProduct(lineElements[1], Convert.ToDouble(lineElements[2]), Convert.ToInt32(lineElements[3]));
                                    this.ProductList.Add(product);
                                    Console.WriteLine($"Product created; code {product.ProductCode}, price {product.Price.ToString("#.##").Replace(',','.')}, stock {product.Stock}");
                                    break;
                                case "get_product_info":
                                    if (lineElements.Length != 2)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: get_product_info PRODUCTCODE");
                                    }
                                    Product foundProduct = this.GetProductInfo(this.ProductList, lineElements[1]);
                                    if (foundProduct != null && !foundProduct.IsNull)
                                    {
                                        Console.WriteLine($"Product {foundProduct.ProductCode} info; price {foundProduct.Price.ToString("#.##").Replace(',','.')}, stock {foundProduct.Stock}");
                                    }
                                    break;
                                case "create_order":
                                    if (lineElements.Length != 3)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: create_order PRODUCTCODE QUANTITY");
                                    }
                                    Order order = this.CreateOrder(lineElements[1], Convert.ToInt32(lineElements[2]));
                                    if (order == null)
                                    {
                                        Console.WriteLine($"Order cannot be created. {this.ErrorMessageFromCreatingOrder}");
                                        break;
                                    }
                                    this.OrderList.Add(order);
                                    Console.WriteLine($"Order created; product {order.ProductCode}, quantity {order.Quantity}");
                                    break;
                                case "create_campaign":
                                    if (lineElements.Length != 6)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: create_campaign NAME PRODUCTCODE DURATION PMLIMIT TARGETSALESCOUNT");
                                    }
                                    Campaign campaign = this.CreateCampaign(lineElements[1], lineElements[2], Convert.ToInt32(lineElements[3]), Convert.ToInt32(lineElements[4]), Convert.ToInt32(lineElements[5]));
                                    if (campaign == null)
                                    {
                                        Console.WriteLine(this.ErrorMessageFromCreatingCampaign);
                                        break;
                                    }
                                    this.CampaignList.Add(campaign);
                                    Console.WriteLine($"Campaign created; name {campaign.Name}, " 
                                        + $"product {campaign.ProductCode}, duration {campaign.Duration}, "
                                        +$"limit {campaign.PriceManipulationLimit}, target sales count {campaign.TargetSalesCount}");
                                    break;
                                case "get_campaign_info":
                                    if (lineElements.Length != 2)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: get_campaign_info NAME");
                                    }
                                    Campaign foundCampaign = this.GetCampaignInfo(this.CampaignList, lineElements[1]);
                                    if (foundCampaign != null && !foundCampaign.IsNull)
                                    {
                                        
                                        double turnOver = GetTurnoverOfProductInCampaign(foundCampaign.ProductCode);
                                        string turnOverStr = turnOver > 0 ? turnOver.ToString("#.##").Replace(',', '.')
                                            : "0";
                                        double averageItemPrice = GetAverageItemPrice(foundCampaign.ProductCode);
                                        string averageItemPriceStr = averageItemPrice > 0 ? averageItemPrice.ToString("#.##").Replace(',', '.')
                                            : "-";
                                        Console.WriteLine($"Campaign {foundCampaign.Name} info; " +
                                            $"Status {foundCampaign.CheckCampaignExpiration(this.ElapsedTimeInHours)}," +
                                            $"Target Sales {foundCampaign.TargetSalesCount}, " +
                                            $"Total Sales {GetTotalSalesOfProductInCampaign(foundCampaign.ProductCode)}, " +
                                            $"Turnover {turnOverStr}, " +
                                            $"Average Item Price {averageItemPriceStr}");
                                    }
                                    break;
                                case "increase_time":
                                    if (lineElements.Length != 2)
                                    {
                                        throw new IOException("Input string was not in a correct format. Correct format is: increase_time HOUR");
                                    }
                                    this.ElapsedTimeInHours = IncreaseTime(this.ElapsedTimeInHours, Convert.ToInt32(lineElements[1])) % 24; // the hour must be in the range 0..23
                                    ManipulateProductPrice(this.ElapsedTimeInHours);
                                    Console.WriteLine($"Time is : {this.ElapsedTimeInHours.ToString().PadLeft(2, '0')}:00");
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"{ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public void ManipulateProductPrice(int timeIncreaseCount)
        {
            try
            {
                foreach (var item in this.CampaignList)
                {
                    Product p = GetProductInfo(this.ProductList, item.ProductCode);
                    if (!p.IsNull)
                    {
                        if (item.PriceManipulationLimit == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        timeIncreaseCount = timeIncreaseCount * 2 >= item.PriceManipulationLimit
                            ? item.PriceManipulationLimit : timeIncreaseCount;
                        if (timeIncreaseCount % 2 == 0)
                        {
                            p.Price += ((double)timeIncreaseCount / item.PriceManipulationLimit) * p.Price;
                        }
                        else
                        {
                            p.Price -= ((double)timeIncreaseCount / item.PriceManipulationLimit) * p.Price;
                        }
                    }
                    
                }
            }
            catch(DivideByZeroException divideByZeroEx)
            {
                Console.WriteLine("PriceManipulationLimit is zero. Zero is forbidden for price manipulation.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured in ManipulateProductPrice method. Error message: {ex.Message}");
                throw;
            }
        }

        public Product CreateProduct(string productCode, double price, int stock)
        {
            try
            {
                Product product = new Product();
                product.ProductCode = productCode;
                product.Price = price;
                product.Stock = stock;
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public Product GetProductInfo(List<Product> productList, string productCode)
        {
            try
            {
                Product product = productList.First(p => p.ProductCode == productCode);
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public Order CreateOrder(string productCode, int quantity)
        {
            try
            {
                Product foundProduct = this.ProductList.Find(p => p.ProductCode == productCode && p.Stock >= quantity);
                Campaign foundCampaign = this.CampaignList.Find(c => c.ProductCode == productCode && c.CheckCampaignExpiration(this.ElapsedTimeInHours) == "Active");
                if (foundProduct == null)
                {
                    this.ErrorMessageFromCreatingOrder = "The product that you want to order is not found in the stock.";
                    return null;
                }
                if (foundCampaign == null)
                {
                    this.ErrorMessageFromCreatingOrder = "The campaign that you want to take advantage of is ended.";
                    return null;
                }
                Order order = new Order();
                order.ProductCode = productCode;
                order.Quantity = quantity;
                order.ProductCurrentPrice = this.ProductList.Find(p => p.ProductCode == productCode).Price;
                foundProduct.Stock -= order.Quantity;
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public Campaign CreateCampaign(string name, string productCode, int duration, int pmLimit, int targetSalesCount)
        {
            try
            {
                if (this.ProductList.Count <= 0 || !this.ProductList.Exists(p => p.ProductCode == productCode))
                {
                    this.ErrorMessageFromCreatingCampaign = $"Campaign cannot be created. There is no product named {productCode}";
                    return null;
                }
                Campaign campaign = new Campaign();
                campaign.Name = name;
                campaign.ProductCode = productCode;
                campaign.Duration = duration;
                campaign.PriceManipulationLimit = pmLimit;
                campaign.TargetSalesCount = targetSalesCount;
                return campaign;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public Campaign GetCampaignInfo(List<Campaign> campaignList, string name)
        {
            try
            {
                Campaign campaign = campaignList.First(c => c.Name == name);
                return campaign;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public int IncreaseTime(int currentTime, int hour)
        {
            try
            {
                return currentTime + hour;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public int GetTotalSalesOfProductInCampaign(string campaignProductCode)
        {
            try
            {
                if (this.OrderList.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return this.OrderList.FindAll(o => o.ProductCode == campaignProductCode).Sum(or => or.Quantity);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public double GetTurnoverOfProductInCampaign(string campaignProductCode)
        {
            try
            {
                if (this.OrderList.Count == 0)
                {
                    return 0.00;
                }
                else
                {
                    return this.OrderList.FindAll(o => o.ProductCode == campaignProductCode).Sum(o => o.Turnover);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured. Error message: {ex.Message}");
                throw;
            }
        }

        public double GetAverageItemPrice(string productCode)
        {
            try
            {
                if (this.OrderList.Count == 0)
                {
                    return 0.00;
                }
                if (this.OrderList.FindAll(o => o.ProductCode == productCode).Sum(or => or.Quantity) == 0.00)
                {
                    return 0.00;
                }
                return this.OrderList.FindAll(o => o.ProductCode == productCode).Sum(or => or.Turnover)
                    / this.OrderList.FindAll(o => o.ProductCode == productCode).Sum(or => or.Quantity);
            }
            catch (DivideByZeroException zeroDivisionEx)
            {
                Console.WriteLine($"Total quantity is 0. Division by zero is forbidden. Error message: {zeroDivisionEx.Message}");
                throw zeroDivisionEx;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured in GetAverageItemPrice method. Error message: {ex.Message}");
                throw ex;
            }
        }

    }
}
