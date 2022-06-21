using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Lib;
using MisakiEQ.Properties;
using MisakiEQ.Struct;
namespace MisakiEQ.Funcs
{


    public class SoundCollective
    {
        private class EEWSound
        {
            public EEWSound(string Event, Struct.Common.Intensity max)
            {
                LatestTime = DateTime.Now;
                EventID = Event;
                MaxIntensity = max;
            }
            public string EventID { get; set; } = string.Empty;
            public Struct.Common.Intensity MaxIntensity { get; set; } = 0;
            public DateTime LatestTime { get; set; } = DateTime.Now;
        }
        private static SoundCollective? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static SoundCollective GetInstance()
        {
            if (singleton == null)
            {
                singleton = new();
            }
            return singleton;
        }
        public static async void Init()
        {
            var snd = Sounds.GetInstance();

            (await snd.GetSound("Earthquake_High")).MP3Init(Sound.Earthquake_High);
            (await snd.GetSound("Earthquake_Mid")).MP3Init(Sound.Earthquake_Mid);
            (await snd.GetSound("Earthquake_Low")).MP3Init(Sound.Earthquake_Low);
            (await snd.GetSound("Earthquake_Prompt")).MP3Init(Sound.Earthquake_Prompt);
            (await snd.GetSound("EEW_None")).MP3Init(Sound.EEW_None);
            (await snd.GetSound("EEW_shindo1")).MP3Init(Sound.EEW_shindo1);
            (await snd.GetSound("EEW_shindo2")).MP3Init(Sound.EEW_shindo2);
            (await snd.GetSound("EEW_shindo3")).MP3Init(Sound.EEW_shindo3);
            (await snd.GetSound("EEW_shindo4")).MP3Init(Sound.EEW_shindo4);
            (await snd.GetSound("EEW_shindo5")).MP3Init(Sound.EEW_shindo5);
            (await snd.GetSound("EEW_shindo6")).MP3Init(Sound.EEW_shindo6);
            (await snd.GetSound("EEW_shindo7")).MP3Init(Sound.EEW_shindo7);
            (await snd.GetSound("Tsunami_Cancel")).MP3Init(Sound.Tsunami_Cancel);
            (await snd.GetSound("Tsunami_Watch")).MP3Init(Sound.Tsunami_Watch);
            (await snd.GetSound("Tsunami_Warn")).MP3Init(Sound.Tsunami_Warn);
            (await snd.GetSound("Tsunami_MajorWarn")).MP3Init(Sound.Tsunami_MajorWarn);
        }
        readonly Lib.AsyncLock EEW_Lock = new();
        readonly List<EEWSound> eewsound = new();
        public async Task<bool> SoundEEW(Struct.EEW eew)
        {
            using (await EEW_Lock.LockAsync())
            {
                try
                {
                    int Index = -1;
                    bool IsNew = true;
                    Struct.Common.Intensity max = Struct.Common.Intensity.Unknown;
                    EEWSound tmp = new(eew.Serial.EventID, eew.EarthQuake.MaxIntensity);
                    for (int i = 0; i < eewsound.Count; i++)
                    {
                        if (eewsound[i].EventID == eew.Serial.EventID)
                        {
                            tmp = eewsound[i];
                            max = eewsound[i].MaxIntensity;
                            eewsound[i].LatestTime = DateTime.Now;
                            Index = i;
                            IsNew = false;
                        }
                    }
                    if (IsNew || tmp.MaxIntensity < eew.EarthQuake.MaxIntensity)
                    {
                        tmp.MaxIntensity = eew.EarthQuake.MaxIntensity;
                        switch (eew.EarthQuake.MaxIntensity)
                        {
                            case Struct.Common.Intensity.Unknown:
                            case Struct.Common.Intensity.Int0:
                                (await Sounds.GetInstance().GetSound("EEW_None")).Replay();
                                break;
                            case Struct.Common.Intensity.Int1:
                                (await Sounds.GetInstance().GetSound("EEW_shindo1")).Replay();
                                break;
                            case Struct.Common.Intensity.Int2:
                                (await Sounds.GetInstance().GetSound("EEW_shindo2")).Replay();
                                break;
                            case Struct.Common.Intensity.Int3:
                                (await Sounds.GetInstance().GetSound("EEW_shindo3")).Replay();
                                break;
                            case Struct.Common.Intensity.Int4:
                                (await Sounds.GetInstance().GetSound("EEW_shindo4")).Replay();
                                break;
                            case Struct.Common.Intensity.Int5Down:
                            case Struct.Common.Intensity.Int5Up:
                                (await Sounds.GetInstance().GetSound("EEW_shindo5")).Replay();
                                break;
                            case Struct.Common.Intensity.Int6Down:
                            case Struct.Common.Intensity.Int6Up:
                                (await Sounds.GetInstance().GetSound("EEW_shindo6")).Replay();
                                break;
                            case Struct.Common.Intensity.Int7:
                                (await Sounds.GetInstance().GetSound("EEW_shindo7")).Replay();
                                break;
                        }

                    }

                    for (int i = eewsound.Count - 1; i >= 0; i--)
                    {
                        TimeSpan T = DateTime.Now - eewsound[i].LatestTime;
                        if (T.Seconds > 180) eewsound.RemoveAt(i);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Logger.GetInstance().Error(ex);
                    return false;
                }
            }

        }

        public static async void SoundEarthquake(Struct.EarthQuake eq)
        {
            switch (eq.Issue.Type)
            {
                case Struct.EarthQuake.EarthQuakeType.ScalePrompt:
                    if (eq.Details.MaxIntensity >= Struct.Common.Intensity.Int5Down)
                        (await Sounds.GetInstance().GetSound("Earthquake_High")).Replay();
                    else 
                        (await Sounds.GetInstance().GetSound("Earthquake_Mid")).Replay();
                    break;
                case Struct.EarthQuake.EarthQuakeType.Destination:
                case Struct.EarthQuake.EarthQuakeType.ScaleAndDestination:
                    (await Sounds.GetInstance().GetSound("Earthquake_Prompt")).Replay();
                    break;
                case Struct.EarthQuake.EarthQuakeType.DetailScale:
                    (await Sounds.GetInstance().GetSound("Earthquake_Low")).Replay();
                    break;
            }
        }
        public static async void SoundTsunami(Struct.Tsunami data)
        {
            if (data.Cancelled)
            {
                (await Sounds.GetInstance().GetSound("Tsunami_Cancel")).Replay();
            }
            else
            {
                int watch = 0, warn = 0, mwarn = 0;
                for (int i = 0; i < data.Areas.Count; i++)
                {
                    switch (data.Areas[i].Grade)
                    {
                        case Tsunami.TsunamiGrade.Watch:
                            watch++;
                            break;
                        case Tsunami.TsunamiGrade.Warning:
                            warn++;
                            break;
                        case Tsunami.TsunamiGrade.MajorWarning:
                            mwarn++;
                            break;
                    }
                }
                if (mwarn > 0)
                    (await Sounds.GetInstance().GetSound("Tsunami_MajorWarn")).Replay();
                else if (warn > 0)
                    (await Sounds.GetInstance().GetSound("Tsunami_Warn")).Replay();
                else if (watch > 0)
                    (await Sounds.GetInstance().GetSound("Tsunami_Watch")).Replay();
            }
        }
    }
}
