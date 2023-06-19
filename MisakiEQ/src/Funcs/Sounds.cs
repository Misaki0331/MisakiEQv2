using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MisakiEQ.Lib.Sound;
using MisakiEQ.Properties;
using MisakiEQ;
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
            singleton ??= new();
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
                    Common.Intensity max = Common.Intensity.Unknown;
                    EEWSound tmp = new(eew.Serial.EventID, eew.EarthQuake.MaxIntensity);
                    var sound = eewsound.Find(a => a.EventID == eew.Serial.EventID);
                    if (sound != null)
                    {
                        tmp = sound;
                        max=sound.MaxIntensity;
                        sound.LatestTime = DateTime.Now;
                        Index = eewsound.FindIndex(a => sound == a);
                    }
                    if (sound!=null || tmp.MaxIntensity < eew.EarthQuake.MaxIntensity)
                    {
                        tmp.MaxIntensity = eew.EarthQuake.MaxIntensity;
                        SoundController? controll = null;
                        var ins = Sounds.GetInstance();
                        switch (eew.EarthQuake.MaxIntensity)
                        {
                            case Struct.Common.Intensity.Unknown:
                            case Struct.Common.Intensity.Int0:
                                controll = await ins.GetSound("EEW_None");
                                break;
                            case Struct.Common.Intensity.Int1:
                                controll = await ins.GetSound("EEW_shindo1");
                                break;
                            case Struct.Common.Intensity.Int2:
                                controll = await ins.GetSound("EEW_shindo2");
                                break;
                            case Struct.Common.Intensity.Int3:
                                controll = await ins.GetSound("EEW_shindo3");
                                break;
                            case Struct.Common.Intensity.Int4:
                                controll = await ins.GetSound("EEW_shindo4");
                                break;
                            case Struct.Common.Intensity.Int5Down:
                            case Struct.Common.Intensity.Int5Up:
                                controll = await ins.GetSound("EEW_shindo5");
                                break;
                            case Struct.Common.Intensity.Int6Down:
                            case Struct.Common.Intensity.Int6Up:
                                controll = await ins.GetSound("EEW_shindo6");
                                break;
                            case Struct.Common.Intensity.Int7:
                                controll = await ins.GetSound("EEW_shindo7");
                                break;
                        }
                        try
                        {
                            if (controll != null && !ins.Config.IsMute)
                            {
                                controll.Volume = ins.Config.EEWVolume / 100f; ;
                                controll.Replay();
                            }
                        }catch(Exception ex)
                        {
                            Log.Error(ex);
                            Init();
                        }
                    }
                    if (sound != null) eewsound.Add(tmp);
                    var delete = eewsound.FindAll(a => (DateTime.Now - a.LatestTime).Seconds > 180);
                    foreach (var item in delete)eewsound.Remove(item);
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return false;
                }
            }

        }

        public static async void SoundEarthquake(EarthQuake eq)
        {
            try
            {
                SoundController? controll = null;
                var ins = Sounds.GetInstance();
                switch (eq.Issue.Type)
                {
                    case EarthQuake.EarthQuakeType.ScalePrompt:
                        if (eq.Details.MaxIntensity >= Common.Intensity.Int5Down)
                            controll = await ins.GetSound("Earthquake_High");
                        else
                            controll = await ins.GetSound("Earthquake_Mid");
                        break;
                    case EarthQuake.EarthQuakeType.Destination:
                    case EarthQuake.EarthQuakeType.ScaleAndDestination:
                        controll = await ins.GetSound("Earthquake_Prompt");
                        break;
                    case EarthQuake.EarthQuakeType.DetailScale:
                        controll = await ins.GetSound("Earthquake_Low");
                        break;
                }
                if (controll != null&&!ins.Config.IsMute)
                {
                    controll.Volume = ins.Config.EarthquakeVolume / 100f;
                    controll.Replay();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Init();
            }
        }
        public static async void SoundTsunami(Tsunami data)
        {
            try
            {
                SoundController? controll = null;
                var ins = Sounds.GetInstance();
                if (data.Cancelled) controll = await ins.GetSound("Tsunami_Cancel");
                else
                {
                    int watch = 0, warn = 0, mwarn = 0;
                    foreach (var area in data.Areas)
                    {
                        switch (area.Grade)
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
                        if (mwarn > 0) break;
                    }
                    if (mwarn > 0)
                        controll = await ins.GetSound("Tsunami_MajorWarn");
                    else if (warn > 0)
                        controll = await ins.GetSound("Tsunami_Warn");
                    else if (watch > 0)
                        controll = await ins.GetSound("Tsunami_Watch");
                }
                if (controll != null && !ins.Config.IsMute)
                {
                    controll.Volume = ins.Config.TsunamiVolume / 100f;
                    controll.Replay();
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Init();
            }
        }
    }
}
