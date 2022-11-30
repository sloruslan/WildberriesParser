using BlablacarApi;

var code = "708004917";
var postRequest = new PostRequest("https://baucenter.ru/");

postRequest.Data = $"ajax_call=y&INPUT_ID=title-search-input&q={code}&l=2";
postRequest.Accept = "*/*";
postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
postRequest.ContentType = "application/x-www-form-urlencoded";
postRequest.Referer = "https://baucenter.ru/";
postRequest.Host = "baucenter.ru";

postRequest.Headers.Add("Bx-ajax", "true");
postRequest.Headers.Add("Origin", "https://baucenter.ru");
postRequest.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
postRequest.Headers.Add("sec-ch-ua-mobile","?0");
postRequest.Headers.Add("sec-ch-ua-platform" ,"\"Windows\"");
postRequest.Headers.Add("Sec-Fetch-Dest","empty");
postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
postRequest.Headers.Add("Sec-Fetch-Site", "same-origin");

postRequest.Run();