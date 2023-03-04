using MisakiEQ.Lib.Misskey.API;
using MisakiEQ.Lib.PrefecturesAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Parameters;

namespace MisakiEQ.Lib.Misskey
{
    public class Setting
    {
        public enum Visibility
        {
            Public,
            Home,
            Followers,
            Specified
        }
    }
    public static class APIData
    {
        public static Config Config = new();
        static HttpClient client = new HttpClient();
        const string baseUrl = "https://misskey.io/api";
        public static string accessToken = "";


        public static async Task<string> CreateNote(string text, Setting.Visibility visibility, string replyid="", string fileid = "")
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                Log.Instance.Info("Misskeyのアクセストークンが存在しないためノート投稿できませんでした。");
                return "";
            }
            try
            {
                var api = new API.CreateNote();
                api.Text = text;
                api.I = accessToken;
                api.Visibility = visibility.ToString().ToLower();

                if (!String.IsNullOrEmpty(fileid))
                {
                    api.FileId = new();
                    api.FileId.Add(fileid);
                }
                if (!string.IsNullOrEmpty(replyid))
                {
                    api.replyId = replyid;
                }
                Log.Instance.Debug(JsonConvert.SerializeObject(api));
                var content = new StringContent(JsonConvert.SerializeObject(api), Encoding.UTF8, @"application/json");

                Log.Instance.Debug("Misskey API Posting...");

                var responce = await client.PostAsync(baseUrl + "/notes/create", content);

                Log.Instance.Debug($"{responce.StatusCode.ToString()}");

                var rs = JsonConvert.DeserializeObject<API.CreateNoteResponse.Root>(await responce.Content.ReadAsStringAsync());
                if (rs == null)
                {
                    Log.Instance.Error("Misskey.ioは何も返しませんでした。");
                    return "";
                }
                else {
                    Log.Instance.Debug($"NoteID:{rs.createdNote.id} Visibility:{rs.createdNote.visibility}");
                }
                return rs.createdNote.id;
            }catch(Exception ex)
            {
                Log.Instance.Error(ex);
                return "";
            }
        }

    }
}
