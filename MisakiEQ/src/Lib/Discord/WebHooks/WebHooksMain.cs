using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MisakiEQ.Lib.Discord.WebHooks
{
    public class Main
    {
        public static Token? TokenData { get => token; }
        static Token token=new(0,"");
        public static bool SetToken(string url)
        {
            try
            {
                token = new(url);
            }catch(Exception e)
            {
                Log.Instance.Error(e);
                return false;
            }
            return true;
        }

        static private Main? instance;
        static public Main Instance { get {
                instance ??= new();
                return instance;
            } }
        public Config Config = new();
        private Main()
        {
        }
        public class Token
        {
            //public Token(Uri uri) { Token(uri.OriginalString); }
            public Token(string url)
            {
                if (url.StartsWith("https://discord.com/api/webhooks/"))
                {
                    url = url.Replace("https://discord.com/api/webhooks/", "");
                    var data = url.Split('/');
                    if (data.Length == 2)
                    {
                        if (!long.TryParse(data[0], out TokenID)) throw new ArgumentException("第1引数は数値である必要があります。");
                        TokenKey = data[1];
                    }
                    else throw new ArgumentException("ウェブフックAPIの引数が違います。");
                }
                else
                    throw new ArgumentException("これはウェブフックのURLではありません。");
            }
            public Token(long ID, string Key)
            {
                TokenID = ID;
                TokenKey = Key;
            }
            public long TokenID = -1;
            public string TokenKey = "";
        }
        public class Content
        {
            public string username { get; set; } = "MisakiEQ - 地震情報";
            public string avatar_url { get; set; } = "https://github.com/Misaki0331/MisakiEQ/blob/main/Resources/icon/main_big.png?raw=true";
            public string content { get; set; } = string.Empty;
            public bool tts { get; set; }
            public List<JSON.Embed> embeds { get; set; } = new();
        }

        public async static void Sent(Token token, Content content)
        {
            await Lib.WebAPI.PostJson($"https://discord.com/api/webhooks/{token.TokenID}/{token.TokenKey}", JsonSerializer.Serialize(content));
        }
    }
    
}
namespace MisakiEQ.Lib.Discord.WebHooks.JSON
{
    public class Embed
    {
        public string type { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int color { get; set; }
        public List<Field> fields { get; set; } = new();
        public DateTime timestamp { get; set; }
    }

    public class Field
    {
        public string name { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
        public bool inline { get; set; }
    }
}