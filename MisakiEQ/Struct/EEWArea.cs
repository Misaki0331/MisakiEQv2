using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    internal class EEWArea
    {
        #region Enum list
        public enum Regions
        {
            /// <summary>不明</summary>
            Unknown = -1,
            /// <summary>石狩地方北部</summary>
            Area100 = 100,
            /// <summary>石狩地方中部</summary>
            Area101 = 101,
            /// <summary>石狩地方南部</summary>
            Area102 = 102,
            /// <summary>渡島地方北部</summary>
            Area105 = 105,
            /// <summary>渡島地方東部</summary>
            Area106 = 106,
            /// <summary>渡島地方西部</summary>
            Area107 = 107,
            /// <summary>檜山地方</summary>
            Area110 = 110,
            /// <summary>後志地方北部</summary>
            Area115 = 115,
            /// <summary>後志地方東部</summary>
            Area116 = 116,
            /// <summary>後志地方西部</summary>
            Area117 = 117,
            /// <summary>北海道奥尻島</summary>
            Area119 = 119,
            /// <summary>空知地方北部</summary>
            Area120 = 120,
            /// <summary>空知地方中部</summary>
            Area121 = 121,
            /// <summary>空知地方南部</summary>
            Area122 = 122,
            /// <summary>上川地方北部</summary>
            Area125 = 125,
            /// <summary>上川地方中部</summary>
            Area126 = 126,
            /// <summary>上川地方南部</summary>
            Area127 = 127,
            /// <summary>留萌地方中北部</summary>
            Area130 = 130,
            /// <summary>留萌地方南部</summary>
            Area131 = 131,
            /// <summary>宗谷地方北部</summary>
            Area135 = 135,
            /// <summary>宗谷地方南部</summary>
            Area136 = 136,
            /// <summary>北海道利尻礼文</summary>
            Area139 = 139,
            /// <summary>網走地方</summary>
            Area140 = 140,
            /// <summary>北見地方</summary>
            Area141 = 141,
            /// <summary>紋別地方</summary>
            Area142 = 142,
            /// <summary>胆振地方西部</summary>
            Area145 = 145,
            /// <summary>胆振地方中東部</summary>
            Area146 = 146,
            /// <summary>日高地方西部</summary>
            Area150 = 150,
            /// <summary>日高地方中部</summary>
            Area151 = 151,
            /// <summary>日高地方東部</summary>
            Area152 = 152,
            /// <summary>十勝地方北部</summary>
            Area155 = 155,
            /// <summary>十勝地方中部</summary>
            Area156 = 156,
            /// <summary>十勝地方南部</summary>
            Area157 = 157,
            /// <summary>釧路地方北部</summary>
            Area160 = 160,
            /// <summary>釧路地方中南部</summary>
            Area161 = 161,
            /// <summary>根室地方北部</summary>
            Area165 = 165,
            /// <summary>根室地方中部</summary>
            Area166 = 166,
            /// <summary>根室地方南部</summary>
            Area167 = 167,
            /// <summary>青森県津軽北部</summary>
            Area200 = 200,
            /// <summary>青森県津軽南部</summary>
            Area201 = 201,
            /// <summary>青森県三八上北</summary>
            Area202 = 202,
            /// <summary>青森県下北</summary>
            Area203 = 203,
            /// <summary>岩手県沿岸北部</summary>
            Area210 = 210,
            /// <summary>岩手県沿岸南部</summary>
            Area211 = 211,
            /// <summary>岩手県内陸北部</summary>
            Area212 = 212,
            /// <summary>岩手県内陸南部</summary>
            Area213 = 213,
            /// <summary>宮城県北部</summary>
            Area220 = 220,
            /// <summary>宮城県南部</summary>
            Area221 = 221,
            /// <summary>宮城県中部</summary>
            Area222 = 222,
            /// <summary>秋田県沿岸北部</summary>
            Area230 = 230,
            /// <summary>秋田県沿岸南部</summary>
            Area231 = 231,
            /// <summary>秋田県内陸北部</summary>
            Area232 = 232,
            /// <summary>秋田県内陸南部</summary>
            Area233 = 233,
            /// <summary>山形県庄内</summary>
            Area240 = 240,
            /// <summary>山形県最上</summary>
            Area241 = 241,
            /// <summary>山形県村山</summary>
            Area242 = 242,
            /// <summary>山形県置賜</summary>
            Area243 = 243,
            /// <summary>福島県中通り</summary>
            Area250 = 250,
            /// <summary>福島県浜通り</summary>
            Area251 = 251,
            /// <summary>福島県会津</summary>
            Area252 = 252,
            /// <summary>茨城県北部</summary>
            Area300 = 300,
            /// <summary>茨城県南部</summary>
            Area301 = 301,
            /// <summary>栃木県北部</summary>
            Area310 = 310,
            /// <summary>栃木県南部</summary>
            Area311 = 311,
            /// <summary>群馬県北部</summary>
            Area320 = 320,
            /// <summary>群馬県南部</summary>
            Area321 = 321,
            /// <summary>埼玉県北部</summary>
            Area330 = 330,
            /// <summary>埼玉県南部</summary>
            Area331 = 331,
            /// <summary>埼玉県秩父</summary>
            Area332 = 332,
            /// <summary>千葉県北東部</summary>
            Area340 = 340,
            /// <summary>千葉県北西部</summary>
            Area341 = 341,
            /// <summary>千葉県南部</summary>
            Area342 = 342,
            /// <summary>東京都２３区</summary>
            Area350 = 350,
            /// <summary>東京都多摩東部</summary>
            Area351 = 351,
            /// <summary>東京都多摩西部</summary>
            Area352 = 352,
            /// <summary>神津島</summary>
            Area354 = 354,
            /// <summary>伊豆大島</summary>
            Area355 = 355,
            /// <summary>新島</summary>
            Area356 = 356,
            /// <summary>三宅島</summary>
            Area357 = 357,
            /// <summary>八丈島</summary>
            Area358 = 358,
            /// <summary>小笠原</summary>
            Area359 = 359,
            /// <summary>神奈川県東部</summary>
            Area360 = 360,
            /// <summary>神奈川県西部</summary>
            Area361 = 361,
            /// <summary>新潟県上越</summary>
            Area370 = 370,
            /// <summary>新潟県中越</summary>
            Area371 = 371,
            /// <summary>新潟県下越</summary>
            Area372 = 372,
            /// <summary>新潟県佐渡</summary>
            Area375 = 375,
            /// <summary>富山県東部</summary>
            Area380 = 380,
            /// <summary>富山県西部</summary>
            Area381 = 381,
            /// <summary>石川県能登</summary>
            Area390 = 390,
            /// <summary>石川県加賀</summary>
            Area391 = 391,
            /// <summary>福井県嶺北</summary>
            Area400 = 400,
            /// <summary>福井県嶺南</summary>
            Area401 = 401,
            /// <summary>山梨県中・西部</summary>
            Area411 = 411,
            /// <summary>山梨県東部・富士五湖</summary>
            Area412 = 412,
            /// <summary>長野県北部</summary>
            Area420 = 420,
            /// <summary>長野県中部</summary>
            Area421 = 421,
            /// <summary>長野県南部</summary>
            Area422 = 422,
            /// <summary>岐阜県飛騨</summary>
            Area430 = 430,
            /// <summary>岐阜県美濃東部</summary>
            Area431 = 431,
            /// <summary>岐阜県美濃中西部</summary>
            Area432 = 432,
            /// <summary>静岡県伊豆</summary>
            Area440 = 440,
            /// <summary>静岡県東部</summary>
            Area441 = 441,
            /// <summary>静岡県中部</summary>
            Area442 = 442,
            /// <summary>静岡県西部</summary>
            Area443 = 443,
            /// <summary>愛知県東部</summary>
            Area450 = 450,
            /// <summary>愛知県西部</summary>
            Area451 = 451,
            /// <summary>三重県北部</summary>
            Area460 = 460,
            /// <summary>三重県中部</summary>
            Area461 = 461,
            /// <summary>三重県南部</summary>
            Area462 = 462,
            /// <summary>滋賀県北部</summary>
            Area500 = 500,
            /// <summary>滋賀県南部</summary>
            Area501 = 501,
            /// <summary>京都府北部</summary>
            Area510 = 510,
            /// <summary>京都府南部</summary>
            Area511 = 511,
            /// <summary>大阪府北部</summary>
            Area520 = 520,
            /// <summary>大阪府南部</summary>
            Area521 = 521,
            /// <summary>兵庫県北部</summary>
            Area530 = 530,
            /// <summary>兵庫県南東部</summary>
            Area531 = 531,
            /// <summary>兵庫県南西部</summary>
            Area532 = 532,
            /// <summary>兵庫県淡路島</summary>
            Area535 = 535,
            /// <summary>奈良県</summary>
            Area540 = 540,
            /// <summary>和歌山県北部</summary>
            Area550 = 550,
            /// <summary>和歌山県南部</summary>
            Area551 = 551,
            /// <summary>鳥取県東部</summary>
            Area560 = 560,
            /// <summary>鳥取県中部</summary>
            Area562 = 562,
            /// <summary>鳥取県西部</summary>
            Area563 = 563,
            /// <summary>島根県東部</summary>
            Area570 = 570,
            /// <summary>島根県西部</summary>
            Area571 = 571,
            /// <summary>島根県隠岐</summary>
            Area575 = 575,
            /// <summary>岡山県北部</summary>
            Area580 = 580,
            /// <summary>岡山県南部</summary>
            Area581 = 581,
            /// <summary>広島県北部</summary>
            Area590 = 590,
            /// <summary>広島県南東部</summary>
            Area591 = 591,
            /// <summary>広島県南西部</summary>
            Area592 = 592,
            /// <summary>徳島県北部</summary>
            Area600 = 600,
            /// <summary>徳島県南部</summary>
            Area601 = 601,
            /// <summary>香川県東部</summary>
            Area610 = 610,
            /// <summary>香川県西部</summary>
            Area611 = 611,
            /// <summary>愛媛県東予</summary>
            Area620 = 620,
            /// <summary>愛媛県中予</summary>
            Area621 = 621,
            /// <summary>愛媛県南予</summary>
            Area622 = 622,
            /// <summary>高知県東部</summary>
            Area630 = 630,
            /// <summary>高知県中部</summary>
            Area631 = 631,
            /// <summary>高知県西部</summary>
            Area632 = 632,
            /// <summary>山口県北部</summary>
            Area700 = 700,
            /// <summary>山口県西部</summary>
            Area702 = 702,
            /// <summary>山口県東部</summary>
            Area703 = 703,
            /// <summary>山口県中部</summary>
            Area704 = 704,
            /// <summary>福岡県福岡</summary>
            Area710 = 710,
            /// <summary>福岡県北九州</summary>
            Area711 = 711,
            /// <summary>福岡県筑豊</summary>
            Area712 = 712,
            /// <summary>福岡県筑後</summary>
            Area713 = 713,
            /// <summary>佐賀県北部</summary>
            Area720 = 720,
            /// <summary>佐賀県南部</summary>
            Area721 = 721,
            /// <summary>長崎県北部</summary>
            Area730 = 730,
            /// <summary>長崎県南西部</summary>
            Area731 = 731,
            /// <summary>長崎県島原半島</summary>
            Area732 = 732,
            /// <summary>長崎県対馬</summary>
            Area735 = 735,
            /// <summary>長崎県壱岐</summary>
            Area736 = 736,
            /// <summary>長崎県五島</summary>
            Area737 = 737,
            /// <summary>熊本県阿蘇</summary>
            Area740 = 740,
            /// <summary>熊本県熊本</summary>
            Area741 = 741,
            /// <summary>熊本県球磨</summary>
            Area742 = 742,
            /// <summary>熊本県天草・芦北</summary>
            Area743 = 743,
            /// <summary>大分県北部</summary>
            Area750 = 750,
            /// <summary>大分県中部</summary>
            Area751 = 751,
            /// <summary>大分県南部</summary>
            Area752 = 752,
            /// <summary>大分県西部</summary>
            Area753 = 753,
            /// <summary>宮崎県北部平野部</summary>
            Area760 = 760,
            /// <summary>宮崎県北部山沿い</summary>
            Area761 = 761,
            /// <summary>宮崎県南部平野部</summary>
            Area762 = 762,
            /// <summary>宮崎県南部山沿い</summary>
            Area763 = 763,
            /// <summary>鹿児島県薩摩</summary>
            Area770 = 770,
            /// <summary>鹿児島県大隅</summary>
            Area771 = 771,
            /// <summary>鹿児島県十島村</summary>
            Area774 = 774,
            /// <summary>鹿児島県甑島</summary>
            Area775 = 775,
            /// <summary>鹿児島県種子島</summary>
            Area776 = 776,
            /// <summary>鹿児島県屋久島</summary>
            Area777 = 777,
            /// <summary>鹿児島県奄美北部</summary>
            Area778 = 778,
            /// <summary>鹿児島県奄美南部</summary>
            Area779 = 779,
            /// <summary>沖縄県本島北部</summary>
            Area800 = 800,
            /// <summary>沖縄県本島中南部</summary>
            Area801 = 801,
            /// <summary>沖縄県久米島</summary>
            Area802 = 802,
            /// <summary>沖縄県大東島</summary>
            Area803 = 803,
            /// <summary>沖縄県宮古島</summary>
            Area804 = 804,
            /// <summary>沖縄県石垣島</summary>
            Area805 = 805,
            /// <summary>沖縄県与那国島</summary>
            Area806 = 806,
            /// <summary>沖縄県西表島</summary>
            Area807 = 807,
        }
        public enum LocalAreas
        {
            /// <summary>不明</summary>
            Unknown = -1,
            /// <summary>北海道道央</summary>
            Area9011 = 9011,
            /// <summary>北海道道南</summary>
            Area9012 = 9012,
            /// <summary>北海道道北</summary>
            Area9013 = 9013,
            /// <summary>北海道道東</summary>
            Area9014 = 9014,
            /// <summary>青森</summary>
            Area9020 = 9020,
            /// <summary>岩手</summary>
            Area9030 = 9030,
            /// <summary>宮城</summary>
            Area9040 = 9040,
            /// <summary>秋田</summary>
            Area9050 = 9050,
            /// <summary>山形</summary>
            Area9060 = 9060,
            /// <summary>福島</summary>
            Area9070 = 9070,
            /// <summary>茨城</summary>
            Area9080 = 9080,
            /// <summary>栃木</summary>
            Area9090 = 9090,
            /// <summary>群馬</summary>
            Area9100 = 9100,
            /// <summary>埼玉</summary>
            Area9110 = 9110,
            /// <summary>千葉</summary>
            Area9120 = 9120,
            /// <summary>東京</summary>
            Area9131 = 9131,
            /// <summary>伊豆諸島</summary>
            Area9132 = 9132,
            /// <summary>小笠原</summary>
            Area9133 = 9133,
            /// <summary>神奈川</summary>
            Area9140 = 9140,
            /// <summary>新潟</summary>
            Area9150 = 9150,
            /// <summary>富山</summary>
            Area9160 = 9160,
            /// <summary>石川</summary>
            Area9170 = 9170,
            /// <summary>福井</summary>
            Area9180 = 9180,
            /// <summary>山梨</summary>
            Area9190 = 9190,
            /// <summary>長野</summary>
            Area9200 = 9200,
            /// <summary>岐阜</summary>
            Area9210 = 9210,
            /// <summary>静岡</summary>
            Area9220 = 9220,
            /// <summary>愛知</summary>
            Area9230 = 9230,
            /// <summary>三重</summary>
            Area9240 = 9240,
            /// <summary>滋賀</summary>
            Area9250 = 9250,
            /// <summary>京都</summary>
            Area9260 = 9260,
            /// <summary>大阪</summary>
            Area9270 = 9270,
            /// <summary>兵庫</summary>
            Area9280 = 9280,
            /// <summary>奈良</summary>
            Area9290 = 9290,
            /// <summary>和歌山</summary>
            Area9300 = 9300,
            /// <summary>鳥取</summary>
            Area9310 = 9310,
            /// <summary>島根</summary>
            Area9320 = 9320,
            /// <summary>岡山</summary>
            Area9330 = 9330,
            /// <summary>広島</summary>
            Area9340 = 9340,
            /// <summary>徳島</summary>
            Area9360 = 9360,
            /// <summary>香川</summary>
            Area9370 = 9370,
            /// <summary>愛媛</summary>
            Area9380 = 9380,
            /// <summary>高知</summary>
            Area9390 = 9390,
            /// <summary>山口</summary>
            Area9350 = 9350,
            /// <summary>福岡</summary>
            Area9400 = 9400,
            /// <summary>佐賀</summary>
            Area9410 = 9410,
            /// <summary>長崎</summary>
            Area9420 = 9420,
            /// <summary>熊本</summary>
            Area9430 = 9430,
            /// <summary>大分</summary>
            Area9440 = 9440,
            /// <summary>宮崎</summary>
            Area9450 = 9450,
            /// <summary>鹿児島</summary>
            Area9461 = 9461,
            /// <summary>奄美(群島)</summary>
            Area9462 = 9462,
            /// <summary>沖縄本島</summary>
            Area9471 = 9471,
            /// <summary>大東島</summary>
            Area9472 = 9472,
            /// <summary>宮古島</summary>
            Area9473 = 9473,
            /// <summary>八重山</summary>
            Area9474 = 9474,

        }
        public enum District
        {
            /// <summary>不明</summary>
            Unknown = -1,
            /// <summary>北海道</summary>
            Area9910 = 9910,
            /// <summary>東北</summary>
            Area9920 = 9920,
            /// <summary>関東</summary>
            Area9931 = 9931,
            /// <summary>伊豆諸島</summary>
            Area9932 = 9932,
            /// <summary>小笠原</summary>
            Area9933 = 9933,
            /// <summary>北陸</summary>
            Area9934 = 9934,
            /// <summary>甲信</summary>
            Area9935 = 9935,
            /// <summary>東海</summary>
            Area9936 = 9936,
            /// <summary>近畿</summary>
            Area9941 = 9941,
            /// <summary>中国</summary>
            Area9942 = 9942,
            /// <summary>四国</summary>
            Area9943 = 9943,
            /// <summary>九州</summary>
            Area9951 = 9951,
            /// <summary>奄美</summary>
            Area9952 = 9952,
            /// <summary>沖縄</summary>
            Area9960 = 9960,

        }
        #endregion
        #region Enum to Enum
        /// <summary>
        /// 地域からローカルに変換
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static LocalAreas RegionToLocal(Regions reg)
        {
            return reg switch
            {
                Regions.Area100 or Regions.Area101 or Regions.Area102 or Regions.Area115 or Regions.Area116 or Regions.Area117 or Regions.Area120 or Regions.Area121 or Regions.Area122 => LocalAreas.Area9011,
                Regions.Area105 or Regions.Area106 or Regions.Area107 or Regions.Area110 or Regions.Area119 or Regions.Area145 or Regions.Area146 or Regions.Area150 or Regions.Area151 or Regions.Area152 => LocalAreas.Area9012,
                Regions.Area125 or Regions.Area126 or Regions.Area127 or Regions.Area130 or Regions.Area131 or Regions.Area135 or Regions.Area136 or Regions.Area139 => LocalAreas.Area9013,
                Regions.Area140 or Regions.Area141 or Regions.Area142 or Regions.Area155 or Regions.Area156 or Regions.Area157 or Regions.Area160 or Regions.Area161 or Regions.Area165 or Regions.Area166 or Regions.Area167 => LocalAreas.Area9014,
                Regions.Area200 or Regions.Area201 or Regions.Area202 or Regions.Area203 => LocalAreas.Area9020,
                Regions.Area210 or Regions.Area211 or Regions.Area212 or Regions.Area213 => LocalAreas.Area9030,
                Regions.Area220 or Regions.Area222 or Regions.Area221 => LocalAreas.Area9040,
                Regions.Area230 or Regions.Area231 or Regions.Area232 or Regions.Area233 => LocalAreas.Area9050,
                Regions.Area240 or Regions.Area241 or Regions.Area242 or Regions.Area243 => LocalAreas.Area9060,
                Regions.Area250 or Regions.Area251 or Regions.Area252 => LocalAreas.Area9070,
                Regions.Area300 or Regions.Area301 => LocalAreas.Area9080,
                Regions.Area310 or Regions.Area311 => LocalAreas.Area9090,
                Regions.Area320 or Regions.Area321 => LocalAreas.Area9100,
                Regions.Area330 or Regions.Area331 or Regions.Area332 => LocalAreas.Area9110,
                Regions.Area340 or Regions.Area341 or Regions.Area342 => LocalAreas.Area9120,
                Regions.Area350 or Regions.Area351 or Regions.Area352 => LocalAreas.Area9131,
                Regions.Area355 or Regions.Area356 or Regions.Area354 or Regions.Area357 or Regions.Area358 => LocalAreas.Area9132,
                Regions.Area359 => LocalAreas.Area9133,
                Regions.Area360 or Regions.Area361 => LocalAreas.Area9140,
                Regions.Area370 or Regions.Area371 or Regions.Area372 or Regions.Area375 => LocalAreas.Area9150,
                Regions.Area380 or Regions.Area381 => LocalAreas.Area9160,
                Regions.Area390 or Regions.Area391 => LocalAreas.Area9170,
                Regions.Area400 or Regions.Area401 => LocalAreas.Area9180,
                Regions.Area412 or Regions.Area411 => LocalAreas.Area9190,
                Regions.Area420 or Regions.Area421 or Regions.Area422 => LocalAreas.Area9200,
                Regions.Area430 or Regions.Area431 or Regions.Area432 => LocalAreas.Area9210,
                Regions.Area440 or Regions.Area441 or Regions.Area442 or Regions.Area443 => LocalAreas.Area9220,
                Regions.Area450 or Regions.Area451 => LocalAreas.Area9230,
                Regions.Area460 or Regions.Area461 or Regions.Area462 => LocalAreas.Area9240,
                Regions.Area500 or Regions.Area501 => LocalAreas.Area9250,
                Regions.Area510 or Regions.Area511 => LocalAreas.Area9260,
                Regions.Area520 or Regions.Area521 => LocalAreas.Area9270,
                Regions.Area530 or Regions.Area531 or Regions.Area532 or Regions.Area535 => LocalAreas.Area9280,
                Regions.Area540 => LocalAreas.Area9290,
                Regions.Area550 or Regions.Area551 => LocalAreas.Area9300,
                Regions.Area560 or Regions.Area562 or Regions.Area563 => LocalAreas.Area9310,
                Regions.Area570 or Regions.Area571 or Regions.Area575 => LocalAreas.Area9320,
                Regions.Area580 or Regions.Area581 => LocalAreas.Area9330,
                Regions.Area590 or Regions.Area591 or Regions.Area592 => LocalAreas.Area9340,
                Regions.Area700 or Regions.Area703 or Regions.Area704 or Regions.Area702 => LocalAreas.Area9350,
                Regions.Area600 or Regions.Area601 => LocalAreas.Area9360,
                Regions.Area610 or Regions.Area611 => LocalAreas.Area9370,
                Regions.Area620 or Regions.Area621 or Regions.Area622 => LocalAreas.Area9380,
                Regions.Area630 or Regions.Area631 or Regions.Area632 => LocalAreas.Area9390,
                Regions.Area710 or Regions.Area711 or Regions.Area712 or Regions.Area713 => LocalAreas.Area9400,
                Regions.Area720 or Regions.Area721 => LocalAreas.Area9410,
                Regions.Area730 or Regions.Area731 or Regions.Area732 or Regions.Area735 or Regions.Area736 or Regions.Area737 => LocalAreas.Area9420,
                Regions.Area740 or Regions.Area741 or Regions.Area742 or Regions.Area743 => LocalAreas.Area9430,
                Regions.Area750 or Regions.Area751 or Regions.Area752 or Regions.Area753 => LocalAreas.Area9440,
                Regions.Area760 or Regions.Area761 or Regions.Area762 or Regions.Area763 => LocalAreas.Area9450,
                Regions.Area770 or Regions.Area771 or Regions.Area774 or Regions.Area775 or Regions.Area776 or Regions.Area777 => LocalAreas.Area9461,
                Regions.Area778 or Regions.Area779 => LocalAreas.Area9462,
                Regions.Area800 or Regions.Area801 or Regions.Area802 => LocalAreas.Area9471,
                Regions.Area803 => LocalAreas.Area9472,
                Regions.Area804 => LocalAreas.Area9473,
                Regions.Area805 or Regions.Area806 or Regions.Area807 => LocalAreas.Area9474,
                _ => LocalAreas.Unknown,
            };
        }
        /// <summary>
        /// ローカルから地方に変換
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        public static District LocalToDistrict(LocalAreas reg)
        {
            return reg switch
            {
                LocalAreas.Area9011 or LocalAreas.Area9012 or LocalAreas.Area9013 or LocalAreas.Area9014 => District.Area9910,
                LocalAreas.Area9020 or LocalAreas.Area9030 or LocalAreas.Area9040 or LocalAreas.Area9050 or LocalAreas.Area9060 or LocalAreas.Area9070 => District.Area9920,
                LocalAreas.Area9080 or LocalAreas.Area9090 or LocalAreas.Area9100 or LocalAreas.Area9110 or LocalAreas.Area9120 or LocalAreas.Area9131 or LocalAreas.Area9140 => District.Area9931,
                LocalAreas.Area9132 => District.Area9932,
                LocalAreas.Area9133 => District.Area9933,
                LocalAreas.Area9160 or LocalAreas.Area9170 or LocalAreas.Area9180 => District.Area9934,
                LocalAreas.Area9150 or LocalAreas.Area9190 or LocalAreas.Area9200 => District.Area9935,
                LocalAreas.Area9210 or LocalAreas.Area9220 or LocalAreas.Area9230 or LocalAreas.Area9240 => District.Area9936,
                LocalAreas.Area9250 or LocalAreas.Area9260 or LocalAreas.Area9270 or LocalAreas.Area9280 or LocalAreas.Area9290 or LocalAreas.Area9300 => District.Area9941,
                LocalAreas.Area9310 or LocalAreas.Area9320 or LocalAreas.Area9330 or LocalAreas.Area9340 or LocalAreas.Area9350 => District.Area9942,
                LocalAreas.Area9360 or LocalAreas.Area9370 or LocalAreas.Area9380 or LocalAreas.Area9390 => District.Area9943,
                LocalAreas.Area9400 or LocalAreas.Area9410 or LocalAreas.Area9420 or LocalAreas.Area9430 or LocalAreas.Area9440 or LocalAreas.Area9450 or LocalAreas.Area9461 => District.Area9951,
                LocalAreas.Area9462 => District.Area9952,
                LocalAreas.Area9471 or LocalAreas.Area9472 or LocalAreas.Area9473 or LocalAreas.Area9474 => District.Area9960,
                _ => District.Unknown,
            };
        }
        #endregion
        #region String to Enum
        /// <summary>
        /// エリア名からenumに変換
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static Regions StrToRegion(string area)
        {
            return area switch
            {
                "石狩地方北部" => Regions.Area100,
                "石狩地方中部" => Regions.Area101,
                "石狩地方南部" => Regions.Area102,
                "渡島地方北部" => Regions.Area105,
                "渡島地方東部" => Regions.Area106,
                "渡島地方西部" => Regions.Area107,
                "檜山地方" => Regions.Area110,
                "後志地方北部" => Regions.Area115,
                "後志地方東部" => Regions.Area116,
                "後志地方西部" => Regions.Area117,
                "北海道奥尻島" => Regions.Area119,
                "空知地方北部" => Regions.Area120,
                "空知地方中部" => Regions.Area121,
                "空知地方南部" => Regions.Area122,
                "上川地方北部" => Regions.Area125,
                "上川地方中部" => Regions.Area126,
                "上川地方南部" => Regions.Area127,
                "留萌地方中北部" => Regions.Area130,
                "留萌地方南部" => Regions.Area131,
                "宗谷地方北部" => Regions.Area135,
                "宗谷地方南部" => Regions.Area136,
                "北海道利尻礼文" => Regions.Area139,
                "網走地方" => Regions.Area140,
                "北見地方" => Regions.Area141,
                "紋別地方" => Regions.Area142,
                "胆振地方西部" => Regions.Area145,
                "胆振地方中東部" => Regions.Area146,
                "日高地方西部" => Regions.Area150,
                "日高地方中部" => Regions.Area151,
                "日高地方東部" => Regions.Area152,
                "十勝地方北部" => Regions.Area155,
                "十勝地方中部" => Regions.Area156,
                "十勝地方南部" => Regions.Area157,
                "釧路地方北部" => Regions.Area160,
                "釧路地方中南部" => Regions.Area161,
                "根室地方北部" => Regions.Area165,
                "根室地方中部" => Regions.Area166,
                "根室地方南部" => Regions.Area167,
                "青森県津軽北部" => Regions.Area200,
                "青森県津軽南部" => Regions.Area201,
                "青森県三八上北" => Regions.Area202,
                "青森県下北" => Regions.Area203,
                "岩手県沿岸北部" => Regions.Area210,
                "岩手県沿岸南部" => Regions.Area211,
                "岩手県内陸北部" => Regions.Area212,
                "岩手県内陸南部" => Regions.Area213,
                "宮城県北部" => Regions.Area220,
                "宮城県南部" => Regions.Area221,
                "宮城県中部" => Regions.Area222,
                "秋田県沿岸北部" => Regions.Area230,
                "秋田県沿岸南部" => Regions.Area231,
                "秋田県内陸北部" => Regions.Area232,
                "秋田県内陸南部" => Regions.Area233,
                "山形県庄内" => Regions.Area240,
                "山形県最上" => Regions.Area241,
                "山形県村山" => Regions.Area242,
                "山形県置賜" => Regions.Area243,
                "福島県中通り" => Regions.Area250,
                "福島県浜通り" => Regions.Area251,
                "福島県会津" => Regions.Area252,
                "茨城県北部" => Regions.Area300,
                "茨城県南部" => Regions.Area301,
                "栃木県北部" => Regions.Area310,
                "栃木県南部" => Regions.Area311,
                "群馬県北部" => Regions.Area320,
                "群馬県南部" => Regions.Area321,
                "埼玉県北部" => Regions.Area330,
                "埼玉県南部" => Regions.Area331,
                "埼玉県秩父" => Regions.Area332,
                "千葉県北東部" => Regions.Area340,
                "千葉県北西部" => Regions.Area341,
                "千葉県南部" => Regions.Area342,
                "東京都２３区" => Regions.Area350,
                "東京都多摩東部" => Regions.Area351,
                "東京都多摩西部" => Regions.Area352,
                "神津島" => Regions.Area354,
                "伊豆大島" => Regions.Area355,
                "新島" => Regions.Area356,
                "三宅島" => Regions.Area357,
                "八丈島" => Regions.Area358,
                "小笠原" => Regions.Area359,
                "神奈川県東部" => Regions.Area360,
                "神奈川県西部" => Regions.Area361,
                "新潟県上越" => Regions.Area370,
                "新潟県中越" => Regions.Area371,
                "新潟県下越" => Regions.Area372,
                "新潟県佐渡" => Regions.Area375,
                "富山県東部" => Regions.Area380,
                "富山県西部" => Regions.Area381,
                "石川県能登" => Regions.Area390,
                "石川県加賀" => Regions.Area391,
                "福井県嶺北" => Regions.Area400,
                "福井県嶺南" => Regions.Area401,
                "山梨県中・西部" => Regions.Area411,
                "山梨県東部・富士五湖" => Regions.Area412,
                "長野県北部" => Regions.Area420,
                "長野県中部" => Regions.Area421,
                "長野県南部" => Regions.Area422,
                "岐阜県飛騨" => Regions.Area430,
                "岐阜県美濃東部" => Regions.Area431,
                "岐阜県美濃中西部" => Regions.Area432,
                "静岡県伊豆" => Regions.Area440,
                "静岡県東部" => Regions.Area441,
                "静岡県中部" => Regions.Area442,
                "静岡県西部" => Regions.Area443,
                "愛知県東部" => Regions.Area450,
                "愛知県西部" => Regions.Area451,
                "三重県北部" => Regions.Area460,
                "三重県中部" => Regions.Area461,
                "三重県南部" => Regions.Area462,
                "滋賀県北部" => Regions.Area500,
                "滋賀県南部" => Regions.Area501,
                "京都府北部" => Regions.Area510,
                "京都府南部" => Regions.Area511,
                "大阪府北部" => Regions.Area520,
                "大阪府南部" => Regions.Area521,
                "兵庫県北部" => Regions.Area530,
                "兵庫県南東部" => Regions.Area531,
                "兵庫県南西部" => Regions.Area532,
                "兵庫県淡路島" => Regions.Area535,
                "奈良県" => Regions.Area540,
                "和歌山県北部" => Regions.Area550,
                "和歌山県南部" => Regions.Area551,
                "鳥取県東部" => Regions.Area560,
                "鳥取県中部" => Regions.Area562,
                "鳥取県西部" => Regions.Area563,
                "島根県東部" => Regions.Area570,
                "島根県西部" => Regions.Area571,
                "島根県隠岐" => Regions.Area575,
                "岡山県北部" => Regions.Area580,
                "岡山県南部" => Regions.Area581,
                "広島県北部" => Regions.Area590,
                "広島県南東部" => Regions.Area591,
                "広島県南西部" => Regions.Area592,
                "徳島県北部" => Regions.Area600,
                "徳島県南部" => Regions.Area601,
                "香川県東部" => Regions.Area610,
                "香川県西部" => Regions.Area611,
                "愛媛県東予" => Regions.Area620,
                "愛媛県中予" => Regions.Area621,
                "愛媛県南予" => Regions.Area622,
                "高知県東部" => Regions.Area630,
                "高知県中部" => Regions.Area631,
                "高知県西部" => Regions.Area632,
                "山口県北部" => Regions.Area700,
                "山口県西部" => Regions.Area702,
                "山口県東部" => Regions.Area703,
                "山口県中部" => Regions.Area704,
                "福岡県福岡" => Regions.Area710,
                "福岡県北九州" => Regions.Area711,
                "福岡県筑豊" => Regions.Area712,
                "福岡県筑後" => Regions.Area713,
                "佐賀県北部" => Regions.Area720,
                "佐賀県南部" => Regions.Area721,
                "長崎県北部" => Regions.Area730,
                "長崎県南西部" => Regions.Area731,
                "長崎県島原半島" => Regions.Area732,
                "長崎県対馬" => Regions.Area735,
                "長崎県壱岐" => Regions.Area736,
                "長崎県五島" => Regions.Area737,
                "熊本県阿蘇" => Regions.Area740,
                "熊本県熊本" => Regions.Area741,
                "熊本県球磨" => Regions.Area742,
                "熊本県天草・芦北" => Regions.Area743,
                "大分県北部" => Regions.Area750,
                "大分県中部" => Regions.Area751,
                "大分県南部" => Regions.Area752,
                "大分県西部" => Regions.Area753,
                "宮崎県北部平野部" => Regions.Area760,
                "宮崎県北部山沿い" => Regions.Area761,
                "宮崎県南部平野部" => Regions.Area762,
                "宮崎県南部山沿い" => Regions.Area763,
                "鹿児島県薩摩" => Regions.Area770,
                "鹿児島県大隅" => Regions.Area771,
                "鹿児島県十島村" => Regions.Area774,
                "鹿児島県甑島" => Regions.Area775,
                "鹿児島県種子島" => Regions.Area776,
                "鹿児島県屋久島" => Regions.Area777,
                "鹿児島県奄美北部" => Regions.Area778,
                "鹿児島県奄美南部" => Regions.Area779,
                "沖縄県本島北部" => Regions.Area800,
                "沖縄県本島中南部" => Regions.Area801,
                "沖縄県久米島" => Regions.Area802,
                "沖縄県大東島" => Regions.Area803,
                "沖縄県宮古島" => Regions.Area804,
                "沖縄県石垣島" => Regions.Area805,
                "沖縄県与那国島" => Regions.Area806,
                "沖縄県西表島" => Regions.Area807,
                _ => Regions.Unknown,
            };
        }
        /// <summary>
        /// ローカルからenumに変換
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static LocalAreas StrToLocal(string area)
        {
            return area switch
            {
                "北海道道央" => LocalAreas.Area9011,
                "北海道道南" => LocalAreas.Area9012,
                "北海道道北" => LocalAreas.Area9013,
                "北海道道東" => LocalAreas.Area9014,
                "青森" => LocalAreas.Area9020,
                "岩手" => LocalAreas.Area9030,
                "宮城" => LocalAreas.Area9040,
                "秋田" => LocalAreas.Area9050,
                "山形" => LocalAreas.Area9060,
                "福島" => LocalAreas.Area9070,
                "茨城" => LocalAreas.Area9080,
                "栃木" => LocalAreas.Area9090,
                "群馬" => LocalAreas.Area9100,
                "埼玉" => LocalAreas.Area9110,
                "千葉" => LocalAreas.Area9120,
                "東京" => LocalAreas.Area9131,
                "伊豆諸島" => LocalAreas.Area9132,
                "小笠原" => LocalAreas.Area9133,
                "神奈川" => LocalAreas.Area9140,
                "新潟" => LocalAreas.Area9150,
                "富山" => LocalAreas.Area9160,
                "石川" => LocalAreas.Area9170,
                "福井" => LocalAreas.Area9180,
                "山梨" => LocalAreas.Area9190,
                "長野" => LocalAreas.Area9200,
                "岐阜" => LocalAreas.Area9210,
                "静岡" => LocalAreas.Area9220,
                "愛知" => LocalAreas.Area9230,
                "三重" => LocalAreas.Area9240,
                "滋賀" => LocalAreas.Area9250,
                "京都" => LocalAreas.Area9260,
                "大阪" => LocalAreas.Area9270,
                "兵庫" => LocalAreas.Area9280,
                "奈良" => LocalAreas.Area9290,
                "和歌山" => LocalAreas.Area9300,
                "鳥取" => LocalAreas.Area9310,
                "島根" => LocalAreas.Area9320,
                "岡山" => LocalAreas.Area9330,
                "広島" => LocalAreas.Area9340,
                "徳島" => LocalAreas.Area9360,
                "香川" => LocalAreas.Area9370,
                "愛媛" => LocalAreas.Area9380,
                "高知" => LocalAreas.Area9390,
                "山口" => LocalAreas.Area9350,
                "福岡" => LocalAreas.Area9400,
                "佐賀" => LocalAreas.Area9410,
                "長崎" => LocalAreas.Area9420,
                "熊本" => LocalAreas.Area9430,
                "大分" => LocalAreas.Area9440,
                "宮崎" => LocalAreas.Area9450,
                "鹿児島" => LocalAreas.Area9461,
                "奄美" => LocalAreas.Area9462,
                "沖縄本島" => LocalAreas.Area9471,
                "大東島" => LocalAreas.Area9472,
                "宮古島" => LocalAreas.Area9473,
                "八重山" => LocalAreas.Area9474,
                _ => LocalAreas.Unknown,
            };
        }
        /// <summary>
        /// 地方名からenumに変換
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static District StrToDistrict(string area)
        {
            return area switch
            {
                "北海道" => District.Area9910,
                "東北" => District.Area9920,
                "関東" => District.Area9931,
                "伊豆諸島" => District.Area9932,
                "小笠原" => District.Area9933,
                "北陸" => District.Area9934,
                "甲信" => District.Area9935,
                "東海" => District.Area9936,
                "近畿" => District.Area9941,
                "中国" => District.Area9942,
                "四国" => District.Area9943,
                "九州" => District.Area9951,
                "奄美" or "奄美群島" or "奄美(群島)" => District.Area9952,
                "沖縄" => District.Area9960,
                _ => District.Unknown,
            };
        }
        #endregion
        #region Enum to String
        public static string RegionsToStr(Regions area)
        {
            return area switch
            {
                Regions.Area100 => "石狩地方北部",
                Regions.Area101 => "石狩地方中部",
                Regions.Area102 => "石狩地方南部",
                Regions.Area105 => "渡島地方北部",
                Regions.Area106 => "渡島地方東部",
                Regions.Area107 => "渡島地方西部",
                Regions.Area110 => "檜山地方",
                Regions.Area115 => "後志地方北部",
                Regions.Area116 => "後志地方東部",
                Regions.Area117 => "後志地方西部",
                Regions.Area119 => "北海道奥尻島",
                Regions.Area120 => "空知地方北部",
                Regions.Area121 => "空知地方中部",
                Regions.Area122 => "空知地方南部",
                Regions.Area125 => "上川地方北部",
                Regions.Area126 => "上川地方中部",
                Regions.Area127 => "上川地方南部",
                Regions.Area130 => "留萌地方中北部",
                Regions.Area131 => "留萌地方南部",
                Regions.Area135 => "宗谷地方北部",
                Regions.Area136 => "宗谷地方南部",
                Regions.Area139 => "北海道利尻礼文",
                Regions.Area140 => "網走地方",
                Regions.Area141 => "北見地方",
                Regions.Area142 => "紋別地方",
                Regions.Area145 => "胆振地方西部",
                Regions.Area146 => "胆振地方中東部",
                Regions.Area150 => "日高地方西部",
                Regions.Area151 => "日高地方中部",
                Regions.Area152 => "日高地方東部",
                Regions.Area155 => "十勝地方北部",
                Regions.Area156 => "十勝地方中部",
                Regions.Area157 => "十勝地方南部",
                Regions.Area160 => "釧路地方北部",
                Regions.Area161 => "釧路地方中南部",
                Regions.Area165 => "根室地方北部",
                Regions.Area166 => "根室地方中部",
                Regions.Area167 => "根室地方南部",
                Regions.Area200 => "青森県津軽北部",
                Regions.Area201 => "青森県津軽南部",
                Regions.Area202 => "青森県三八上北",
                Regions.Area203 => "青森県下北",
                Regions.Area210 => "岩手県沿岸北部",
                Regions.Area211 => "岩手県沿岸南部",
                Regions.Area212 => "岩手県内陸北部",
                Regions.Area213 => "岩手県内陸南部",
                Regions.Area220 => "宮城県北部",
                Regions.Area221 => "宮城県南部",
                Regions.Area222 => "宮城県中部",
                Regions.Area230 => "秋田県沿岸北部",
                Regions.Area231 => "秋田県沿岸南部",
                Regions.Area232 => "秋田県内陸北部",
                Regions.Area233 => "秋田県内陸南部",
                Regions.Area240 => "山形県庄内",
                Regions.Area241 => "山形県最上",
                Regions.Area242 => "山形県村山",
                Regions.Area243 => "山形県置賜",
                Regions.Area250 => "福島県中通り",
                Regions.Area251 => "福島県浜通り",
                Regions.Area252 => "福島県会津",
                Regions.Area300 => "茨城県北部",
                Regions.Area301 => "茨城県南部",
                Regions.Area310 => "栃木県北部",
                Regions.Area311 => "栃木県南部",
                Regions.Area320 => "群馬県北部",
                Regions.Area321 => "群馬県南部",
                Regions.Area330 => "埼玉県北部",
                Regions.Area331 => "埼玉県南部",
                Regions.Area332 => "埼玉県秩父",
                Regions.Area340 => "千葉県北東部",
                Regions.Area341 => "千葉県北西部",
                Regions.Area342 => "千葉県南部",
                Regions.Area350 => "東京都２３区",
                Regions.Area351 => "東京都多摩東部",
                Regions.Area352 => "東京都多摩西部",
                Regions.Area354 => "神津島",
                Regions.Area355 => "伊豆大島",
                Regions.Area356 => "新島",
                Regions.Area357 => "三宅島",
                Regions.Area358 => "八丈島",
                Regions.Area359 => "小笠原",
                Regions.Area360 => "神奈川県東部",
                Regions.Area361 => "神奈川県西部",
                Regions.Area370 => "新潟県上越",
                Regions.Area371 => "新潟県中越",
                Regions.Area372 => "新潟県下越",
                Regions.Area375 => "新潟県佐渡",
                Regions.Area380 => "富山県東部",
                Regions.Area381 => "富山県西部",
                Regions.Area390 => "石川県能登",
                Regions.Area391 => "石川県加賀",
                Regions.Area400 => "福井県嶺北",
                Regions.Area401 => "福井県嶺南",
                Regions.Area411 => "山梨県中・西部",
                Regions.Area412 => "山梨県東部・富士五湖",
                Regions.Area420 => "長野県北部",
                Regions.Area421 => "長野県中部",
                Regions.Area422 => "長野県南部",
                Regions.Area430 => "岐阜県飛騨",
                Regions.Area431 => "岐阜県美濃東部",
                Regions.Area432 => "岐阜県美濃中西部",
                Regions.Area440 => "静岡県伊豆",
                Regions.Area441 => "静岡県東部",
                Regions.Area442 => "静岡県中部",
                Regions.Area443 => "静岡県西部",
                Regions.Area450 => "愛知県東部",
                Regions.Area451 => "愛知県西部",
                Regions.Area460 => "三重県北部",
                Regions.Area461 => "三重県中部",
                Regions.Area462 => "三重県南部",
                Regions.Area500 => "滋賀県北部",
                Regions.Area501 => "滋賀県南部",
                Regions.Area510 => "京都府北部",
                Regions.Area511 => "京都府南部",
                Regions.Area520 => "大阪府北部",
                Regions.Area521 => "大阪府南部",
                Regions.Area530 => "兵庫県北部",
                Regions.Area531 => "兵庫県南東部",
                Regions.Area532 => "兵庫県南西部",
                Regions.Area535 => "兵庫県淡路島",
                Regions.Area540 => "奈良県",
                Regions.Area550 => "和歌山県北部",
                Regions.Area551 => "和歌山県南部",
                Regions.Area560 => "鳥取県東部",
                Regions.Area562 => "鳥取県中部",
                Regions.Area563 => "鳥取県西部",
                Regions.Area570 => "島根県東部",
                Regions.Area571 => "島根県西部",
                Regions.Area575 => "島根県隠岐",
                Regions.Area580 => "岡山県北部",
                Regions.Area581 => "岡山県南部",
                Regions.Area590 => "広島県北部",
                Regions.Area591 => "広島県南東部",
                Regions.Area592 => "広島県南西部",
                Regions.Area600 => "徳島県北部",
                Regions.Area601 => "徳島県南部",
                Regions.Area610 => "香川県東部",
                Regions.Area611 => "香川県西部",
                Regions.Area620 => "愛媛県東予",
                Regions.Area621 => "愛媛県中予",
                Regions.Area622 => "愛媛県南予",
                Regions.Area630 => "高知県東部",
                Regions.Area631 => "高知県中部",
                Regions.Area632 => "高知県西部",
                Regions.Area700 => "山口県北部",
                Regions.Area702 => "山口県西部",
                Regions.Area703 => "山口県東部",
                Regions.Area704 => "山口県中部",
                Regions.Area710 => "福岡県福岡",
                Regions.Area711 => "福岡県北九州",
                Regions.Area712 => "福岡県筑豊",
                Regions.Area713 => "福岡県筑後",
                Regions.Area720 => "佐賀県北部",
                Regions.Area721 => "佐賀県南部",
                Regions.Area730 => "長崎県北部",
                Regions.Area731 => "長崎県南西部",
                Regions.Area732 => "長崎県島原半島",
                Regions.Area735 => "長崎県対馬",
                Regions.Area736 => "長崎県壱岐",
                Regions.Area737 => "長崎県五島",
                Regions.Area740 => "熊本県阿蘇",
                Regions.Area741 => "熊本県熊本",
                Regions.Area742 => "熊本県球磨",
                Regions.Area743 => "熊本県天草・芦北",
                Regions.Area750 => "大分県北部",
                Regions.Area751 => "大分県中部",
                Regions.Area752 => "大分県南部",
                Regions.Area753 => "大分県西部",
                Regions.Area760 => "宮崎県北部平野部",
                Regions.Area761 => "宮崎県北部山沿い",
                Regions.Area762 => "宮崎県南部平野部",
                Regions.Area763 => "宮崎県南部山沿い",
                Regions.Area770 => "鹿児島県薩摩",
                Regions.Area771 => "鹿児島県大隅",
                Regions.Area774 => "鹿児島県十島村",
                Regions.Area775 => "鹿児島県甑島",
                Regions.Area776 => "鹿児島県種子島",
                Regions.Area777 => "鹿児島県屋久島",
                Regions.Area778 => "鹿児島県奄美北部",
                Regions.Area779 => "鹿児島県奄美南部",
                Regions.Area800 => "沖縄県本島北部",
                Regions.Area801 => "沖縄県本島中南部",
                Regions.Area802 => "沖縄県久米島",
                Regions.Area803 => "沖縄県大東島",
                Regions.Area804 => "沖縄県宮古島",
                Regions.Area805 => "沖縄県石垣島",
                Regions.Area806 => "沖縄県与那国島",
                Regions.Area807 => "沖縄県西表島",
                _ => "不明",
            };
        }
        public static string LocalAreasToStr(LocalAreas area)
        {
            return area switch
            {
                LocalAreas.Area9011 => "北海道道央",
                LocalAreas.Area9012 => "北海道道南",
                LocalAreas.Area9013 => "北海道道北",
                LocalAreas.Area9014 => "北海道道東",
                LocalAreas.Area9020 => "青森",
                LocalAreas.Area9030 => "岩手",
                LocalAreas.Area9040 => "宮城",
                LocalAreas.Area9050 => "秋田",
                LocalAreas.Area9060 => "山形",
                LocalAreas.Area9070 => "福島",
                LocalAreas.Area9080 => "茨城",
                LocalAreas.Area9090 => "栃木",
                LocalAreas.Area9100 => "群馬",
                LocalAreas.Area9110 => "埼玉",
                LocalAreas.Area9120 => "千葉",
                LocalAreas.Area9131 => "東京",
                LocalAreas.Area9132 => "伊豆諸島",
                LocalAreas.Area9133 => "小笠原",
                LocalAreas.Area9140 => "神奈川",
                LocalAreas.Area9150 => "新潟",
                LocalAreas.Area9160 => "富山",
                LocalAreas.Area9170 => "石川",
                LocalAreas.Area9180 => "福井",
                LocalAreas.Area9190 => "山梨",
                LocalAreas.Area9200 => "長野",
                LocalAreas.Area9210 => "岐阜",
                LocalAreas.Area9220 => "静岡",
                LocalAreas.Area9230 => "愛知",
                LocalAreas.Area9240 => "三重",
                LocalAreas.Area9250 => "滋賀",
                LocalAreas.Area9260 => "京都",
                LocalAreas.Area9270 => "大阪",
                LocalAreas.Area9280 => "兵庫",
                LocalAreas.Area9290 => "奈良",
                LocalAreas.Area9300 => "和歌山",
                LocalAreas.Area9310 => "鳥取",
                LocalAreas.Area9320 => "島根",
                LocalAreas.Area9330 => "岡山",
                LocalAreas.Area9340 => "広島",
                LocalAreas.Area9360 => "徳島",
                LocalAreas.Area9370 => "香川",
                LocalAreas.Area9380 => "愛媛",
                LocalAreas.Area9390 => "高知",
                LocalAreas.Area9350 => "山口",
                LocalAreas.Area9400 => "福岡",
                LocalAreas.Area9410 => "佐賀",
                LocalAreas.Area9420 => "長崎",
                LocalAreas.Area9430 => "熊本",
                LocalAreas.Area9440 => "大分",
                LocalAreas.Area9450 => "宮崎",
                LocalAreas.Area9461 => "鹿児島",
                LocalAreas.Area9462 => "奄美",
                LocalAreas.Area9471 => "沖縄本島",
                LocalAreas.Area9472 => "大東島",
                LocalAreas.Area9473 => "宮古島",
                LocalAreas.Area9474 => "八重山",
                _ => "不明",
            };
        }
        public static string DistrictToStr(District area)
        {
            return area switch
            {
                District.Area9910 => "北海道",
                District.Area9920 => "東北",
                District.Area9931 => "関東",
                District.Area9932 => "伊豆諸島",
                District.Area9933 => "小笠原",
                District.Area9934 => "北陸",
                District.Area9935 => "甲信",
                District.Area9936 => "東海",
                District.Area9941 => "近畿",
                District.Area9942 => "中国",
                District.Area9943 => "四国",
                District.Area9951 => "九州",
                District.Area9952 => "奄美",
                District.Area9960 => "沖縄",
                _ => "不明",
            };
        }
        #endregion

    }
}
