# MisakiEQ

> ## MisakiEQとは

MisakiEQはWindows上で動作する地震通知アプリケーションです。  
緊急地震速報・地震情報・津波情報を随時通知・表示します。  
また、リアルタイムの揺れ情報の確認もできる強震モニタも備わっております。  
注意：当アプリケーションはWindows10の最新バージョンのみ対応しております。  
古いWindows10には対応していません！(エラーが出て起動できません)  

> ## 前バージョンのMisakiEQとの違いは？

- コードがより綺麗になりました。  
- 非同期処理化によりもっさり感が減少
- .NET Frameworks4.0 から .NET6 に移行  
- ソフトウェアの安定性の向上  
- ビジュアルの大幅な変更  
- 地震情報の取得がよりリアルタイムに  
- 通信量の削減  
- 緯度経度から推定震度の計測が可能に  
- 強震モニタの値が取得可能に  
- Twitter API v2に対応 **←ここが一番重要だったりする**  
- ログをより見やすくデバッグ作業の効率向上  
- 容量の圧縮化  
 他色々  

> ## 対応状況

||旧MisakiEQ|新MisakiEQ|対応予定|
|-|-|-|-|
|地震情報取得|15秒/回|3秒/回(共通)|
|津波情報取得|60秒/回|3秒/回(共通)|
|情報取得の遅延|45秒程度|5秒程度|
|緊急地震速報の取得|1回/秒|都度調節可|
|1度の取得処理数|1回|可変(データ量による)|
|COM通信|一部対応|未対応|双方向対応予定|
|イベントビューア送信|なし|未対応|対応済|
|サウンド再生|対応|対応済|
|アナウンス再生|未対応|未対応|実装予定(未定)|
|強震モニタ再生|一部対応|全データに対応|
|強震モニタ自動調整|対応|対応|
|推定震度計算|座標のみ|緯経度|

> ## Twitter Bot

新バージョン移行に伴い、少ないツイートでより多くの情報を入手できるようになります。  
ツイートロジックも全般的に見直し、効率的にデータの発信ができるようになります。  
現在発生している以下の不具合も解決されます。  

- [x] 複数の地震情報で震度情報がツイートされない不具合  
- [x] 複数の緊急地震速報が交互に発信した場合スレッド化されない不具合  
- [x] 短時間でより多くのツイートの処理  

また、Botは以下のリンクから閲覧できます。

[MisakiEQ Twitter Bot](http://twitter.com/MisakiEQ "@MisakiEQ")  

> ## 謝辞  

### 地震APIやライブラリは以下のサイトから利用しております  

> #### API  

- [api.iedred7584.com](https://iedred7584.dev) iedred7584様 (高度利用者向け緊急地震速報の提供)  
- [P2Pquake.net](https://p2pquake.net) たくや様 (地震・津波情報の提供)
- [kmoni.bosai.go.jp](http://kmoni.bosai.go.jp) 防災科学技術研究所様 (強震モニタの提供)  

> #### ライブラリ  

- [多項式補間を使用して強震モニタ画像から数値データを決定する](https://qiita.com/NoneType1/items/a4d2cf932e20b56ca444) NoneType1様 (強震モニタの値の取得に使用)
- [TweetinviAPI](https://github.com/linvi/tweetinvi) (TwitterBotの製作に使用)
- [DiscordRichPresence](https://github.com/Lachee/discord-rpc-csharp) (Discord RPCのデータ更新に使用)
- [Microsoft.Toolkit.Uwp.Notificaitions](https://github.com/CommunityToolkit/WindowsCommunityToolkit) (Windows10以降の通知のカスタマイズ)
- [Newtonsoft.Json](https://www.newtonsoft.com/json) (APIに使用するJSON関係の処理)  
