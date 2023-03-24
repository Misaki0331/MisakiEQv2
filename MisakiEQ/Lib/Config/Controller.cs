using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using MisakiEQ.Background;
/*
namespace MisakiEQ.Lib.ConfigV2.Components.Common{
    public class CommonData{
        public string Name {get; internal set;} = string.Empty;
        public string Title {get; internal set;} = string.Empty;
        public string Description {get; internal set;} = string.Empty;
        public Component ComponentType {get; internal set;} = Component.Unknown;
        public enum Component {
            Unknown,
            Toggle,
            Numeric,
            TrackBar,
            TextBox,
            Command,
            ReadOnly,
            ComboBox
        };
    }
}
namespace MisakiEQ.Lib.ConfigV2.Components{
    ///<summary>
    ///Toggleボタン用のコンポーネント
    ///</summary>
    public class Toggle : Common.CommonData{
        public bool Default;   //デフォルト値
        public bool Value;     //現在の値
        //UI関連
        public string OnText;  //オン設定時のテキスト
        public string OffText; //オフ設定時のテキスト
        ///<summary>
        ///
        ///</summary>
        public Toggle(string name,string title, string description = "", bool def = false, string onText = "", string offText = "")
        {
            //共通
            Name = name;
            Title = title;
            Description = description;
            //初期値の値
            Default = def;
            Value = def;
            //ここにオンもしくはオフ状態のテキスト
            OnText = onText;
            OffText = offText;

            ComponentType = Component.Toggle;
        }
    }

    ///<summary>
    ///
    ///</summary>
    public class Numeric : Common.CommonData{
        public double Minimum;
        public double Maximum;
        public double Default;
        public double Value;
        //表示コンフィグ
        public int Decimal;
        public string UnitName;
        public Numeric(string name,string title, string description = "", double min = double.NegativeInfinity, double max = double.PositiveInfinity, double def = 0, string unitName = "", int DecimalValue = 0)
        {
            Name = name;
            Title = title;
            Description = description;

            if(DecimalValue<0|| DecimalValue >= 10)throw new ArgumentException(); //小数点関係のエラーも例外投げる
            if(min>max)throw new ArgumentException();    //最大値より最小値が大きい場合は例外投げる
            Minimum = min;
            Maximum = max;
            Default = def;
            Value = def;
            UnitName = unitName;
            Decimal = DecimalValue;
            
            ComponentType = Component.Numeric;
        }
    }

    public class Trackbar : Common.CommonData{
        public int Minimum;
        public int Maximum;
        public int Default;
        public int Value;
        //表示コンフィグ
        string UnitName;
        public Trackbar(string name,string title, string description = "", int min = int.MinValue, int max = int.MaxValue, int def = 0, string unitName = "")
        {
            Name = name;
            Title = title;
            Description = description;
            Minimum = min;
            Maximum = max;
            Default = def;
            Value = def;
            UnitName = unitName;
            
            ComponentType = Component.TrackBar;
        }
    }
    public class Textbox : Common.CommonData{
        public string Default;
        public string Value;
        public Textbox(string name, string title, string description = "",  string def = "")
        {
            Name = name;
            Title = title;
            Description = description;
            Default = def;
            Value = def;
            ComponentType = Component.TextBox;
        }
    }
    public class Command : Common.CommonData{
        Action FunctionAction;
        public string ReadyString;
        public string WorkingString;
        public Command(string name, string title, string description, string ReadyTitle, Action action 
        ,string? WorkingTitle = null)
        {
            Description = description;
            Name = name;
            Title = title;
            FunctionAction = action;
            ReadyString = ReadyTitle;
            if (WorkingTitle != null) WorkingString = WorkingTitle;
            else WorkingString = ReadyTitle;

            ComponentType = Component.Command;
        }
        void DoAction(){
            FunctionAction();
        }
        /// <summary>
        /// ボタンの有効状態が変更された際に実行されるイベントです。
        /// </summary>
        public EventHandler<EventArgs>? ButtonChanged = null;
    }
    public class ReadOnly : Common.CommonData{
        public event EventHandler<EventArgs>? ValueChanged;
        string _value = "";
        public string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value= value;
                    if (ValueChanged != null)
                    {
                        ValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }
        public ReadOnly(string name, string title, string description)
        {
            Description = description;
            Name = name;
            Title = title;
            ComponentType = Component.ReadOnly;
        }
    }
    public class Combobox : Common.CommonData{
        int Default;
        int Value;
        List<string> ComboStrings;
        public Combobox(string name, string title, string description, int def, List<string> list)
        {
            ComboStrings = new();
            Init(name,title,description,def,list.Count);
            for (int i = 0; i < list.Count; i++) ComboStrings.Add(list[i]);
        }
        public Combobox(string name, string title, string description, int def, string[] list)
        {
            ComboStrings = new();
            Init(name,title,description,def,list.Length);
            for (int i = 0; i < list.Length; i++) ComboStrings.Add(list[i]);
        }
        private void Init(string name, string title, string description, int def,int count){
            if(def>=count)throw new ArgumentException();
            ComboStrings=new List<string>();
            Description = description;
            Name = name;
            Title = title;
            Default = def;
            Value = def;
            ComponentType = Component.ComboBox;
        }
    }

}
/*
public class IndexData
        {

            public IndexData(string name, string title, string description, int def, List<string> list)
            {
                Type = "combo";
                Description = description;
                Name = name;
                LongDefault = def;
                LongValue = def;
                Title = title;
                for (int i = 0; i < list.Count; i++) ComboStrings.Add(list[i]);

            }
            public IndexData(string name, string title, string description, int def, string[] list)
            {
                Type = "combo";
                Description = description;
                Name = name;
                LongDefault = def;
                LongValue = def;
                Title = title;
                for (int i = 0; i < list.Length; i++) ComboStrings.Add(list[i]);

            }
            /// <summary>
            /// ラムダ式を設定します。
            /// IndexDataのタイプが Function である必要があります。
            /// そうでない場合は例外がスローされます。
            /// </summary>
            /// <param name="act">実行関数</param>
            /// <exception cref="InvalidOperationException"></exception>
            public void SetAction(Action act)
            {
                //if (Type != "function") throw new InvalidOperationException($"{Type}型は関数設定できません！");
                FunctionAction = act;
            }
            public EventHandler? ValueChanged = null;

            public bool ValueDefaultable()
            {
                return Type switch
                {
                    "long" => true,
                    "string" => true,
                    "bool" => true,
                    "combo" => true,
                    _ => false
                };
            }
            /// <summary>
            /// 値を設定または取得します。
            /// </summary>
            public object Value
            {
                get
                {
                    return Type switch
                    {
                        "long" => LongValue,
                        "string" => StringValue,
                        "bool" => BooleanValue,
                        "readonly" => StringValue,
                        "combo" => LongValue,
                        _ => throw new InvalidDataException($"[{Name}]は値がありません。"),
                    };
                }
                set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(short))
                            {
                                if (LongMin < short.MinValue || LongMax > short.MaxValue) throw new ArgumentOutOfRangeException($"[{Name}]の設定可能値はshort型からオーバーフローしています。long型で変更してください。");
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else
                            if (value.GetType() == typeof(int))
                            {
                                if (LongMin < int.MinValue || LongMax > int.MaxValue) throw new ArgumentOutOfRangeException($"[{Name}]の設定可能値はint型オーバーフローしています。long型で変更してください。");
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} < {LongMin}");
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{value} > {LongMax}");
                                if (LongValue != (long)value)
                                {
                                    LongValue = (long)value;
                                    FunctionAction?.Invoke();
                                }
                            }
                            else
                                throw new InvalidCastException($"[{Name}]は整数型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            break;
                        case "string":
                            if (value.GetType() != typeof(string)) throw new InvalidCastException($"[{Name}]はstring型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            if ((string)value!=StringValue)
                            {
                                StringValue = (string)value;
                                try { ValueChanged?.Invoke(this, EventArgs.Empty); } catch(Exception ex) { Log.Instance.Warn(ex.Message); }
                            }
                            break;
                        case "bool":
                            if(value.GetType()!=typeof(bool)) throw new InvalidCastException($"[{Name}]はbool型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            BooleanValue = (bool)value;
                            break;
                        case "readonly":
                            if (value.GetType() != typeof(string)) throw new InvalidCastException($"[{Name}]はstring型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            if ((string)value != StringValue)
                            {
                                StringValue = (string)value;
                                try { ValueChanged?.Invoke(this, EventArgs.Empty); } catch (Exception ex) { Log.Instance.Warn(ex.Message); }
                            }
                            break;
                        case "combo":
                            if (value.GetType() == typeof(string))
                            {
                                for(int i = 0; i < ComboStrings.Count; i++)
                                {
                                    if(ComboStrings[i] == (string)value)
                                    {
                                        LongValue = i;
                                        break;
                                    }
                                    if(i == ComboStrings.Count - 1)
                                        throw new ArgumentException($"\"{value}\"は[{Name}]のコレクションの中には存在しませんでした。", nameof(value));
                                }
                            }
                            else if(value.GetType() == typeof(int))
                            {
                                int v = (int)value;
                                if (v < 0 || v >= ComboStrings.Count) throw new ArgumentOutOfRangeException($"[{Name}]のコレクションに対応する配列が見つかりません。設定した値:{value} 配列数:{ComboStrings.Count}");
                                LongValue = v;
                            }
                            else if (value.GetType() == typeof(long))
                            {
                                long v = (long)value;
                                if (v < 0 || v >= ComboStrings.Count) throw new ArgumentOutOfRangeException($"[{Name}]のコレクションに対応する配列が見つかりません。設定した値:{value} 配列数:{ComboStrings.Count}");
                                LongValue = v;
                            }
                            else
                            {
                                throw new InvalidCastException($"[{Name}]はstring型,int型,long型しか対応していませんが、{value.GetType()}で変更しようとしました。");
                            }
                            break;
                        default:
                            throw new InvalidDataException($"[{Name}は値を設定することができません。");
                    }
                }
            }
            /// <summary>
            /// コンフィグファイルの読書き用に使用されます。
            /// </summary>
            public string Config
            {
                get
                {
                    if (Type == "function"||Type== "readonly") return string.Empty;
                    return Name+"="+Type switch
                    {
                        "long" => LongValue.ToString(),
                        "string" => StringValue.Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D"),
                        "bool" => BooleanValue.ToString(),
                        "combo" => ComboStrings[(int)LongValue].Replace("%", "%%").Replace("=", "%3D").Replace("\n", "%0D"),
                        _ => throw new NullReferenceException("タイプが一致しません。"),
                    }+Environment.NewLine;
                }
                set
                {
                    switch (Type)
                    {
                        case "long":
                            long val = long.Parse(value);
                            if (val < LongMin) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} < {LongMin}");
                            if (val > LongMax) throw new ArgumentOutOfRangeException($"[{Name}]セットされた値が範囲外です。{val} > {LongMax}");
                            LongValue = val;
                            break;
                        case "string":
                            string? str = (string?)value;
                            if (string.IsNullOrEmpty(str)) str = string.Empty;
                            StringValue = str.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                            break;
                        case "bool":
                            if (value.ToLower() == "true" || value == "0")
                            {
                                BooleanValue = true;
                            }
                            else if (value.ToLower() == "false" || value == "1")
                            {
                                BooleanValue = false;
                            }
                            else
                            {
                                throw new ArgumentException($"bool型ではない、もしくは正しく検出できませんでした。val=\"{value}\"", nameof(value));
                            }
                            break;
                        case "combo":
                            if (value.StartsWith("="))
                            {
                                if(int.TryParse(value.Replace("=",""),out int num))
                                {
                                    if (ComboStrings.Count > num && 0 <= num)
                                    {
                                        LongValue = num;
                                        break;
                                    }
                                    else throw new ArgumentOutOfRangeException(nameof(value), $"\"{num}\" は \"{ComboStrings.Count}\" の範囲外です。");
                                }
                                else
                                {
                                    throw new InvalidDataException($"\"{value}\"は数値に変換できませんでした。");
                                }
                            }
                            for (int i = 0; i < ComboStrings.Count; i++)
                            {
                                string vals = value.Replace("%0D", "\n").Replace("%3D", "=").Replace("%%", "%");
                                if (ComboStrings[i] == vals)
                                {
                                    LongValue = i;
                                    break;
                                }

                                if (i == ComboStrings.Count - 1)
                                    throw new ArgumentException($"\"{vals}\"は[{Name}]のコレクションの中には存在しませんでした。", nameof(value));
                            }
                            break;
                    }
                }
            }
            /// <summary>
            /// Function関数の場合は関数が実行されます。<br/>
            /// そうでない場合は例外がスローされます。
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public void ExecuteAction()
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                Task.Run(() => { FunctionAction?.Invoke(); });
            }
            bool __buttonEnable = true;
            /// <summary>
            /// ボタンが使用できるかどうかを設定します。<br/>
            /// Function 型では無い場合は例外がスローされます。
            /// </summary>
            public bool ButtonEnable
            {
                get
                {
                    if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                    return __buttonEnable;
                }
                set
                {
                    if(Type!="function") throw new InvalidCastException($"{Type}型は実行できません。");
                    if (__buttonEnable != value)
                    {
                        __buttonEnable = value;
                        try
                        {
                            ButtonChanged?.Invoke(this, EventArgs.Empty);
                        }catch(Exception ex)
                        {
                            Log.Instance.Warn(ex.Message);
                        }
                    }
                }
            }
            /// <summary>
            /// ボタンの有効状態が変更された際に実行されるイベントです。
            /// </summary>
            public EventHandler<EventArgs>? ButtonChanged = null;
            /// <summary>
            /// 有効中かそれ以外でボタンのテキストに設定する文字列を取得します。
            /// Function型以外は例外がスローされます。
            /// </summary>
            /// <param name="IsWorking">有効中かどうか</param>
            /// <returns>対応する文字列型</returns>
            /// <exception cref="InvalidCastException"></exception>
            public string GetButton(bool IsWorking)
            {
                if (Type != "function") throw new InvalidCastException($"{Type}型は実行できません。");
                if (IsWorking) return FunctionWorking?? FunctionReady;
                else return FunctionReady;
            }
            public string GetToggleOffText()
            {
                return Type switch
                {
                    "bool" => BoolToggleOff,
                    _ => throw new ArgumentException("タイプが一致しません。")
                };
            }
            public string GetToggleOnText()
            {
                return Type switch
                {
                    "bool" => BoolToggleOn,
                    _ => throw new ArgumentException("タイプが一致しません。")
                };
            }

            public double GetDisplayMag()
            {
                return Type switch
                {
                    "long" => DisplayMag,
                    _ => throw new ArgumentException("タイプが一致しません。"),
                };
            }

            string _type="Unknown";
            string _name="";
            string _title = "";
            string _unitName = "";
            string _description = "";

            /// <summary>
            /// 関数の型名
            /// </summary>
            public string Type
            {
                get => _type; 
                private set => _type = value; 
            }
            /// <summary>
            /// 関数の名前
            /// </summary>
            public string Name
            {
                get => _name; 
                private set => _name = value;
            }
            /// <summary>
            /// 関数の表示する文字列
            /// </summary>
            public string Title
            {
                get => _title;
                private set => _title = value;
            }
            /// <summary>
            /// 数量名の設定<br/>
            /// long型以外は例外がスローされます。
            /// </summary>
            public string UnitName
            {
                get { if (Type != "long") throw new InvalidCastException($"\"{Type}\"longに変換できません。");
                    return _unitName; } 
                private set => _unitName = value; 
            }
            /// <summary>
            /// 説明
            /// </summary>
            public string Description
            {
                get => _description;
                private set => _description = value;
            }
            public object Min
            {
                get
                {
                    return Type switch
                    {
                        "long" => (object)LongMin,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch(Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value > LongMax) throw new ArgumentOutOfRangeException($"値は必ずMaxより小さければなりません。 {(long)value}<={LongMax}");
                                LongMin = (long)value;
                            }else if(value.GetType() == typeof(int))
                            {
                                if ((int)value > LongMax) throw new ArgumentOutOfRangeException($"値は必ずMaxより小さければなりません。 {(long)value}<={LongMax}");
                                LongMin = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                        break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            public object Max
            {
                get
                {
                    return Type switch
                    {
                        "long" => (object)LongMax,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongMax = (long)value;
                            }
                            else if (value.GetType() == typeof(int))
                            {
                                if ((int)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongMax = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                            break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            /// <summary>
            /// デフォルト値の取得
            /// </summary>
            public object Default
            {
                get
                {
                    return Type switch
                    {
                        "long" => LongDefault,
                        "string" => StringDefault,
                        "bool" => BooleanDefault,
                        "combo"=> LongDefault,
                        _ => throw new ArgumentException("タイプが一致しません。"),
                    };
                }
                private set
                {
                    switch (Type)
                    {
                        case "long":
                            if (value.GetType() == typeof(long))
                            {
                                if ((long)value < LongMin) throw new ArgumentOutOfRangeException($"値は必ずMinより大きければなりません。 {(long)value}>={LongMin}");
                                LongDefault = (long)value;
                            }
                            else if (value.GetType() == typeof(int))
                            {
                                LongDefault = (int)value;
                            }
                            else
                            {
                                throw new InvalidCastException($"{value.GetType()}は整数型に変換できません。");
                            }
                            break;
                        default: throw new ArgumentException("タイプが一致しません。");
                    }
                }
            }
            public string[] Items { get
                {
                    if (Type != "combo") throw new ArgumentException("タイプが一致しません");
                    string[] strings = new string[ComboStrings.Count];
                    for (int i = 0; i < ComboStrings.Count; i++) strings[i] = ComboStrings[i];
                    return strings;
                } }
            long LongValue;
            long LongMin;
            long LongMax;
            long LongDefault;
            readonly double DisplayMag;
            string StringValue="";
            readonly string StringDefault="";
            bool BooleanValue = false;
            readonly bool BooleanDefault = false;
            readonly string BoolToggleOn = "";
            readonly string BoolToggleOff = "";
            Action? FunctionAction = null;
            readonly string FunctionReady = "";
            readonly string? FunctionWorking = null;
            readonly List<string> ComboStrings = new();
        }
    }*/