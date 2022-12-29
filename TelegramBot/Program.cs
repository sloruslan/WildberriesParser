using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Wildberries.Shared.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Wildberries;
using Wildberries.Shared;
using TelegramBot;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    static ITelegramBotClient bot = new TelegramBotClient("5658520744:AAHJ3fPiIHqBavqOSoxdvfr0LaoxMjGWYsI");

    
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            
            var message = update.Message;

            switch (message?.Text)
            {
                case "/start":
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать в отслеживатель цен на Wildberries!");
                        await botClient.SendStickerAsync(message.Chat, "CAACAgIAAxkBAAIBp2OTkg0RDKJx8aQQ9tddk__Dw9kcAALYDwACSPJgSxX7xNp4dGuYKwQ");
                        break;
                    }

                case "/myproducts":
                    {
                        await GetMyProducts(botClient, message);
                        break;
                    }

                case "/help":
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Просто отправь мне ссылку на товар и я добавлю его в список отслеживания. " +
                            "Как только цена на товар изменится, я пришлю тебе об этом уведомление.");
                        break;
                    }

                default:
                    {
                        await AnotherCommand(botClient, message);
                    }
                    break;
            }
            
        }
    }

    private static async void ObserveData(ITelegramBotClient botClient)
    {
        try
        {
            while (true)
            {
                using (var db = new WbDataBaseContext())
                {
                    var Products = db.Card.Include(x => x.User).ToList();
                    for (int i = 0; i < Products.Count; i++)
                    {
                        Products[i] = Parsing.ParseData(Products[i]);
                        if (Products[i].SalePrice.Count > 2)
                        {
                            var newPrice = Products[i].SalePrice[Products[i].SalePrice.Count - 1];
                            var oldPrice = Products[i].SalePrice[Products[i].SalePrice.Count - 2];
                            if (newPrice > oldPrice)
                            {
                                await botClient.SendTextMessageAsync(Products[i].User.Id, 
                                    $"Товар '{Products[i].Name}' \n {Products[i].Url} \n подорожал с {oldPrice/100}.{oldPrice % 100} ₽ на {newPrice/100}.{newPrice % 100} ₽");
                            }
                            if (newPrice < oldPrice)
                            {
                                await botClient.SendTextMessageAsync(Products[i].User.Id,
                                    $"Товар '{Products[i].Name}' \n {Products[i].Url} \n подешевел с {oldPrice / 100}.{oldPrice % 100} ₽ на {newPrice / 100}.{newPrice % 100} ₽");
                            }
                        }
                        if (Products[i].SalePrice.Count > 100)
                        {
                            Products[i].SalePrice.Clear();
                            Products[i].Time.Clear();
                        }
                    }
                    db.SaveChanges();

                }

                await Task.Delay(600000);
            }
        }
        catch (Exception)
        {
 
        }
        
        


    }
    

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }


    private static async Task AnotherCommand(ITelegramBotClient botClient, Message? message)
    {
        var idFromMessage = message.From.Id;
        UserEntity currentUser = null;
        
        if (message != null)
        {
            if (message.From != null)
            {
                using (var db = new WbDataBaseContext())
                {
                    if (!db.User.Any(x => x.Id.Equals(idFromMessage)))
                    {
                        currentUser = new UserEntity()
                        {
                            Id = idFromMessage,
                            FirstName = message.From.FirstName ?? string.Empty,
                            LanguageCode = message.From.LanguageCode ?? string.Empty,
                            LastName = message.From.LastName ?? string.Empty,
                            Username = message.From.Username ?? string.Empty,
                            IsBot = message.From.IsBot,
                            UserProduct = new List<CardEntity>()
                        };

                        db.User.Add(currentUser);
                        db.SaveChanges();
                    }
                }
            }
            var findString = "wildberries.ru/catalog/";
            if (message.Text != null)
            {
                if (message.Text.Contains(findString))
                {
                    var start = message.Text.IndexOf(findString) + findString.Length;
                    var end = message.Text.IndexOf("/detail.aspx", start);
                    if (end == -1)
                    {
                        return;
                    }
                    var idProduct = message.Text.Substring(start, end - start);
                    if (idProduct != null)
                    {
                        using (var db = new WbDataBaseContext())
                        {
                            var currUser = db.User.Include(x => x.UserProduct).FirstOrDefault(x => x.Id == idFromMessage);
                            if (currUser != null)
                            {
                                if (currUser.UserProduct.Any(x => x.Article == Convert.ToInt64(idProduct)))
                                {
                                    var currProduct = currUser.UserProduct.FirstOrDefault(x => x.Article == Convert.ToInt64(idProduct));
                                    if (currProduct.SalePrice.Count > 1)
                                    {
                                        await botClient.SendTextMessageAsync(message.Chat, $"Товар '{currProduct.Name}' уже остлеживается\n Последняя проверка цены была\n " +
                                            $"{currProduct.Time.Last().LocalDateTime} \n" +
                                            $"Цена без учета личной скидки -  {currProduct.SalePrice.Last() / 100}.{currProduct.SalePrice.Last() % 100} ₽");
                                    }
                                    else
                                    {
                                        currProduct = Parsing.ParseData(currProduct);
                                        await botClient.SendTextMessageAsync(message.Chat, $"Товар '{currProduct.Name}' уже есть остлеживается.\n Последняя проверка цены была " +
                                            $"{currProduct.Time.Last().LocalDateTime} - {currProduct.SalePrice.Last() / 100}.{currProduct.SalePrice.Last() % 100} ₽");
                                    }

                                    return;
                                }
                                else
                                {
                                    var product = new CardEntity()
                                    {
                                        Article = Convert.ToInt64(idProduct),
                                        DateOfAddition = DateTimeOffset.UtcNow
                                    };
                                    product = Parsing.ParseData(product);
                                    currUser.UserProduct.Add(product);
                                    db.User.Update(currUser);
                                    db.SaveChanges();

                                    await botClient.SendTextMessageAsync(message.Chat.Id, "Товар добавлен в базу для отслеживания");
                                }


                            }
                        }



                    }
                }
            }
        }
    }

    private static async Task GetMyProducts(ITelegramBotClient botClient, Message? message)
    {
        var idFromMessage = message.From.Id;
        UserEntity currentUser = null;
        if (message != null)
        {
            if (message.From != null)
            {
                using (var db = new WbDataBaseContext())
                {
                    if (!db.User.Any(x => x.Id.Equals(idFromMessage)))
                    {
                        currentUser = new UserEntity()
                        {
                            Id = idFromMessage,
                            FirstName = message.From.FirstName ?? string.Empty,
                            LanguageCode = message.From.LanguageCode ?? string.Empty,
                            LastName = message.From.LastName ?? string.Empty,
                            Username = message.From.Username ?? string.Empty,
                            IsBot = message.From.IsBot,
                            UserProduct = new List<CardEntity>()
                        };

                        db.User.Add(currentUser);
                        db.SaveChanges();
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Ваш список товаров пока пуст");
                    }
                    else
                    {
                        currentUser = db.User.Include(x => x.UserProduct).FirstOrDefault(x => x.Id.Equals(idFromMessage));
                        
                        var listProduct = currentUser.UserProduct.OrderBy(x => x.DateOfAddition).ToList();
                        var listProductStr = "Ваш список товаров:\n\n";
                        
                        for (int j = 0; j < listProduct.Count; j+= 10)
                        {
                            if (j + 10 > listProduct.Count)
                            {
                                for (int i = j; i < listProduct.Count; i++)
                                {
                                    listProductStr += $"\n{i + 1} {listProduct[i].Name}\n{listProduct[i].Url}\nДобавлен {listProduct[i].DateOfAddition.LocalDateTime}\n";
                                }
                                await botClient.SendTextMessageAsync(message.Chat.Id, listProductStr);
                                listProductStr = "";
                            }
                            else
                            {
                                for (int i = j; i < j + 10; i++)
                                {
                                    listProductStr += $"\n{i + 1} {listProduct[i].Name}\n{listProduct[i].Url}\nДобавлен {listProduct[i].DateOfAddition.LocalDateTime}\n";
                                }
                                await botClient.SendTextMessageAsync(message.Chat.Id, listProductStr);
                                listProductStr = "";
                            }
                        }
                        
                        
                        
                    }
                }
            }
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        ObserveData(bot);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
       
    }
}


