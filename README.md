
# MisakiEQ

> ## MisakiEQとは

MisakiEQはWindows上で動作する地震通知アプリケーションです。  
緊急地震速報・地震情報・津波情報を随時通知・表示します。  
また、リアルタイムの揺れ情報の確認もできる強震モニタも備わっております。  
注意：当アプリケーションはWindows10の最新バージョン及びWindows11に対応しております。  
古いWindows10には対応していません！(エラーが出て起動できません)  
また、高DPIには対応しておりません。

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
|地震情報取得|15秒/回|3秒/回(共通)|WebSocket実装予定|
|津波情報取得|60秒/回|3秒/回(共通)|WebSocket実装予定|
|情報取得の遅延|45秒程度|5秒程度|
|緊急地震速報の取得|1回/秒|WebSocket対応|DMDATA.jpの契約が必須条件|
|1度の取得処理数|1回|複数(理論値)|
|COM通信|一部対応|未対応|双方向対応予定|
|イベントビューア送信|なし|対応済|初回起動のみ管理者権限必須|
|サウンド再生|対応|対応済|
|アナウンス再生|未対応|未対応|実装予定(未定)|
|強震モニタの同時再生|なし|未対応|実装予定
|強震モニタ再生|一部対応|全データに対応|
|強震モニタ自動調整|対応|対応|
|推定震度計算|座標のみ|緯経度|
|地震情報の詳細表示|制限あり(通知のみ)|制限あり(通知のみ)|実装予定|

> ## Twitter Bot

新バージョン移行に伴い、少ないツイートでより多くの情報を入手できるようになります。  
ツイートロジックも全般的に見直し、効率的にデータの発信ができるようになります。  
現在発生している以下の不具合も解決されます。  

- [x] 複数の地震情報で震度情報がツイートされない不具合  
- [x] 複数の緊急地震速報が交互に発信した場合スレッド化されない不具合  
- [x] 短時間でより多くのツイートの処理  

また、Botは以下のリンクから閲覧できます。

[MisakiEQ Twitter Bot](http://twitter.com/MisakiEQ "@MisakiEQ")  
> ## 利用規約
使用する場合は以下の内容に同意してから利用して頂くようお願いいたします。
- アプリの解析・リバースエンジニアリングした内容を公開するのはおやめください。
- アプリ内で使用されているキャラクター・イラスト・音源・ソフトウェアは作者である「水咲」とそのコンテンツ作者の著作物です。
- このアプリを金銭が伴う第三者による紹介・販売等、営利目的として利用しないでください。  
  ただしYouTubeのGoogle Adsence、ニコニコ動画のクリエイター奨励プログラム等、  
  配信規約に書いている配信プラットフォームからの間接的な収益はOKとします。
- アプリ内のイラスト、音源等を再利用しないでください。
- 作者にコーヒー(~~Ko-Fiだけに~~)や焼き肉を奢ることができます。
- このアプリケーションによってユーザーがいかなる不利益を生じても、作者は一切の責任を負わないものとします。

2022年6月21日 第1版 制定  
2022年8月15日 第2版 改訂
> ## 配信規約
### 作者として、このアプリを観測地点として配信するのは大変歓迎いたします。
以下の内容にご留意して頂いた上でコンテンツの配信をお願いいたします。
- コンテンツの公開はYouTube、ニコニコ動画(生放送)、Twitch、bilibili動画に限られます。
- 公序良俗に反する内容、またはそれに該当されるであろうコンテンツ、配信サイトには使用できません。
- 本来の目的(災害情報)以外に使用することはできません。
- 動画の概要欄や配信画面内、もしくは紹介動画内には必ず「MisakiEQ」を使用していることを明記してください。
- リンクを含める場合は必ずこちらのリンクを使用してください
「 https://github.com/Misaki0331/MisakiEQv2 」
- **自身が地震観測者として、SNSを通じての発信など節度ある行動を保ってください。**

2022年6月21日 第1版 制定  
2022年8月15日 第2版 改訂

> ## 謝辞  

### 地震APIやライブラリは以下のサイトから利用しております  

サービスを提供して頂いた方に深く感謝申し上げます。

> #### API  

- ~~[api.iedred7584.com](https://iedred7584.dev) iedred7584様 (高度利用者向け緊急地震速報の提供)~~ 提供終了
- [Project DM(Disaster Mitigation)-Data Send Service](https://dmdata.jp/) 緊急地震速報の生データ提供
- [P2Pquake.net](https://p2pquake.net) たくや様 (地震・津波情報の提供)
- [kmoni.bosai.go.jp](http://kmoni.bosai.go.jp) 防災科学技術研究所様 (強震モニタの提供)  

> #### ライブラリ  

- [多項式補間を使用して強震モニタ画像から数値データを決定する](https://qiita.com/NoneType1/items/a4d2cf932e20b56ca444) NoneType1様 (強震モニタの値の取得に使用)
- [KyoshinEewViewerIngen](https://github.com/ingen084/KyoshinEewViewerIngen/) ingen084様 (強震モニタでの観測地点データの使用)
- [KyoshinMonitorLib](https://github.com/ingen084/KyoshinMonitorLib) ingen084様 (上記のデータ解凍に使用)
- [TweetinviAPI](https://github.com/linvi/tweetinvi) (TwitterBotの製作に使用)
- [DiscordRichPresence](https://github.com/Lachee/discord-rpc-csharp) (Discord RPCのデータ更新に使用)
- [Microsoft.Toolkit.Uwp.Notificaitions](https://github.com/CommunityToolkit/WindowsCommunityToolkit) (Windows10以降の通知のカスタマイズ)
- [Newtonsoft.Json](https://www.newtonsoft.com/json) (APIに使用するJSON関係の処理)  

> #### 素材
- サウンド/SFX : Yokkin様 [Twitter](https://twitter.com/froggie3_)
- アイコンイラスト : あめ様 [Twitter](https://twitter.com/OgG4e)
- アプリアイコン・ヘッダー : 拝隠様 [Twitter](https://twitter.com/OGAMI_NABARI)

> ## リンク集
- [作者のTwitter](https://twitter.com/0x7FF)
- [Ko-Fi](https://ko-fi.com/Misaki0331)
