using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DmdataSharp;
using DmdataSharp.Authentication.OAuth;

namespace MisakiEQ.Background.API.EEW.dmdata
{
    public class Analysis
    {
        DmdataApiClientBuilder ApiBuilder;
        string RefreshToken = "";
        public Analysis()
        {
            ApiBuilder = DmdataApiClientBuilder.Default
                .UserAgent("MisakiEQ")
                .Referrer(new Uri("https://github.com/Misaki0331/MisakiEQv2/"));
        }
        public async Task<string?> Authentication(CancellationToken? token)
        {
            try
            {
                var clientId = Properties.Resources.dmdata;
                var scopes = new[] { "contract.list", "telegram.list", "socket.start", "telegram.get.earthquake", "gd.eew" };
                var credential = await SimpleOAuthAuthenticator.AuthorizationAsync(
                    ApiBuilder.HttpClient,
                    clientId,
                    scopes,
                    "MisakiEQ",
                    u => Process.Start(new ProcessStartInfo("cmd", $"/c start {u.Replace("&", "^&")}") { CreateNoWindow = true }),
                    token: token );
                RefreshToken = credential.RefreshToken;
                Log.Instance.Debug($"RefreshToken : ************************");
                return credential.RefreshToken;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                return null;
            }
        }
        public async void Init()
        {
            var clientId = Properties.Resources.dmdata; 
            var scopes = new[] { "contract.list", "telegram.list", "socket.start", "telegram.get.earthquake","gd.eew" };
            var credential = new OAuthRefreshTokenCredential(ApiBuilder.HttpClient,
                    scopes,
                    clientId,
                    RefreshToken);
            ApiBuilder = ApiBuilder.UseOAuth(credential);
            using var client = ApiBuilder.BuildV2ApiClient();
            var telegramList = await client.GetEewEventsAsync(limit: 10);
            Log.Instance.Debug($"Status : {telegramList.Status}");
            for(int i=0;i< telegramList.Items.Length; i++)
            {
                string str = $"Data[{i}] : {telegramList.Items[i].DateTime} {(telegramList.Items[i].IsCanceled ? "Cancelled" : "")} ";
                var data = telegramList.Items[i].Earthquake;
                if (data != null)
                {
                    str += $"{data.Hypocenter.Name} M{data.Magnitude.Value:0.0} {data.Hypocenter.Depth.Value}km";
                }
                Log.Instance.Debug($"Data[{i}] : {str}");
            }
            await credential.RevokeRefreshTokenAsync();
        }
    }
}
