using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MisakiEQ.Lib.Misskey.API
{

    public class CreateNote
    {
        /// <summary>
        /// ACCESS TOKEN
        /// </summary>
        [JsonProperty("i")]
        public string I { get; set; } = string.Empty;
        [JsonProperty("detail")]
        public bool detail { get; set; } = false;
        [JsonProperty("visibility")]
        public string Visibility { get; set; } = "Home";
        [JsonProperty("text")]
        public string Text { get; set; } = "";
        [JsonIgnore]
        [JsonProperty("fileId")]
        public List<string>? FileId { get; set; } = null;
        [JsonIgnore]
        [JsonProperty("replyId")]
        public string? replyId { get; set; } = null;
    }
    public class CreateNoteResponse
    {
        public class CreatedNote
        {
            public string id { get; set; }= string.Empty;
            public DateTime createdAt { get; set; }
            public string userId { get; set; }=string.Empty;
            public User user { get; set; } = new();
            public string? text { get; set; } = string.Empty;
            public string? cw { get; set; } = string.Empty;
            public string visibility { get; set; } = string.Empty;
            public bool localOnly { get; set; }
            public int renoteCount { get; set; }
            public int repliesCount { get; set; }
            public Reactions reactions { get; set; } = new();
            public ReactionEmojis reactionEmojis { get; set; } = new();
            public List<object> fileIds { get; set; } = new();
            public List<object> files { get; set; } = new();
            public string? replyId { get; set; }
            public string? renoteId { get; set; }
        }

        public class Emojis
        {
        }

        public class ReactionEmojis
        {
        }

        public class Reactions
        {
        }

        public class Root
        {
            public CreatedNote createdNote { get; set; }= new CreatedNote();
        }

        public class User
        {
            public string id { get; set; } = string.Empty;
            public string? name { get; set; }
            public string username { get; set; }=string.Empty;
            public string? host { get; set; }
            public string avatarUrl { get; set; }=string.Empty;
            public string? avatarBlurhash { get; set; }
            public bool isBot { get; set; }
            public bool isCat { get; set; }
            public Emojis emojis { get; set; } = new Emojis();
            public string onlineStatus { get; set; } = string.Empty;
            public List<object> badgeRoles { get; set; }=new ();
        }
    }
}
