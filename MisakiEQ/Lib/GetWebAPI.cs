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
            catch (WebException ex)
            {

                string State = "";
                switch (ex.Status)
                {
                    case WebExceptionStatus.CacheEntryNotFound:
                        State = $"{ex.Status} - 指定したキャッシュ エントリが見つかりませんでした。";
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        State = $"{ex.Status} - 接続に失敗しました。ファイアウォールやプロキシがブロックしている可能性があります。";
                        break;
                    case WebExceptionStatus.ConnectionClosed:
                        State = $"{ex.Status} - 接続を終了するのが早すぎました。";
                        break;
                    case WebExceptionStatus.KeepAliveFailure:
                        State = $"{ex.Status} - Keep-alive ヘッダーを指定する要求のための接続が予期せずに閉じられました。";
                        break;
                    case WebExceptionStatus.MessageLengthLimitExceeded:
                        State = $"{ex.Status} - 要求の送信時またはサーバーから応答の受信時に指定された制限を超えるメッセージが受信されました。";
                        break;
                    case WebExceptionStatus.NameResolutionFailure:
                        State = $"{ex.Status} - 名前解決サービスがホスト名を解決できませんでした。";
                        break;
                    case WebExceptionStatus.Pending:
                        State = $"{ex.Status} - 内部非同期要求が保留中です。";
                        break;
                    case WebExceptionStatus.PipelineFailure:
                        State = $"{ex.Status} - 要求がパイプライン処理された要求で、応答の受信前に接続が閉じられました。";
                        break;
                    case WebExceptionStatus.ProxyNameResolutionFailure:
                        State = $"{ex.Status} - ネーム リゾルバー サービスがプロキシ ホスト名を解決できませんでした。";
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        State = $"{ex.Status} - 完全な応答がリモート サーバーから受信されませんでした。";
                        break;
                    case WebExceptionStatus.RequestCanceled:
                        State = $"{ex.Status} - 要求が取り消されたか、分類できないエラーです。";
                        break;
                    case WebExceptionStatus.RequestProhibitedByCachePolicy:
                        State = $"{ex.Status} - 要求はキャッシュ ポリシーで許可されませんでした。";
                        break;
                    case WebExceptionStatus.RequestProhibitedByProxy:
                        State = $"{ex.Status} - この要求はプロキシで許可されませんでした。";
                        break;
                    case WebExceptionStatus.SecureChannelFailure:
                        State = $"{ex.Status} - SSL を使用して接続を確立する際にエラーが発生しました。";
                        break;
                    case WebExceptionStatus.SendFailure:
                        State = $"{ex.Status} - リモート サーバーに完全な要求を送信できませんでした。";
                        break;
                    case WebExceptionStatus.ServerProtocolViolation:
                        State = $"{ex.Status} - サーバーの応答が有効な HTTP 応答ではありません。";
                        break;
                    case WebExceptionStatus.Success:
                        State = $"{ex.Status} - ステータスは成功していますが、例外エラーが発生しました。";
                        State += "\n" + ex;
                        break;
                    case WebExceptionStatus.Timeout:
                        State = $"{ex.Status} - 要求のタイムアウト時間中に応答が受信されませんでした。";
                        break;
                    case WebExceptionStatus.TrustFailure:
                        State = $"{ex.Status} - サーバー証明書を検証できませんでした。";
                        break;
                    case WebExceptionStatus.ProtocolError:
                        if (ex.Response == null)
                        {
                            State = $"{ex.Status} - プロトコルエラーですが、ヘッダーが空です。";
                            return string.Empty;
                        }
                        HttpWebResponse errres = (HttpWebResponse)ex.Response;
                        
                        break;
                    case WebExceptionStatus.UnknownError:
                        State = $"{ex.Status} - 不明な種類の例外が発生しました。";
                        State += $"\n{ex}";
                        break;
                    default:
                        State = $"不明な例外が発生しました。";
                        State += $"\n{ex}";
                        break;

                }
                log.Error(State);
                throw new WebException(State);
                //return string.Empty;
            }
            catch (HttpRequestException ex)
            {
                string State = ex.StatusCode switch
                {
                    null => "Undefined - 未定義です。",
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

                log.Error($"{State} 参照元:\"{URL}\"");
                return string.Empty;

            }
            catch (TaskCanceledException ex)
            {
                log.Info("タスクが取り消されました。");
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return string.Empty;
            }
        }
    }
}
