using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace MisakiEQ.Lib
{
    internal static class WebAPI
    {
        public static async Task<byte[]> GetBytes(string URL,CancellationToken? token = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(URL)) throw new ArgumentException("ダウンロード先のURLが指定されていません。");
                if (!Uri.IsWellFormedUriString(URL, UriKind.Absolute)) throw new ArgumentException("ダウンロード先のURLのリンクが不正です。");
                using HttpClient webClient = new();
                var stream = await webClient.GetByteArrayAsync(new Uri(URL), (CancellationToken)(token != null ? token : CancellationToken.None));
                return stream;
            }
            catch (HttpRequestException ex)
            {
                throw new FileLoadException($"{GetHTTPErrorName(ex)} 参照元:\"{URL}\"");
            }
            catch (TaskCanceledException ex)
            {
                Log.Warn("タスクが取り消されました。");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                await Task.FromException<string>(ex);
                return Array.Empty<byte>();
            }
        }
        public static async Task<string> GetString(string URL,CancellationToken? token=null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(URL)) throw new ArgumentException("ダウンロード先のURLが指定されていません。");
                if (!Uri.IsWellFormedUriString(URL, UriKind.Absolute)) throw new ArgumentException("ダウンロード先のURLのリンクが不正です。");
                using HttpClient webClient = new();
                var stream = await webClient.GetStringAsync(new Uri(URL), (CancellationToken)(token != null ? token : CancellationToken.None));
                return stream;
            }
            catch (HttpRequestException ex)
            {
                throw new FileLoadException($"{GetHTTPErrorName(ex)} 参照元:\"{URL}\"");
            }
            catch (TaskCanceledException ex)
            {
                Log.Warn("タスクが取り消されました。");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                await Task.FromException<string>(ex);
                return string.Empty;
            }
        }

        public static async Task<string> PostJson(string URL, string json, CancellationToken? token = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(URL)) throw new ArgumentException("ダウンロード先のURLが指定されていません。");
                if (!Uri.IsWellFormedUriString(URL, UriKind.Absolute)) throw new ArgumentException("ダウンロード先のURLのリンクが不正です。");
                if(string.IsNullOrWhiteSpace(json)) throw new ArgumentException("アップロードするjsonが指定されていません。");
                using HttpClient webClient = new();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var stream = await webClient.PostAsync(new Uri(URL), content, (CancellationToken)(token != null ? token : CancellationToken.None)); 
                var text = await stream.Content.ReadAsStringAsync();
                Log.Debug($"PostJson:{new Uri(URL).Host} - {(int)stream.StatusCode}({stream.StatusCode})");
                if (string.IsNullOrEmpty(text)) return string.Empty;
                return text;
            }
            catch (HttpRequestException ex)
            {
                throw new FileLoadException($"{GetHTTPErrorName(ex)} 参照元:\"{URL}\"");
            }
            catch (TaskCanceledException ex)
            {
                Log.Warn("タスクが取り消されました。");
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                await Task.FromException<string>(ex);
                return string.Empty;
            }
        }


        static string GetHTTPErrorName(HttpRequestException ex)
        {
            return ex.StatusCode switch
            {
                null => $"{ex.Message}",
                HttpStatusCode.BadRequest => "400 - リクエストが無効です。",
                HttpStatusCode.Unauthorized => "401 - 認証に失敗しました。",
                HttpStatusCode.PaymentRequired => "402 - 決済が必要です。(実験的機能)",
                HttpStatusCode.Forbidden => "403 - アクセス拒否",
                HttpStatusCode.NotFound => "404 - リソースが存在しません。",
                HttpStatusCode.MethodNotAllowed => "405 - リクエストメゾットは無効化されています。",
                HttpStatusCode.NotAcceptable => "406 - 要求されたページをクライアントが受け入れ可能な形式で送信することができません。",
                HttpStatusCode.ProxyAuthenticationRequired => "407 - プロキシ認証が必要です。",
                HttpStatusCode.RequestTimeout => "408 - リクエストがタイムアウトしました。",
                HttpStatusCode.Conflict => "409 - リクエストとサーバーの状態に矛盾しています。",
                HttpStatusCode.Gone => "410 - このリソースは削除されました。",
                HttpStatusCode.LengthRequired => "411 - Content-Lengthを定義してください。",
                HttpStatusCode.PreconditionFailed => "412 - サーバー上で適合しない条件が、クライアントのヘッダーに含まれています。",
                HttpStatusCode.RequestEntityTooLarge => "413 - リクエスト上限に到達しています。",
                HttpStatusCode.RequestUriTooLong => "414 - リクエストURLがサーバーで扱える長さを超えています。",
                HttpStatusCode.UnsupportedMediaType => "415 - リクエストされたメディア形式がサポートされていない為、拒否されました。",
                HttpStatusCode.RequestedRangeNotSatisfiable => "416 - RangeヘッダーFieldで指定された範囲を満たすことができませんでした。",
                HttpStatusCode.ExpectationFailed => "417 - Expectリクエストヘッダーで指定された内容がサーバー側と適合しませんでした。",
                (HttpStatusCode)418 => "418 - 私はティーポットです。コーヒーを入れることはできません。(ジョークです)",
                HttpStatusCode.MisdirectedRequest => "421 - リクエストはレスポンスを生成できないサーバーに送られました。",
                HttpStatusCode.UnprocessableEntity => "422 - リクエストは適正ですが、意味が誤っているために従うことができません。",
                HttpStatusCode.Locked => "423 - アクセス中のリソースはロックされています。",
                HttpStatusCode.FailedDependency => "424 - 前回のリクエストが失敗している為、このリクエストも失敗しました。",
                (HttpStatusCode)425 => "425 - 繰り返される可能性のあるリクエストを拒否しました。(実験的)",
                HttpStatusCode.UpgradeRequired => "426 - 別のプロトコルにアップグレードが必要です。",
                HttpStatusCode.PreconditionRequired => "428 - オリジンサーバーはリクエストが条件付きになることを必要としています。",
                HttpStatusCode.TooManyRequests => "429 - リクエストのレート制限に到達しました。時間をおいて再度実行してください。",
                HttpStatusCode.RequestHeaderFieldsTooLarge => "431 - ヘッダーフィールドが大きすぎる為、リクエストを拒否しました。",
                HttpStatusCode.UnavailableForLegalReasons => "451 - 検閲されたサーバーにリクエストしようとしました。",
                (HttpStatusCode)499 => "499 - CloudFlare:要求処理中にクライアントが閉じられました。これは通常発生しないメッセージです。",
                HttpStatusCode.InternalServerError => "500 - サーバー内部エラー",
                HttpStatusCode.NotImplemented => "501 - サーバーがリクエストメゾットに対応していません。",
                HttpStatusCode.BadGateway => "502 - サーバー内での通信エラー",
                HttpStatusCode.ServiceUnavailable => "503 - サーバーは現在利用できません。",
                HttpStatusCode.GatewayTimeout => "504 - ゲートウェイサーバーがタイムアウトしました。",
                HttpStatusCode.HttpVersionNotSupported => "505 - リクエストされたHTTPバージョンはサーバー上では対応していません。",
                HttpStatusCode.VariantAlsoNegotiates => "506 - サーバー内部構成エラー",
                HttpStatusCode.InsufficientStorage => "507 - リクエストを処理するために必要なストレージの容量が足りませんでした。",
                HttpStatusCode.LoopDetected => "508 - リダイレクトループが検出されました。",
                (HttpStatusCode)509 => "509 - サーバーの転送量が上限に達しました。",
                HttpStatusCode.NotExtended => "510 - サーバーがリクエストを処理するために、リクエストをさらに拡張することが必要です。",
                HttpStatusCode.NetworkAuthenticationRequired => "511 - ネットワークにアクセスするには認証が必要です。",
                (HttpStatusCode)520 => "520 - CloudFlare:Webサーバーが不明なエラーを返しました。",
                (HttpStatusCode)521 => "521 - CloudFlare:サーバーはオフラインです。",
                (HttpStatusCode)522 => "522 - CloudFlare:サーバーへの接続がタイムアウトしました。",
                (HttpStatusCode)523 => "523 - CloudFlare:発信元に到達することができませんでした。",
                (HttpStatusCode)524 => "524 - CloudFlare:内部サーバー処理がタイムアウトしました。",
                (HttpStatusCode)525 => "525 - CloudFlare:SSLハンドシェイクに失敗しました。",
                (HttpStatusCode)526 => "526 - CloudFlare:無効なSSL証明書です。",
                (HttpStatusCode)527 => "527 - CloudFlare:Railgun Listener to origin error",
                (HttpStatusCode)530 => $"530 - CloudFlare:CloudFlareによるエラーです。情報は利用できません。",
                _ => $"{(int)ex.StatusCode} - {ex.StatusCode}",
            };
        }
    }
}
