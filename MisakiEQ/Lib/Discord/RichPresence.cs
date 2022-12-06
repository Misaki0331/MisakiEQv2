using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;

namespace MisakiEQ.Lib.Discord
{
    public class RichPresence
    {

        private DiscordRpcClient? client=null;
        private RichPresence()
        {

        }
        private static RichPresence? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static RichPresence GetInstance()
        {
            if (singleton == null)
            {
                singleton = new RichPresence();
            }
            return singleton;
        }


        string DiscordDetail { get; set; } = string.Empty;
        string DiscordState { get; set; } = string.Empty;
        string DiscordLImagePath { get; set; } = string.Empty;
        string DiscordSImagePath { get; set; } = string.Empty;
        string DiscordLImageText { get; set; } = string.Empty;
        string DiscordSImageText { get; set; } = string.Empty;

        public void Init()
        {
            try
            {
                client = new DiscordRpcClient(Resources.API.API.DiscordRPC);
                client.OnReady += (sender, e) =>
                {
                    Log.Instance.Info($"ユーザー名 : {e.User.Username}");
                    Log.Instance.Info($"会員資格 : {e.User.Premium}");
                };


                client.OnPresenceUpdate += (sender, e) =>
                {
                    //Log.Instance.Debug($"更新情報を受信 : {e.Presence.}");
                };

                //Connect to the RPC
                client.Initialize();
                if (DiscordLImagePath == "") DiscordLImagePath = "default_main";
                client.SetPresence(new DiscordRPC.RichPresence()
                {
                    Details = DiscordDetail,
                    State = DiscordState,
                    Assets = new Assets()
                    {
                        LargeImageKey = DiscordLImagePath,
                        LargeImageText = DiscordLImageText,
                        SmallImageKey = DiscordSImagePath,
                        SmallImageText = DiscordSImageText
                    }
                });
            }
            catch
            {

            }
        }
        public void Update(string? detail = null, string? status = null, string? LImgKey = null, string? LImgText = null, string? SImgKey = null, string? SImgText = null)
        {
            if (client == null) throw new ArgumentException("Discord RPCは初期化されていません。Init()関数を実行してください。");
            if (detail != null) DiscordDetail = detail;
            if (status != null) DiscordState = status;
            if (LImgKey != null) DiscordLImagePath = LImgKey;
            if (LImgText != null) DiscordLImageText = LImgText;
            if(SImgKey != null) DiscordLImagePath = SImgKey;
            if(SImgText != null) DiscordLImageText = SImgText;
            if (DiscordLImagePath == "") DiscordLImagePath = "default_main";
            client.SetPresence(new DiscordRPC.RichPresence()
            {
                Details = DiscordDetail,
                State = DiscordState,
                Assets = new Assets()
                {
                    LargeImageKey = DiscordLImagePath,
                    LargeImageText = DiscordLImageText,
                    SmallImageKey = DiscordSImagePath,
                    SmallImageText = DiscordSImageText
                }
            });
        }
    }
}
