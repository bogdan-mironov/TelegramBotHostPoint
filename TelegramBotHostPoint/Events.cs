using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using static TelegramBotHostPoint.Program;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;
using System.IO;

namespace TelegramBotHostPoint
{
    class Events
    {
        public async static void ShowKeyboard()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
{               // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Да", "Yes"),
                    InlineKeyboardButton.WithCallbackData("Нет", "No")
                }});
            await Bot.SendTextMessageAsync(chat_id, "Ищем прокси по фильтру?", replyMarkup: inlineKeyboard);
        }
        public async static void Bot_OnCallbackQuery(object sc, CallbackQueryEventArgs ev)
        {
            
            switch (ev.CallbackQuery.Data)
            {
                // Поиск по фильтру
                case ("Yes"):
                    await Bot.SendTextMessageAsync(chat_id,
                        "out - тип файла, в котором будет выдан результат поиска" +
                        "\n\nmaxtime - максимальная задержка" +
                        "\n\nports - порты \nВводите порты через запятую или тире (8080,2000-3000)" +
                        "\n\ntype - протоколы \nВводите в параметр код протокола (Для HTTP - h, HTTPS - s,SOCKS4 - 4,SOCKS5 -5, можно вводить несколько)" +
                        "\n\ncountry - прокси, запущенные из этих стран, будут выведены в результате поиска (Используйте 2-х значный код страны по ISO 3166-1)" +
                        "\n\nПример поиска по фильтру: /start maxtime=1000;type=4,s,h;out=csv" +
                        "\n\nПараметр out обязательно должен быть в конце");

                    break;
                case ("No"):
                    Fetch(_out:"plain");
                    break;
            }
           }
        public async static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            chat_id = e.Message.Chat.Id;
            if (e.Message.Text == "/start")
            {
                ShowKeyboard();
            }
            else if (e.Message.Text.StartsWith("/start") && e.Message.Text.Contains("out="))
            {
                string text = e.Message.Text;
                string _out = text.Substring(text.Length - 3);
                string filter = e.Message.Text.Split(' ')[1];
                Fetch(_out,filter);
            }
        }
    }
}
