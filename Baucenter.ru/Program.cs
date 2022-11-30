using BlablacarApi;
using System.Net;

var code = "416001653";
var proxy = new WebProxy("127.0.0.1:8888");
var cookieContainer = new CookieContainer();

var postRequest = new PostRequest("https://baucenter.ru/");

postRequest.Data = $"ajax_call=y&INPUT_ID=title-search-input&q={code}&l=2";
postRequest.Accept = "*/*";
postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
postRequest.ContentType = "application/x-www-form-urlencoded";
postRequest.Referer = "https://baucenter.ru/";
postRequest.Host = "baucenter.ru";
postRequest.Proxy = proxy;

postRequest.Headers.Add("Bx-ajax", "true");
postRequest.Headers.Add("Origin", "https://baucenter.ru");
postRequest.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
postRequest.Headers.Add("sec-ch-ua-mobile","?0");
postRequest.Headers.Add("sec-ch-ua-platform" ,"\"Windows\"");
postRequest.Headers.Add("Sec-Fetch-Dest","empty");
postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
postRequest.Headers.Add("Sec-Fetch-Site", "same-origin");
postRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
postRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9");


postRequest.Run(cookieContainer);

var strStart = postRequest.Response.IndexOf("google-site-verification");
strStart = postRequest.Response.IndexOf("content=\"", strStart) + 9;

var strEnd = postRequest.Response.IndexOf("\"", strStart);
var getPath = postRequest.Response.Substring(strStart, strEnd - strStart);
{ }