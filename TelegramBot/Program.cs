using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Wildberries.Shared.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Wildberries;

class Program
{

    static ITelegramBotClient bot = new TelegramBotClient("5658520744:AAHJ3fPiIHqBavqOSoxdvfr0LaoxMjGWYsI");
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
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
                            //db.User.Where(x => x.Id.Equals(idFromMessage)).Include(x => x.UserProduct).FirstOrDefault();
                            db.User.Add(currentUser);
                            db.SaveChanges();
                        }
                        else
                        {
                            currentUser = db.User.Where(x => x.Id.Equals(idFromMessage)).Include(x => x.UserProduct).ThenInclude(x => x.TimePoint).FirstOrDefault();
                        }
                    }
                }
                if (message.Text != null)
                {
                    if (message.Text.Contains("https://wildberries.ru/catalog/"))
                    {
                        var start = message.Text.IndexOf("https://wildberries.ru/catalog/") + 31;
                        var end = message.Text.IndexOf("/detail.aspx", start);
                        var idProduct = message.Text.Substring(start, end - start);
                        if (idProduct != null)
                        {
                            using (var db = new WbDataBaseContext())
                            {
                                db.User.Where(x => x.Id == idFromMessage).FirstOrDefault().UserProduct.Add(new CardEntity() { Id = Convert.ToInt32(idProduct) });
                                db.SaveChanges();
                            }

                            await botClient.SendTextMessageAsync(message.Chat, "Товар добавлен в базу для отслеживания");
                        }
                    }
                }
            }
        }
    }


    

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }


    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

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


