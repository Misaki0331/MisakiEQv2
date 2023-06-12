
# MisakiEQ

> ## MisakiEQとは

MisakiEQはWindows上で動作する開発途上の地震通知アプリケーションです。  
緊急地震速報・地震情報・津波情報・J-ALERTを随時通知・表示します。
また、リアルタイムの揺れ情報の確認もできる強震モニタも備わっております。  


> ## 対応状況

[x]緊急地震速報のWebSocket対応(DMDATA.jpの契約が必須)
[ ]地震情報・津波情報のWebSocket対応
[x]J-ALERTの受信対応
[ ]COM通信対応
[x]イベントの送信
[x]サウンドの再生
[ ]アナウンス再生(声優募集中)
[x]強震モニタの再生
[x]強震モニタの自動時刻調整
[x]強震モニタからの推定震度計算
[ ]強震モニタの同時再生
[ ]過去の地震情報の表示


> ## Misskey Bot

現在Misskey.io上に地震Botを置かせていただいております。
Misskey上ではレートリミットの対策により一定時間ごとに配信するようにしております。

また、Botは以下のリンクから閲覧できます。

[MisakiEQ Misskey Bot](http://misskey.io/@MisakiEQ "@MisakiEQ")  

> ## 利用規約
使用する場合は以下の内容に同意してから利用して頂くようお願いいたします。
- アプリの解析・リバースエンジニアリングした内容を公開するのはおやめください。
- アプリ内で使用されているキャラクター・イラスト・音源・ソフトウェアは作者である「水咲」とそのコンテンツ作者の著作物です。
- このアプリを金銭が伴う第三者による紹介・販売等、営利目的として利用しないでください。  
  ただしYouTubeのGoogle Adsence、ニコニコ動画のクリエイター奨励プログラム等、  
  配信規約に書いている配信プラットフォームからの間接的な収益はOKとします。
- アプリ内のイラスト、音源等を再利用しないでください。
- 作者に寄付を送ることができます。
- このアプリケーションによってユーザーがいかなる不利益を生じても、作者は一切の責任を負わないものとします。

2022年6月21日 第1版 制定  
2022年8月15日 第2版 改訂
2023年6月12日 第3版 改訂

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
- [作者のMisskey](https://misskey.io/@ms)
- [Misskey Bot](https://misskey.io/@MisakiEQ)
- [Ko-Fi](https://ko-fi.com/Misaki0331)
