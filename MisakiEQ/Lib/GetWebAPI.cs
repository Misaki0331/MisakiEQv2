using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Log;

namespace MisakiEQ.Lib
{
    internal static class WebAPI
    {
        static readonly Logger log = Logger.GetInstance();
        public static async Task<string> GetString(string URL,CancellationToken? token=null)
        {
            try
            {
                if (URL == "")
                    throw new ArgumentException("ダウンロード先のURLが指定されていません。");
                using HttpClient webClient = new();
                Task<string> stream;
                if (token != null)
                {
                    stream = webClient.GetStringAsync(new Uri(URL), (CancellationToken)token);
                }
                else
                {
                    stream = webClient.GetStringAsync(new Uri(URL));
                }
                await stream;
                if (stream.IsFaulted)
                {
                    if (stream.Exception != null)
                    {
                        throw stream.Exception;
                    }
                    else
                    {
                        throw new ArgumentException("例外エラーが発生しましたが、例外は返されませんでした。");
                    }
                }
                return stream.Result;
            }
            catch (HttpRequestException ex)
            {
                string State = ex.StatusCode switch
                {
                    null => $"{ex.Message}",
                    HttpStatusCode.BadRequest => "400 - リクエスト構文が無効です。",
                    HttpStatusCode.Unauthorized => "401 - 認証の必要があります。",
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
                    HttpStatusCode.TooManyRequests => "429 - リクエストのレート制限に到達しました。",
                    HttpStatusCode.RequestHeaderFieldsTooLarge => "431 - ヘッダーフィールドが大きすぎる為、リクエストを拒否しました。",
                    HttpStatusCode.UnavailableForLegalReasons => "451 - 検閲されたサーバーにリクエストしようとしました。",
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
                    _ => $"{(int)ex.StatusCode} - {ex.StatusCode}",
                };

                throw new FileLoadException($"{State} 参照元:\"{URL}\"");
                //return string.Empty;

            }
            catch (TaskCanceledException ex)
            {
                log.Info("タスクが取り消されました。");
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                await Task.FromException<string>(ex);
                return string.Empty;
            }
        }
    }
}
