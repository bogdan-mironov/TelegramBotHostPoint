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
        public static TelegramBotClient Bot = new TelegramBotClient("1095007957:AAFkRRUrMpWNxciWiTidT91bebZs9NQG3hQ");
        public static long chat_id {get;set;}
        static string _out = "txt";
        static void Main(string[] args)
        {
            Bot.OnMessage += Events.Bot_OnMessage;
            Bot.OnCallbackQuery += Events.Bot_OnCallbackQuery;
            
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }
        static string address = "http://apigetfile.azurewebsites.net/HostPoint/GetFile?filter=";
        public async static void Fetch(string filter = "maxtime=800;out=plain")
        {
            try
            {
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
                Bot.SendTextMessageAsync(chat_id, "Ошибочка выполнения");
            }
            
        }
    }
}

