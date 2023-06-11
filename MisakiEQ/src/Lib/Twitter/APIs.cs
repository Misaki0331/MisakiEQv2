using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace MisakiEQ.Lib.Twitter
{
    public class APIs
    {
        public readonly Config Config = new();
        private static APIs? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static APIs GetInstance()
        {
            singleton ??= new APIs();
            return singleton;
        }
        public static int GetLen(string str)
        {
            var replace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!\"#$%&'()=~|-^\\@[`{;:]+*},./<>?\n";
            for (int i = 0; i < replace.Length; i++)
            {
                str = str.Replace(replace[i], ' ');
            }
            int len = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ') len++;
                else len += 2;
            }
            return len;
        }

        TwitterClient? client = null;
        TwitterClient? AuthClient = null;
        Tweetinvi.Models.IAuthenticationRequest? AuthReq = null;
        Tweetinvi.Models.IAuthenticatedUser? UserInfo = null;
        private string AccessToken = "";
        private string AccessTokenSecret = "";
        public async Task<string> GetAuthURL()
        {
            try
            {
                var keys = Resources.API.API.TwitterAPI.Split('\n');
                keys[0] = keys[0].Replace("\r", "");
                AuthClient = new TwitterClient(keys[0], keys[1]);
                AuthReq = await AuthClient.Auth.RequestAuthenticationUrlAsync();
                return AuthReq.AuthorizationURL;
            }catch(Exception ex)
            {
                Log.Error($"Twitterの認証中にエラーが発生しました。\n{ex.Message}");
                throw;
            }
        }
        public async Task<bool> AuthFromPincode(string pincode)
        {
            try
            {
                if (AuthClient == null) throw new InvalidOperationException("AuthClientがnullです。");
                var userCredentials = await AuthClient.Auth.RequestCredentialsFromVerifierCodeAsync(pincode, AuthReq);
                client = new TwitterClient(userCredentials);
                UserInfo = await client.Users.GetAuthenticatedUserAsync();
                AccessToken = userCredentials.AccessToken;
                AccessTokenSecret = userCredentials.AccessTokenSecret;
                Log.Info("次のユーザーとしてログインしました: " + UserInfo);
                return true;

            }
            catch (Tweetinvi.Exceptions.TwitterException ex)
            {
                Log.Error($"Twitterの認証に失敗しました。\n{ex.Message}");
                return false;

            }
            catch (Exception ex)
            {
                Log.Error($"Twitterの認証ができませんでした。\n{ex.Message}");
                return false;
            }
        }

        public async Task<bool> AuthFromToken(string token, string secret)
        {
            try
            {
                var keys = Resources.API.API.TwitterAPI.Split('\n');
                keys[0] = keys[0].Replace("\r", "");
                client = new TwitterClient(keys[0], keys[1], token, secret);
                UserInfo = await client.Users.GetAuthenticatedUserAsync();
                Log.Info("次のユーザーとしてログインしました: " + UserInfo);
                return true;
            }
            catch (Tweetinvi.Exceptions.TwitterException ex)
            {
                Log.Error($"Twitterの認証に失敗しました。\n{ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"Twitterの認証ができませんでした。\n{ex.Message}");
                return false;
            }
        }

        public async Task<bool> AuthAgain()
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(AccessTokenSecret))
                    throw new ArgumentException("トークンが無効です。再連携してください。");
                var task = AuthFromToken(AccessToken, AccessTokenSecret);
                await task;
                return task.Result;
            }catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public string GetAccessToken()
        {
            return AccessToken;
        }
        public string GetAccessTokenSecret()
        {
            return AccessTokenSecret;
        }
        public async Task<long> Tweet(string tweet,long reply = 0)
        {
            try
            {
                if (client == null) throw new ArgumentException("認証されていません。");
                if (reply == 0)
                {
                    var res = await client.Tweets.PublishTweetAsync(tweet);
                    return res.Id;
                }
                else
                {

                    var replys = await client.Tweets.PublishTweetAsync(new PublishTweetParameters(tweet)
                    {
                        InReplyToTweetId = reply
                    });
                    return replys.Id;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return -1;
            }
        }
        public string? GetUserScreenID()
        {
            if (UserInfo == null) return null;
            return UserInfo.ScreenName;
        }
        public string? GetUserName()
        {
            if (UserInfo == null) return null;
            return UserInfo.Name;
        }
        public long? GetUserFollowers()
        {
            if (UserInfo == null) return null;
            return UserInfo.FollowersCount;
        }
        public long? GetUserTweets()
        {
            if (UserInfo == null) return null;
            return UserInfo.StatusesCount;
        }
    }
}