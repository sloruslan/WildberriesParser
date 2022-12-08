using Newtonsoft.Json.Linq;
using Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wildberries.Shared.Domain.Entity;

namespace TelegramBot
{
    public static class Parsing
    {
        public static CardEntity ParseData(CardEntity cardEntity)
        {
            var proxy = new WebProxy("127.0.0.1:8888");
            var cookieContainer = new CookieContainer();

            var getRequest = new GetRequest($"https://card.wb.ru/cards/detail?spp=0&regions=80,64,83,4,38,33,70,82,69,68,86,30,40,48,1,22,66,31&pricemarginCoeff=1.0&reg=0&appType=1&emp=0&locale=ru&lang=ru&curr=rub&couponsGeo=2,12,7,3,6,18,21&dest=-1029256,-72223,-1646090,-2307160&nm={cardEntity.Article}");
            getRequest.Accept = "*/*";
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
            getRequest.Referer = "https://www.wildberries.ru/catalog/0/search.aspx?search=65844223";


            getRequest.Headers.Add("Origin", "https://www.wildberries.ru");
            getRequest.Headers.Add("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"");
            getRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            getRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            getRequest.Headers.Add("Sec-Fetch-Dest", "empty");
            getRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            getRequest.Headers.Add("Sec-Fetch-Site", "cross-site");
            getRequest.Host = "card.wb.ru";
            //getRequest.Proxy = proxy;

            getRequest.Run(cookieContainer);

            var json = JObject.Parse(getRequest.Response);
            var data = json["data"];
            if (data != null)
            {
                var products = data["products"];
                if (products != null)
                {
                    var name = products.First()["name"];
                    var mainPrice = Convert.ToInt32(products.First()["priceU"]);
                    var salePrice = Convert.ToInt32(products.First()["salePriceU"]);
                    var averagePrice = Convert.ToInt32(products.First()["averagePrice"]);


                    cardEntity.Name = name.ToString();
                    cardEntity.MainPrice = mainPrice;
                    cardEntity.AveragePrice = averagePrice;
                    cardEntity.SalePrice.Add(salePrice);
                    cardEntity.Time.Add(DateTimeOffset.UtcNow);
                    cardEntity.Url = $"https://www.wildberries.ru/catalog/{cardEntity.Article}/detail.aspx?targetUrl=XS";
                }

            }

            return cardEntity;
        }
    }
}
