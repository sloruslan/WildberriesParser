using BlablacarApi;
using Newtonsoft.Json.Linq;
using Server.Infrastructure;
using System.Net;
using Wildberries;




//var code = "25586868";
//var code = Console.ReadLine();
var code = 25586868;
while (true)
{
    code++;
    try
    {
        using (var db = new CardContext())
        {
            Task.Delay(10);
            var proxy = new WebProxy("127.0.0.1:8888");
            var cookieContainer = new CookieContainer();

            var postRequest = new PostRequest("https://www.wildberries.ru/webapi/stats/pageview");

            postRequest.Data = $"{{\"statusCode\":200,\"path\":\"/catalog/0/search.aspx\",\"queryString\":\"?search={code}\",\"urlReferrer\":\"https://www.google.com/\"}}";
            postRequest.Accept = "*/*";
            postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
            postRequest.ContentType = "application/json";
            postRequest.Referer = "https://www.wildberries.ru/catalog/0/search.aspx?search=25586868";
            postRequest.Host = "www.wildberries.ru";
            postRequest.Proxy = proxy;

            postRequest.Headers.Add("Origin", "https://www.wildberries.ru");
            postRequest.Headers.Add("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"");
            postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
            postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            postRequest.Headers.Add("Sec-Fetch-Site", "same-origin");
            postRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            postRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9");


            postRequest.Run(cookieContainer);

            //var strStart = postRequest.Response.IndexOf("google-site-verification");
            //strStart = postRequest.Response.IndexOf("content=\"", strStart) + 9;

            //var strEnd = postRequest.Response.IndexOf("\"", strStart);
            //var getPath = postRequest.Response.Substring(strStart, strEnd - strStart);

            var getRequest = new GetRequest($"https://card.wb.ru/cards/detail?spp=0&regions=80,64,83,4,38,33,70,82,69,68,86,30,40,48,1,22,66,31&pricemarginCoeff=1.0&reg=0&appType=1&emp=0&locale=ru&lang=ru&curr=rub&couponsGeo=2,12,7,3,6,18,21&dest=-1029256,-72223,-1646090,-2307160&nm={code}");
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
            getRequest.Proxy = proxy;

            getRequest.Run(cookieContainer);

            var json = JObject.Parse(getRequest.Response);
            var data = json["data"];
            var products = data["products"];
            var name = products.First()["name"];
            var mainPrice = Convert.ToInt32(products.First()["priceU"]) / 100;
            var salePrice = Convert.ToInt32(products.First()["salePriceU"]) / 100;


            db.Card.Add(new CardEntity()
            {
                Id = code,
                Name = name.ToString(),
                MainPrice = mainPrice.ToString(),
                SalePrice = salePrice.ToString()
            });
            db.SaveChanges();

            //Console.WriteLine($"Артикул - {code}");
            //Console.WriteLine($"Имя - {name}");
            //Console.WriteLine($"Цена - {mainPrice} руб.");
            //Console.WriteLine($"Цена со скидкой - {salePrice} руб.");
            //Console.WriteLine();
        }
        
        
    }
    catch (Exception)
    {

    }
    
}

