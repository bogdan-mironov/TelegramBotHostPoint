using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using static TelegramBotHostPoint.Program;
using Telegram.Bot.Types.ReplyMarkups;

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
                    InlineKeyboardButton.WithCallbackData("Yes", "Yes"),
                    InlineKeyboardButton.WithCallbackData("No", "No")
                }});
            await Bot.SendTextMessageAsync(chat_id, "Wanna search for proxy servers with filters?", replyMarkup: inlineKeyboard);
        }
        public async static void Bot_OnCallbackQuery(object sc, CallbackQueryEventArgs ev)
        {
            var message = ev.CallbackQuery.Message;
            switch (ev.CallbackQuery.Data)
            {
                // Поиск по фильтру
                case ("Yes"):
                    await Bot.SendTextMessageAsync(chat_id,
                        "out - тип файла, в котором будет результат поиска" +
                        "\nmaxtime - максимальная задержка" +
                        "\nports - порты" +
                        "\ntype - протоколы" +
                        "\nuptime - аптайм" +
                        "\ncountry - страны");
                    break;
                case ("No"):
                    Fetch();
                    break;
            }
           }
        public async static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            chat_id = e.Message.Chat.Id;
            if (e.Message.Text == "Search")
            {
                ShowKeyboard();
            }
            else if (e.Message.Text.Contains('='))
            {

                string text = e.Message.Text;
                int length = text.Length;
                string _out = text.Substring(length - 3).Contains('=') ? text.Substring(length - 2) : text.Substring(length - 3);
                string datastring = null;
                /*
                try
                {
                    string filter = e.Message.Text.Split(' ')[1];
                    string address = "http://apigetfile.azurewebsites.net/HostPoint/GetFile?filter=" + filter;
                    datastring = client.DownloadString(address).Replace('\"', ' ');

                    byte[] rawdata = Convert.FromBase64String(datastring);
                    System.IO.File.WriteAllBytes($"log.{_out}", rawdata);
                    using (var stream = System.IO.File.Open($"log.{_out}", FileMode.OpenOrCreate))
                    {
                        InputOnlineFile iof = new InputOnlineFile(stream);
                        iof.FileName = $"Chery Blossom.{_out}";
                        await Bot.SendDocumentAsync(chat_id, iof, "");
                    }

                }
                catch (Exception)
                {

                    Bot.SendTextMessageAsync(chat_id, $"Error: incorrect filters");
                }
                */
            }
        }
    }
}
