﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MisakiEQ.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MisakiEQ.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   型 System.Drawing.Bitmap のローカライズされたリソースを検索します。
        /// </summary>
        public static System.Drawing.Bitmap K_moni_BaseMap {
            get {
                object obj = ResourceManager.GetObject("K-moni.BaseMap", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   型 System.Drawing.Bitmap のローカライズされたリソースを検索します。
        /// </summary>
        public static System.Drawing.Bitmap Logo_AppIcon {
            get {
                object obj = ResourceManager.GetObject("Logo.AppIcon", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   (アイコン) に類似した型 System.Drawing.Icon のローカライズされたリソースを検索します。
        /// </summary>
        public static System.Drawing.Icon Logo_MainIcon {
            get {
                object obj = ResourceManager.GetObject("Logo.MainIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   型 System.Byte[] のローカライズされたリソースを検索します。
        /// </summary>
        public static byte[] ShindoObsPoints_mpk {
            get {
                object obj = ResourceManager.GetObject("ShindoObsPoints_mpk", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   {
        ///  &quot;ParseStatus&quot;: &quot;Success&quot;,
        ///  &quot;Title&quot;: {
        ///    &quot;Code&quot;: 37,
        ///    &quot;String&quot;: &quot;緊急地震速報（警報）&quot;,
        ///    &quot;Detail&quot;: &quot;マグニチュード、最大予測震度及び主要動到達予測時刻の緊急地震速報（発表パターン3： グリッド サーチ法、EPOS自動処理手法）&quot;
        ///  },
        ///  &quot;Source&quot;: {
        ///    &quot;Code&quot;: 3,
        ///    &quot;String&quot;: &quot;気象庁本庁&quot;
        ///  },
        ///  &quot;Status&quot;: {
        ///    &quot;Code&quot;: &quot;00&quot;,
        ///    &quot;String&quot;: &quot;通常&quot;,
        ///    &quot;Detail&quot;: &quot;通常&quot;
        ///  },
        ///  &quot;AnnouncedTime&quot;: {
        ///    &quot;String&quot;: &quot;2018/06/18 07:58:49&quot;,
        ///    &quot;UnixTime&quot;: 1529276329,
        ///    &quot;RFC1123&quot;: &quot;Sun, 17 Jun 2018 22:58:49 UTC&quot;
        ///  },
        ///  &quot;OriginTime&quot;: {
        ///    &quot;String&quot;: &quot;2018/06/18 07:5 [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string testForecast {
            get {
                return ResourceManager.GetString("testForecast", resourceCulture);
            }
        }
    }
}
