using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductOrderCampaignMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderCampaignMS.Tests
{
    [TestClass()]
    public class ProductOrderCampaignMSTests
    {
        [TestMethod()]
        public void ManipulateProductPriceTest()
        {
            //arrange
            ProductOrderCampaignMS prodOrderCampMS = new ProductOrderCampaignMS();
            Product product = new Product();
            product.Price = 100;
            product.ProductCode = "P1";
            product.Stock = 560;
            prodOrderCampMS.ProductList.Add(product);

            int elapsedTimeInHours = 3;

            Campaign campaign = new Campaign();
            campaign.ProductCode = "P1";
            campaign.PriceManipulationLimit = 15;
            prodOrderCampMS.CampaignList.Add(campaign);

            double expectedPrice = 80.0;

            //act
            prodOrderCampMS.ManipulateProductPrice(elapsedTimeInHours);

            //assert
            Assert.AreEqual(expectedPrice, product.Price);
        }

        [TestMethod()]
        public void CreateProductTest()
        {
            //arrange
            Product expectedProduct = new Product();
            expectedProduct.ProductCode = "P1";
            expectedProduct.Price = 100.0;
            expectedProduct.Stock = 240;

            ProductOrderCampaignMS prodOrderCampMS = new ProductOrderCampaignMS();
            string productCode = "P1";
            double price = 100.0;
            int stock = 240;

            //act
            Product createdProduct = prodOrderCampMS.CreateProduct(productCode, price, stock);

            //assert
            Assert.AreEqual(expectedProduct.ProductCode, createdProduct.ProductCode);
            Assert.AreEqual(expectedProduct.Price, createdProduct.Price);
            Assert.AreEqual(expectedProduct.Stock, createdProduct.Stock);
            Assert.AreEqual(expectedProduct.IsNull, createdProduct.IsNull);
        }

        [TestMethod()]
        public void GetProductInfoTest()
        {
            //arrange
            Product expectedProduct = new Product();
            expectedProduct.ProductCode = "P3";
            expectedProduct.Price = 87.6;
            expectedProduct.Stock = 43;

            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            string productCode = "P3";
            double price = 87.6;
            int stock = 43;
            productOrderCampaignMS.ProductList.Add(new Product() { ProductCode = productCode, Price = price, Stock = stock });

            //act
            Product foundProduct = productOrderCampaignMS.GetProductInfo(productOrderCampaignMS.ProductList, "P3");

            //assert
            Assert.AreEqual(expectedProduct.ProductCode, foundProduct.ProductCode);
            Assert.AreEqual(expectedProduct.Price, foundProduct.Price);
            Assert.AreEqual(expectedProduct.Stock, foundProduct.Stock);
            Assert.AreEqual(expectedProduct.IsNull, foundProduct.IsNull);
        }

        [TestMethod()]
        public void CreateOrderTest()
        {
            //arrange
            Order expectedOrder = new Order();
            expectedOrder.ProductCode = "P5";
            expectedOrder.ProductCurrentPrice = 87.6;
            expectedOrder.Quantity = 10;

            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.ProductList.Add(new Product() { ProductCode = "P5", Price = 87.6, Stock = 10 });
            productOrderCampaignMS.CampaignList.Add(new Campaign() { Duration = 12, Name = "C1", PriceManipulationLimit = 18, ProductCode = "P5", TargetSalesCount = 100 });
            //act

            Order createdOrder = productOrderCampaignMS.CreateOrder("P5", 10);

            //assert
            Assert.AreEqual(expectedOrder.ProductCode, createdOrder.ProductCode);
            Assert.AreEqual(expectedOrder.ProductCurrentPrice, createdOrder.ProductCurrentPrice);
            Assert.AreEqual(expectedOrder.Quantity, createdOrder.Quantity);
            Assert.AreEqual(expectedOrder.Turnover, createdOrder.Turnover);
            Assert.AreEqual(expectedOrder.IsNull, createdOrder.IsNull);
        }

        [TestMethod()]
        public void CreateCampaignTest()
        {
            //arrange
            Campaign expectedCampaign = new Campaign();
            expectedCampaign.Name = "C3";
            expectedCampaign.PriceManipulationLimit = 16;
            expectedCampaign.ProductCode = "P8";
            expectedCampaign.TargetSalesCount = 65;
            expectedCampaign.Duration = 20;

            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.ProductList.Add(new Product() { ProductCode = "P8", Price = 100.0, Stock = 60 });

            //act
            Campaign createdCampaign = productOrderCampaignMS.CreateCampaign("C3", "P8", 20, 16, 65);

            //assert
            Assert.AreEqual(expectedCampaign.Name, createdCampaign.Name);
            Assert.AreEqual(expectedCampaign.PriceManipulationLimit, createdCampaign.PriceManipulationLimit);
            Assert.AreEqual(expectedCampaign.ProductCode, createdCampaign.ProductCode);
            Assert.AreEqual(expectedCampaign.TargetSalesCount, createdCampaign.TargetSalesCount);
            Assert.AreEqual(expectedCampaign.Duration, createdCampaign.Duration);
            Assert.AreEqual(expectedCampaign.IsNull, createdCampaign.IsNull);
        }

        [TestMethod()]
        public void GetCampaignInfoTest()
        {
            //arrange
            Campaign expectedCampaign = new Campaign();
            expectedCampaign.Name = "C2";
            expectedCampaign.PriceManipulationLimit = 20;
            expectedCampaign.ProductCode = "P2";
            expectedCampaign.TargetSalesCount = 100;
            expectedCampaign.Duration = 34;

            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.ProductList.Add(new Product() { Price = 100, ProductCode = "P2", Stock = 130 });
            productOrderCampaignMS.CampaignList.Add(new Campaign() { Duration = 34, Name = "C2", PriceManipulationLimit = 20, ProductCode = "P2", TargetSalesCount = 100 });

            //act
            Campaign foundCampaign = productOrderCampaignMS.GetCampaignInfo(productOrderCampaignMS.CampaignList, "C2");

            //assert
            Assert.AreEqual(expectedCampaign.Name, foundCampaign.Name);
            Assert.AreEqual(expectedCampaign.PriceManipulationLimit, foundCampaign.PriceManipulationLimit);
            Assert.AreEqual(expectedCampaign.ProductCode, foundCampaign.ProductCode);
            Assert.AreEqual(expectedCampaign.TargetSalesCount, foundCampaign.TargetSalesCount);
            Assert.AreEqual(expectedCampaign.Duration, foundCampaign.Duration);
            Assert.AreEqual(expectedCampaign.IsNull, foundCampaign.IsNull);
        }

        [TestMethod()]
        public void IncreaseTimeTest()
        {
            //arrange
            int currentTime = 5;
            int expectedTime = 12;
            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();

            //act
            int foundTime = productOrderCampaignMS.IncreaseTime(currentTime, 7);

            //assert
            Assert.AreEqual(expectedTime, foundTime);
        }

        [TestMethod()]
        public void GetTotalSalesOfProductInCampaignTest()
        {
            //arrange
            int expectedTotalSales = 10;
            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.OrderList.Add(new Order() { ProductCode = "P7", ProductCurrentPrice = 100, Quantity = 10 });

            //act
            int foundTotalSales = productOrderCampaignMS.GetTotalSalesOfProductInCampaign("P7");

            //assert
            Assert.AreEqual(expectedTotalSales, foundTotalSales);
        }

        [TestMethod()]
        public void GetTurnoverOfProductInCampaignTest()
        {
            //arrange
            int quantity = 14;
            double price = 90;
            double expectedTurnover = price * (double)quantity;
            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.OrderList.Add(new Order() { ProductCode = "P11", ProductCurrentPrice = 90, Quantity = 14 });


            //act
            double foundTurnover = productOrderCampaignMS.GetTurnoverOfProductInCampaign("P11");
            
            //assert
            Assert.AreEqual(expectedTurnover, foundTurnover);
        }

        [TestMethod()]
        public void GetAverageItemPriceTest()
        {
            //arrange
            int quantity1 = 5;
            int quantity2 = 8;
            double price1 = 76;
            double price2 = 60;
            double expectedAverageItemPrice = ((double)quantity1 * price1 + (double)quantity2 * price2) / (quantity1 + quantity2);
            ProductOrderCampaignMS productOrderCampaignMS = new ProductOrderCampaignMS();
            productOrderCampaignMS.OrderList.Add(new Order() { ProductCode = "P6", ProductCurrentPrice = 76, Quantity = 5 });
            productOrderCampaignMS.OrderList.Add(new Order() { ProductCode = "P6", ProductCurrentPrice = 60, Quantity = 8 });

            //act
            double foundAverageItemPrice = productOrderCampaignMS.GetAverageItemPrice("P6");

            //assert
            Assert.AreEqual(expectedAverageItemPrice, foundAverageItemPrice);
        }
    }
}