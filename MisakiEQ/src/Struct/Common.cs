using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Struct
{
    public class Common
    {
        /// <summary>
        /// 座標
        /// </summary>
        public class Point
        {
            public Point()
            {
                X = 0;
                Y = 0;
            }
            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
            public Point(System.Drawing.Point point)
            {
                X = point.X;
                Y = point.Y;
            }

            public double X { get; set; }
            public double Y { get; set; }

        }
        /// <summary>
        /// 緯経度
        /// </summary>
        public class LAL
        {
            public LAL()
            {
                Lo = 0;
                La = 0;
            }
            public LAL(double lon, double lat)
            {
                Lon = lon;
                Lat = lat;
            }

            public double Lon{ 
                get 
                { 
                    return Lo; 
                } 
                set 
                {
                    if (double.IsNaN(value))
                    {
                        Lo = double.NaN;
                        La = double.NaN;
                        return;
                    }
                    if (value > 180 || value < -180) throw new ArgumentOutOfRangeException("Lon","経度は-180より下、もしくは180より上を超えてはいけません。");
                    Lo = value;
                } 
            }
            public double Lat
            {
                get
                {
                    return La;
                }
                set
                {
                    if (double.IsNaN(value))
                    {
                        Lo = double.NaN;
                        La = double.NaN;
                        return;
                    }
                    if (value > 90 || value < -90) throw new ArgumentOutOfRangeException("Lat", "緯度は-90より下、もしくは90より上を超えてはいけません。");
                    La = value;
                }
            }
            double Lo;
            double La;
            public double GetDistanceTo(LAL other)
            {
                if (double.IsNaN(Lat) || double.IsNaN(Lon) || double.IsNaN(other.Lat) || double.IsNaN(other.Lon))
                {
                    return double.NaN;
                }
                double num2 = Lat * (Math.PI / 180.0);
                double num3 = Lon * (Math.PI / 180.0);
                double num4 = other.Lat * (Math.PI / 180.0);
                double num5 = other.Lon * (Math.PI / 180.0);
                double num6 = num5 - num3;
                double num7 = num4 - num2;
                double num8 = Math.Pow(Math.Sin(num7 / 2.0), 2.0) + Math.Cos(num2) * Math.Cos(num4) * Math.Pow(Math.Sin(num6 / 2.0), 2.0);
                double num9 = 2.0 * Math.Atan2(Math.Sqrt(num8), Math.Sqrt(1.0 - num8));
                return 6376500.0 * num9;
            }
        }
        

        /// <summary>
        /// stringからDateTimeに変換します。
        /// </summary>
        /// <param name="str">変換させる文字列</param>
        /// <returns><para>変換後のDateTime型</para><para>失敗時はDateTime.MinValueが返ります。</para></returns>
        public static DateTime GetDateTime(string str)
        {
            if(DateTime.TryParse(str, out DateTime dt))
            {
                return dt;
            }
            else
            {
                Log.Warn($"DateTime型への変換に失敗しました。 値 = \"{str}\"");
                return DateTime.MinValue;
            }
        }
        ///<summary>10段階で示される震度です。</summary>
        public enum Intensity
        {
            ///<summary>不明</summary>
            Unknown = -1,
            ///<summary>震度０</summary>
            Int0 = 0,
            ///<summary>震度１</summary>
            Int1 = 1,
            ///<summary>震度２</summary>
            Int2 = 2,
            ///<summary>震度３</summary>
            Int3 = 3,
            ///<summary>震度４</summary>
            Int4 = 4,
            ///<summary>震度５弱</summary>
            Int5Down = 5,
            ///<summary>震度５強</summary>
            Int5Up = 6,
            ///<summary>震度６弱</summary>
            Int6Down = 7,
            ///<summary>震度６強</summary>
            Int6Up = 8,
            ///<summary>震度７</summary>
            Int7 = 9,
            ///<summary>震度５弱以上推定</summary>
            Int5Over = -2,
        }
        /// <summary>
        /// 文字列から都道府県Enumに変換します。
        /// </summary>
        /// <param name="str">都道府県名</param>
        /// <returns>列挙型都道府県</returns>
        public static Prefectures StringToPrefectures(string str)
        {
            string comp = str.Replace("県", "").Replace("府", "");
            return comp switch
            {
                "北海道" => Prefectures.Hokkaido,
                "青森" => Prefectures.Aomori,
                "岩手" => Prefectures.Iwate,
                "宮城" => Prefectures.Miyagi,
                "秋田" => Prefectures.Akita,
                "山形" => Prefectures.Yamagata,
                "福島" => Prefectures.Fukushima,
                "茨城" => Prefectures.Ibaraki,
                "栃木" => Prefectures.Tochigi,
                "群馬" => Prefectures.Gunma,
                "埼玉" => Prefectures.Saitama,
                "千葉" => Prefectures.Chiba,
                "東京都" => Prefectures.Tokyo,
                "東京" => Prefectures.Tokyo,
                "神奈川" => Prefectures.Kanagawa,
                "新潟" => Prefectures.Niigata,
                "富山" => Prefectures.Toyama,
                "石川" => Prefectures.Ishikawa,
                "福井" => Prefectures.Fukui,
                "山梨" => Prefectures.Yamanashi,
                "長野" => Prefectures.Nagano,
                "岐阜" => Prefectures.Gifu,
                "静岡" => Prefectures.Shizuoka,
                "愛知" => Prefectures.Aichi,
                "三重" => Prefectures.Mie,
                "滋賀" => Prefectures.Shiga,
                "京都" => Prefectures.Kyoto,
                "大阪" => Prefectures.Osaka,
                "兵庫" => Prefectures.Hyogo,
                "奈良" => Prefectures.Nara,
                "和歌山" => Prefectures.Wakayama,
                "鳥取" => Prefectures.Tottori,
                "島根" => Prefectures.Shimane,
                "岡山" => Prefectures.Okayama,
                "広島" => Prefectures.Hiroshima,
                "山口" => Prefectures.Yamaguchi,
                "徳島" => Prefectures.Tokushima,
                "香川" => Prefectures.Kagawa,
                "愛媛" => Prefectures.Ehime,
                "高知" => Prefectures.Kouchi,
                "福岡" => Prefectures.Fukuoka,
                "佐賀" => Prefectures.Saga,
                "長崎" => Prefectures.Nagasaki,
                "熊本" => Prefectures.Kumamoto,
                "大分" => Prefectures.Oita,
                "宮崎" => Prefectures.Miyazaki,
                "鹿児島" => Prefectures.Kagoshima,
                "沖縄" => Prefectures.Okinawa,
                _ => Prefectures.Unknown
            };
        }
        /// <summary>
        /// 列挙型都道府県を文字列に変換します
        /// </summary>
        /// <param name="pref">変換する列挙型都道府県名</param>
        /// <returns>日本語の文字列が返されます。</returns>
        public static string PrefecturesToString(Prefectures pref)
        {
            return pref switch
            {
                Prefectures.Hokkaido=>"北海道",
                Prefectures.Aomori=>"青森",
                Prefectures.Iwate=>"岩手",
                Prefectures.Miyagi=>"宮城",
                Prefectures.Akita=>"秋田",
                Prefectures.Yamagata=>"山形",
                Prefectures.Fukushima=>"福島",
                Prefectures.Ibaraki=>"茨城",
                Prefectures.Tochigi=>"栃木",
                Prefectures.Gunma=>"群馬",
                Prefectures.Saitama=>"埼玉",
                Prefectures.Chiba=>"千葉",
                Prefectures.Tokyo=>"東京",
                Prefectures.Kanagawa=>"神奈川",
                Prefectures.Niigata=>"新潟",
                Prefectures.Toyama=>"富山",
                Prefectures.Ishikawa=>"石川",
                Prefectures.Fukui=>"福井",
                Prefectures.Yamanashi=>"山梨",
                Prefectures.Nagano=>"長野",
                Prefectures.Gifu=>"岐阜",
                Prefectures.Shizuoka=>"静岡",
                Prefectures.Aichi=>"愛知",
                Prefectures.Mie=>"三重",
                Prefectures.Shiga=>"滋賀",
                Prefectures.Kyoto=>"京都",
                Prefectures.Osaka=>"大阪",
                Prefectures.Hyogo=>"兵庫",
                Prefectures.Nara=>"奈良",
                Prefectures.Wakayama=>"和歌山",
                Prefectures.Tottori=>"鳥取",
                Prefectures.Shimane=>"島根",
                Prefectures.Okayama=>"岡山",
                Prefectures.Hiroshima=>"広島",
                Prefectures.Yamaguchi=>"山口",
                Prefectures.Tokushima=>"徳島",
                Prefectures.Kagawa=>"香川",
                Prefectures.Ehime=>"愛媛",
                Prefectures.Kouchi=>"高知",
                Prefectures.Fukuoka=>"福岡",
                Prefectures.Saga=>"佐賀",
                Prefectures.Nagasaki=>"長崎",
                Prefectures.Kumamoto=>"熊本",
                Prefectures.Oita=>"大分" ,
                Prefectures.Miyazaki=>"宮崎" ,
                Prefectures.Kagoshima=>"鹿児島",
                Prefectures.Okinawa=>"沖縄" ,
                Prefectures.Unknown=>"不明",
                _=>"不明"
            };
        }
        /// <summary>
        /// <para>ASCIIでも示すことができる震度表示に変換します</para>
        /// <para>例 : <br/>震度3→「3」<br/>震度5強→「5+」<br/>震度不明→「-」</para>
        /// </summary>
        /// <param name="value">列挙型震度情報</param>
        /// <returns>ASCIIのみの2文字以下の文字列が返されます。</returns>
        public static string IntToStringShort(Intensity value)
        {
            return value switch
            {
                Intensity.Unknown => "-",
                Intensity.Int0 => "0",
                Intensity.Int1 => "1",
                Intensity.Int2 => "2",
                Intensity.Int3 => "3",
                Intensity.Int4 => "4",
                Intensity.Int5Down => "5-",
                Intensity.Int5Over => "5-",
                Intensity.Int5Up => "5+",
                Intensity.Int6Down => "6-",
                Intensity.Int6Up => "6+",
                Intensity.Int7 => "7",
                _ => value.ToString(),
            };
        }
        /// <summary>
        /// <para>日本語の全角震度表示に変換します</para>
        /// <para>例 : <br/>震度3→「３」<br/>震度5強→「５強」<br/>震度不明→「不明」</para>
        /// </summary>
        /// <param name="value">列挙型震度情報</param>
        /// <returns>日本語の文字列が返されます。</returns>
        public static string IntToStringLong(Intensity value)
        {
            return value switch
            {
                Intensity.Unknown => "不明",
                Intensity.Int0 => "０",
                Intensity.Int1 => "１",
                Intensity.Int2 => "２",
                Intensity.Int3 => "３",
                Intensity.Int4 => "４",
                Intensity.Int5Down => "５弱",
                Intensity.Int5Over => "５弱以上",
                Intensity.Int5Up => "５強",
                Intensity.Int6Down => "６弱",
                Intensity.Int6Up => "６強",
                Intensity.Int7 => "７",
                _ => value.ToString(),
            };
        }

        /// <summary>
        /// <para>文字列から列挙型震度情報に変換します</para>
        /// <para>どのフォーマットでもOK<br/>例:<br/>
        /// 5+→OK<br/>6弱→OK<br/>５＋→OK<br/>６強→OK</para>
        /// </summary>
        /// <param name="value">任意の変換したい文字列</param>
        /// <returns>列挙型の震度情報が返されます。</returns>
        public static Intensity StringToInt(string? value)
        {
            if (value == null) return Intensity.Unknown;
            string from = "０１２３４５６７８９強弱＋－";
            string to = "0123456789+-+-";
            for(int i = 0; i < from.Length; i++)
                value = value.Replace(from[i], to[i]);
            return value switch
            {
                "不明" => Intensity.Unknown,
                "0" => Intensity.Int0,
                "1" => Intensity.Int1,
                "2" => Intensity.Int2,
                "3" => Intensity.Int3,
                "4" => Intensity.Int4,
                "5-" => Intensity.Int5Down,
                "5+" => Intensity.Int5Up,
                "6-" => Intensity.Int6Down,
                "6+" => Intensity.Int6Up,
                "7" => Intensity.Int7,
                _ => Intensity.Unknown,
            };
        }
        public static Intensity FloatToInt(double value)
        {
            if (value <= 0||double.IsNaN(value)) return Intensity.Unknown;
            if (value < 0.5) return Intensity.Int0;
            if (value < 1.5) return Intensity.Int1;
            if (value < 2.5) return Intensity.Int2;
            if (value < 3.5) return Intensity.Int3;
            if (value < 4.5) return Intensity.Int4;
            if (value < 5.0) return Intensity.Int5Down;
            if (value < 5.5) return Intensity.Int5Up;
            if (value < 6.0) return Intensity.Int6Down;
            if (value < 6.5) return Intensity.Int6Up;
            return Intensity.Int7;
        }
        /// <summary>
        /// 震源の深さを文字列に変換します。<br/>
        /// ごく浅いと不明の場合の処理を入れています。
        /// </summary>
        /// <param name="depth">Int型の震源の深さ</param>
        /// <returns>震源の深さの文字列</returns>
        public static string DepthToString(int depth)
        {
            if (depth < 0) return "不明";
            if (depth == 0) return "ごく浅い";
            return $"{depth} km";
        }

        ///<summary>緯度経度で示される所在地</summary>
        public class Location
        {
            ///<summary>緯度</summary>
            public double Lat = double.NaN;
            ///<summary>経度</summary>
            public double Long = double.NaN;
        }
        ///<summary>都道府県</summary>
        public enum Prefectures
        {
            ///<summary>不明</summary>
            Unknown, 
            ///<summary>北海道 - 北海道地方</summary>
            Hokkaido,
            ///<summary>青森県 - 東北地方</summary>
            Aomori,
            ///<summary>岩手県 - 東北地方</summary>
            Iwate,
            ///<summary>宮城県 - 東北地方</summary>
            Miyagi,
            ///<summary>秋田県 - 東北地方</summary>
            Akita,
            ///<summary>山形県 - 東北地方</summary>
            Yamagata,
            ///<summary>福島県 - 東北地方</summary>
            Fukushima,
            ///<summary>茨城県 - 関東地方</summary>
            Ibaraki,
            ///<summary>栃木県 - 関東地方</summary>
            Tochigi,
            ///<summary>群馬県 - 関東地方</summary>
            Gunma,
            ///<summary>埼玉県 - 関東地方</summary>
            Saitama,
            ///<summary>千葉県 - 関東地方</summary>
            Chiba,
            ///<summary>東京都 - 関東地方</summary>
            Tokyo,
            ///<summary>神奈川県 - 関東地方</summary>
            Kanagawa,
            ///<summary>新潟県 - 中部地方</summary>
            Niigata,
            ///<summary>富山県 - 中部地方</summary>
            Toyama,
            ///<summary>石川県 - 中部地方</summary>
            Ishikawa,
            ///<summary>福井県 - 中部地方</summary>
            Fukui,
            ///<summary>山梨県 - 中部地方</summary>
            Yamanashi,
            ///<summary>長野県 - 中部地方</summary>
            Nagano,
            ///<summary>岐阜県 - 中部地方</summary>
            Gifu,
            ///<summary>静岡県 - 中部地方</summary>
            Shizuoka,
            ///<summary>愛知県 - 中部地方</summary>
            Aichi,
            ///<summary>三重県 - 近畿地方</summary>
            Mie,
            ///<summary>滋賀県 - 近畿地方</summary>
            Shiga,
            ///<summary>京都府 - 近畿地方</summary>
            Kyoto,
            ///<summary>大阪府 - 近畿地方</summary>
            Osaka,
            ///<summary>兵庫県 - 近畿地方</summary>
            Hyogo,
            ///<summary>奈良県 - 近畿地方</summary>
            Nara,
            ///<summary>和歌山県 - 近畿地方</summary>
            Wakayama,
            ///<summary>鳥取県 - 中国地方</summary>
            Tottori,
            ///<summary>島根県 - 中国地方</summary>
            Shimane,
            ///<summary>岡山県 - 中国地方</summary>
            Okayama,
            ///<summary>広島県 - 中国地方</summary>
            Hiroshima,
            ///<summary>山口県 - 中国地方</summary>
            Yamaguchi,
            ///<summary>徳島県 - 四国地方</summary>
            Tokushima,
            ///<summary>香川県 - 四国地方</summary>
            Kagawa,
            ///<summary>愛媛県 - 四国地方</summary>
            Ehime,
            ///<summary>高知県 - 四国地方</summary>
            Kouchi,
            ///<summary>福岡県 - 九州・沖縄地方</summary>
            Fukuoka,
            ///<summary>佐賀県 - 九州・沖縄地方</summary>
            Saga,
            ///<summary>長崎県 - 九州・沖縄地方</summary>
            Nagasaki,
            ///<summary>熊本県 - 九州・沖縄地方</summary>
            Kumamoto,
            ///<summary>大分県 - 九州・沖縄地方</summary>
            Oita,
            ///<summary>宮崎県 - 九州・沖縄地方</summary>
            Miyazaki,
            ///<summary>鹿児島県 - 九州・沖縄地方</summary>
            Kagoshima,
            ///<summary>沖縄県 - 九州・沖縄地方</summary>
            Okinawa
        }
    }
}
