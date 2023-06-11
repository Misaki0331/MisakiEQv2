using MisakiEQ.Lib.Misskey.API;
using MisakiEQ.Lib.PrefecturesAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Parameters;
using System.Collections.Concurrent;

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
        static readonly HttpClient client = new HttpClient();
#if ADMIN
        //本番環境
        const string baseUrl = "https://misskey.io/api";
#elif DEBUG
        //デバッグ環境(ねのはいさんのサーバーをお借り)
        const string baseUrl = "https://msk.nenohi.net/api";
#else
        const string baseUrl = "";
#endif
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
        public static List<Note> SendNotes=new();

        private static readonly AsyncLock s_lock = new();
        public static async Task<string> CreateNote(string text, Setting.Visibility visibility, string replyid="", string fileid = "")
        {
            using (await s_lock.LockAsync())
            {
#if DEBUG
                text = "これはテストです。\n" + text;
#endif
                if (string.IsNullOrEmpty(accessToken))
                {
                    Log.Info("Misskeyのアクセストークンが存在しないためノート投稿できませんでした。");
                    return string.Empty;
                }
                try
                {
                    var api = new CreateNote
                    {
                        Text = text,
                        I = accessToken,
                        Visibility = visibility.ToString().ToLower()
                    };

                    if (!string.IsNullOrEmpty(fileid))
                        api.FileId = new(){fileid};
                    if (!string.IsNullOrEmpty(replyid))
                        api.replyId = replyid;
                    string sendText = JsonConvert.SerializeObject(api);

                    //また暴走を行うことを防ぐ為に
                    //重大インシデントを忘れるな 2023/5/4
                    var dep = SendNotes.Find(note => string.Equals(note.NoteString,text));
                    if(dep != null) {
                        dep.NotedTime = DateTime.Now;
                        Log.Warn("誤動作防止：送信するテキストが同じである為失敗しました。");
                        return "";
                    }
                    var b = new List<Note>();
                    b.AddRange(SendNotes.FindAll(a => a.NotedTime.AddHours(1) < DateTime.Now));
                    foreach (var a in b) SendNotes.Remove(a);
                    var content = new StringContent(sendText, Encoding.UTF8, @"application/json");

                    Log.Debug("Misskey API Posting...");

                    var responce = await client.PostAsync(baseUrl + "/notes/create", content);

                    //レスポンスコードを返す
                    Log.Debug($"Status Code : {(int)responce.StatusCode} - {responce.StatusCode}");
                    //返り値をそのまま出す
                    string output = await responce.Content.ReadAsStringAsync();
                    Log.Debug($"Contents : \"{output}\"");
                    if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        Log.Warn("リクエストが正常に送信できませんでした。");
                        return string.Empty;
                    }
                    var rs = JsonConvert.DeserializeObject<CreateNoteResponse.Root>(output);
                    if (rs == null)
                    {
                        Log.Error("Misskey APIは何も返しませんでした。");
                        return string.Empty;
                    }
                    else
                    {
                        SendNotes.Add(new(text, DateTime.Now));
                        Log.Debug($"NoteID:{rs.createdNote.id} Visibility:{rs.createdNote.visibility}");
                    }
                    return rs.createdNote.id;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return string.Empty;
                }
            }
        }

    }
}
