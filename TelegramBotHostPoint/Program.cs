using System;
using Telegram.Bot;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using System.Net;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;

namespace TelegramBotHostPoint
{
    class Program
    {
        static readonly WebClient client = new WebClient();
        private static  TelegramBotClient Bot = new TelegramBotClient("1095007957:AAFkRRUrMpWNxciWiTidT91bebZs9NQG3hQ");
        static long  chat_id;
        static Dictionary<string, string> filters = new Dictionary<string, string>() {
                    {"maxtime","1000"}, {"country",""},
                    { "ports", "" }, {"type",""},
                    {"anon",""}, {"uptime",""}, {"out","&out=plain" }
                };
        static string _out = "plain";
        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                var message = ev.CallbackQuery.Message;
                if (ev.CallbackQuery.Data == "11")
                {
                    Bot.SendTextMessageAsync(chat_id,"Available filters:\nmaxtime - servers with latency above this variable won't appear" +
                        "\ncountry = include servers hosted in them. Type their 2symbol code (UA for example) " +
                        "\nports = desirable ports (80,3000-4000,4001 for example)" +
                        "\ntype = type of proxy(for HTTP type h, HTTPS - s,SOCKS4- 4,SOCKS5 - 5. )" +
                        "\nanon=anonimity level (1-none,2-low,3-medium,4-high)" +
                        "\nout= desirable format (csv, xml, js, php are available)" +
                        "\nExample how to search with filters:\n/start &maxtime=600&country=UARU&type=s45&out=csv" +
                        "\n Parameter out  is neccessary and has to be entered in the end when searching by filter.");

                }
                else
                if (ev.CallbackQuery.Data == "22")
                {
                    Fetch();
                }
            };
            
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private async static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
           chat_id = e.Message.Chat.Id;
            if (e.Message.Text == "/start")
            {
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
{               // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Yes", "11"),
                    InlineKeyboardButton.WithCallbackData("No", "22")
                }});
                Bot.SendTextMessageAsync(chat_id, "Wanna search for proxy servers with filters?", replyMarkup: inlineKeyboard);

            }
            else if (e.Message.Text.Contains('='))
            {

                string text = e.Message.Text;
                int length = text.Length;
                string _out = text.Substring(length - 3).Contains('=') ? text.Substring(length - 2) : text.Substring(length - 3);
                string datastring = null;
                try
                {

                    string address = "http://apigetfile.azurewebsites.net/HostPoint/GetFile?filter=" + e.Message.Text.Split(' ')[1].Replace('\"', ' ');
                     Console.WriteLine(address);
                     datastring = client.DownloadString(address).Replace('\"',' ');
                    
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

                    Bot.SendTextMessageAsync(chat_id,$"Error: incorrect filters");
                    Console.WriteLine(datastring);
                    throw;
                }
                finally
                {


                }
            }
        }
        /*private  static void FetchByFilter()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
{               // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Max latency", "11"),
                    InlineKeyboardButton.WithCallbackData("Ports", "12")
                },
                // second row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Uptime", "21"),
                    InlineKeyboardButton.WithCallbackData("Types", "22"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Anonimity","31"),
                    InlineKeyboardButton.WithCallbackData("Countries","32")
                }
            });
            Bot.SendTextMessageAsync(chat_id, "Filters?", replyMarkup: inlineKeyboard);
            }
            */
        private async static void Fetch()
        {

            byte[] data = client.DownloadData("http://apigetfile.azurewebsites.net/HostPoint/GetFile");
            System.IO.File.WriteAllBytes($"log.txt", data);
            var stream = System.IO.File.Open($"log.txt", FileMode.OpenOrCreate);
            InputOnlineFile iof = new InputOnlineFile(stream);
            iof.FileName = $"Chery Blossom.txt";
            await Bot.SendDocumentAsync(chat_id, iof, "");
        }
    }
}

