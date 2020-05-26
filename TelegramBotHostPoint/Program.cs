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
        public static readonly WebClient client = new WebClient();
        public static TelegramBotClient Bot = new TelegramBotClient("1095007957:AAFkRRUrMpWNxciWiTidT91bebZs9NQG3hQ");
        public static long chat_id {get;set;}
        static string _out = "txt";
        static List<string> availableformats = new List<string>() { "txt", "json", "xml", "csv", "php" };
        static void Main(string[] args)
        {
            Bot.OnMessage += Events.Bot_OnMessage;
            Bot.OnCallbackQuery += Events.Bot_OnCallbackQuery;
            
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        static string address = "http://apigetfile.azurewebsites.net/HostPoint/GetFile?filter=";
        public async static void Fetch(string _out, string filter = "maxtime=800;out=plain")
        {
            try
            {
                _out = GetFormat(_out);
                Console.WriteLine(address + filter);
                string datastring = client.DownloadString(address + filter).Trim('\"');
                byte[] rawdata = Convert.FromBase64String(datastring);
                File.WriteAllBytes($"output.{_out}", rawdata);
                using (FileStream stream = File.Open($"output.{_out}", FileMode.Open))
                {
                    InputOnlineFile file = new InputOnlineFile(stream);
                    file.FileName = $"out.{_out}";
                    await Bot.SendDocumentAsync(chat_id, file, "");
                }
            }
            catch (Exception)
            {
                Bot.SendTextMessageAsync(chat_id, "Ошибка выполнения");
            }
            
        }
        public static  string GetFormat(string _out)
        {
            if (_out == "plain") return "txt";
            if(_out == "js") return "json";
            if (!availableformats.Contains(_out))
            {
                Bot.SendTextMessageAsync(chat_id, "Вы ввели неверный фильтр");
                throw  new ArgumentException();
            }
            return _out;
        }
    }
}

