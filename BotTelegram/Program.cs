using System;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Data.SQLite;

namespace BotTelegram
{
    
    class Program
    {
        const string TOKEN = "1798560868:AAGtZR48DsiaSIF0cCH4itNb44Qkz5SnSIU";
        public static SQLiteConnection DB;
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    GetMessage().Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Err:" + ex);
                }
            }
        }

        static async Task GetMessage() 
        {
            TelegramBotClient bot = new TelegramBotClient(TOKEN);
            int offset = 0;
            int timeout = 0;
            try
            {
                await bot.SetWebhookAsync("");
                while(true)
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout);

                    foreach(var update in updates)
                    {
                        var message = update.Message;

                        if(message.Text == "MyFirstBot")
                        {
                            Console.WriteLine("Получено сообщение:"+ message.Text);

                            await bot.SendTextMessageAsync(message.Chat.Id,"Ку,епта" + message.Chat.FirstName);
                        }

                        if(message.Text=="/reg")
                        {
                            Registration(message.Chat.Id.ToString(), message.Chat.FirstName);
                            await bot.SendTextMessageAsync(message.Chat.Id, "Пользователь зарегистрирован");
                        }
                        offset = update.Id + 1;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Err:" + ex);
            }
        }

        public static void Registration(string chatId,string userName)
        {
            try
            {
                DB = new SQLiteConnection("Data Source=C:/Users/Компьютер/source/repos/BotTelegram/BotTelegram/DB.db");
                DB.Open();
                //Console.WriteLine(DB.State);
                
                SQLiteCommand regCmd = DB.CreateCommand();
                //regCmd.CommandText = "SELECT *FROM RegUsers";
                regCmd.CommandText = "INSERT INTO RegUsers VALUES(@ChatId,@UserName)";
                regCmd.Parameters.AddWithValue("@ChatId", chatId);
                regCmd.Parameters.AddWithValue("@UserName",userName);
                regCmd.ExecuteNonQuery();
                DB.Close();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine("Err:" + ex);
            }
            
        }

    }
}
