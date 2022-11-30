
// артикул товара
using ConsoleApp.Domain;
using ConsoleApp.Services;
using System.Net;

var code = "11526261";

// прокси-сервер
var proxy = new WebProxy("127.0.0.1:8888");

// контейнер куки
var cookieContainer = new CookieContainer();

//// запрос №1. получение адреса карточки товара по артикулу товара
//var postRequest = new PostRequest("https://wildberries.ru/");
//postRequest.Data = $"catalog/{code}/detail.aspx?targetUrl=XS HTTP/1.1";
//postRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
//postRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
//postRequest.ContentType = "application/x-www-form-urlencoded";
//postRequest.Referer = "https://wildberries.ru/";
//postRequest.Host = "wildberries.ru";
//postRequest.Proxy = proxy;
//postRequest.Headers.Add("Bx-ajax", "true");
//postRequest.Run(cookieContainer);

//// поиск в HTML-коде ответа адрес карточки товара
//var strStart = postRequest.Response.IndexOf("search-result-group search-result-product");
//strStart = postRequest.Response.IndexOf("<a href=", strStart) + 9;
//var strEnd = postRequest.Response.IndexOf("\"", strStart);
//var getPath = postRequest.Response.Substring(strStart, strEnd - strStart);

//// вывод в консоль найденный адрес карточки по артикулу
//Console.WriteLine($"getPath={getPath}");

// запрос №2. получение карточки товара
var getRequest = new GetRequest($"https://wildberries.ru/catalog/{code}/detail.aspx?targetUrl=TB");
getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
getRequest.Referer = "https://wildberries.ru/";
getRequest.Host = "wildberries.ru";
getRequest.Proxy = proxy;
getRequest.Run(cookieContainer);

// создание объекта класса карточки товара для парсинга искомых данных
var card = new Card();
card.Parse(getRequest.Response);

// вывод в консоль параметров найденного товара: название и цена
Console.WriteLine($"title={card.Title}");
Console.WriteLine($"price={card.Price}");

Console.ReadKey();

