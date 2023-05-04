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
        public class Note
        {
            public Note(string note,DateTime time)
            {
                NoteString= note;
                NotedTime = time;
            }
            public string NoteString = "";
            public DateTime NotedTime;
        }
        public static List<Note> SendNotes=new List<Note>();

        public static async Task<string> CreateNote(string text, Setting.Visibility visibility, string replyid="", string fileid = "")
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                Log.Instance.Info("Misskeyのアクセストークンが存在しないためノート投稿できませんでした。");
                return string.Empty;
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
                string sendText = JsonConvert.SerializeObject(api);

                //また暴走を行うことを防ぐ為に
                for(int i = 0; i < SendNotes.Count; i++)
                {
                    if (SendNotes[i].NoteString == text)
                    {
                        SendNotes[i].NotedTime= DateTime.Now;
                        Log.Instance.Warn("誤動作防止：送信するテキストが同じである為失敗しました。");
                        return "";
                    }
                }

                foreach (var a in SendNotes)
                {
                    if(a.NotedTime.AddHours(1) < DateTime.Now)
                    {
                        SendNotes.Remove(a);
                    }
                }
                var content = new StringContent(sendText, Encoding.UTF8, @"application/json");

                Log.Instance.Debug("Misskey API Posting...");

                var responce = await client.PostAsync(baseUrl + "/notes/create", content);

                //レスポンスコードを返す
                Log.Instance.Debug($"Status Code : {(int)responce.StatusCode} - {responce.StatusCode.ToString()}");
                //返り値をそのまま出す
                string output = await responce.Content.ReadAsStringAsync();
                Log.Instance.Debug($"Contents : \"{output}\"");
                if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Log.Instance.Warn("リクエストが正常に送信できませんでした。");
                    return string.Empty;
                }
                var rs = JsonConvert.DeserializeObject<API.CreateNoteResponse.Root>(output);
                if (rs == null)
                {
                    Log.Instance.Error("Misskey APIは何も返しませんでした。");
                    return string.Empty;
                }
                else {
                    SendNotes.Add(new(text, DateTime.Now));
                    Log.Instance.Debug($"NoteID:{rs.createdNote.id} Visibility:{rs.createdNote.visibility}");
                }
                return rs.createdNote.id;
            }catch(Exception ex)
            {
                Log.Instance.Error(ex);
                return string.Empty;
            }
        }

    }
}
