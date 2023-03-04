using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MisakiEQ.Properties;
using NAudio.Wave;

namespace MisakiEQ.Lib.Sound
{
    public class SoundController
    {
        class Reader
        {
            AudioFileReader? audio = null;
            WaveFileReader? wav = null;
            Mp3FileReader? mp3 = null;
            void ClearReader()
            {
                audio = null;
                wav = null;
                mp3 = null;
            }
            public void Init(AudioFileReader aud)
            {
                ClearReader();
                audio = aud;
            }
            public void Init(WaveFileReader aud)
            {
                ClearReader();
                wav = aud;
            }
            public void Init(Mp3FileReader aud)
            {
                ClearReader();
                mp3 = aud;
            }
            public void SetPosition(double sec)
            {
                try
                {
                    if (audio != null) audio.CurrentTime = TimeSpan.FromSeconds(sec);
                    if (wav != null) wav.CurrentTime = TimeSpan.FromSeconds(sec);
                    if (mp3 != null) mp3.CurrentTime = TimeSpan.FromSeconds(sec);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                }
            }
            public double GetPosition()
            {
                if (audio != null) return audio.CurrentTime.TotalSeconds;
                if (wav != null) return wav.CurrentTime.TotalSeconds;
                if (mp3 != null) return mp3.CurrentTime.TotalSeconds;
                return double.NaN;
            }
            public double GetLength()
            {
                if (audio != null) return audio.TotalTime.TotalSeconds;
                if (wav != null) return wav.TotalTime.TotalSeconds;
                if (mp3 != null) return mp3.TotalTime.TotalSeconds;
                return double.NaN;
            }
            public bool CanPlay
            {
                get
                {
                    if (audio != null || wav != null || mp3 != null) return true;
                    return false;
                }
            }
        }
        readonly WaveOut wav = new();
        readonly Reader readers = new();
        public SoundController()
        {
        }
        public SoundController(string FileName)
        {
            FileInit(FileName);
        }

        public bool FileInit(string FileName)
        {
            try
            {

                var reader = new AudioFileReader(FileName);
                readers.Init(reader);
                wav.Init(reader);
                Log.Instance.Debug($"\"{FileName}\"のwaveファイルを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
        }
        MemoryStream? streams = null;
        public bool WaveInit(byte[] stream)
        {
            try
            {
                streams = new MemoryStream(stream);
                var reader = new WaveFileReader(streams);
                readers.Init(reader);
                wav.Init(reader);
                Log.Instance.Debug($"waveストリームを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
        }
        public bool MP3Init(byte[] stream)
        {
            try
            {
                streams = new MemoryStream(stream);
                var reader = new Mp3FileReader(streams);
                readers.Init(reader);
                wav.Init(reader);
                Log.Instance.Debug($"mp3ストリームを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
        }
        public void Dispose()
        {
            wav.Dispose();
        }
        public bool IsPlaying { get { return wav.PlaybackState == PlaybackState.Playing; } }
        public bool IsPaused { get { return wav.PlaybackState == PlaybackState.Paused; } }
        public void Play()
        {
            if (readers.CanPlay&&!Sounds.GetInstance().Config.IsMute)
            {
                if (IsPaused) wav.Resume();
                else wav.Play();
            }
        }
        public void Replay()
        {
            if (readers.CanPlay && !Sounds.GetInstance().Config.IsMute)
            {
                readers.SetPosition(0);
                wav.Play();
            }
        }
        public void Pause()
        {
            wav.Pause();
        }
        public void Stop()
        {
            if (readers.CanPlay)
            {
                wav.Stop();
                readers.SetPosition(0);
            }
        }
        public void SetDeviceID(int ID,string DeviceName)
        {
            var list=new List<string>();
            for (int i = 0; i < WaveOut.DeviceCount; i++)
                list.Add(WaveOut.GetCapabilities(i).ProductName);
            if(list.Count<ID||list.Count>=ID)
                throw new ArgumentOutOfRangeException(nameof(list),"そのデバイスは存在しないIDである為、指定できません。");
            if (list[ID] != DeviceName)
                throw new InvalidDataException("デバイス名とデバイスIDが一致しない為、指定できません。");
            wav.DeviceNumber = ID;
        }
        public int DeviceID { get { return wav.DeviceNumber; } }
        public double Position { get { return readers.GetPosition(); } set { readers.SetPosition(value); } }
        public double Length { get { return readers.GetLength(); } }
        public float Volume { get { return wav.Volume; } set { wav.Volume = value; } }


    }
    public class Sounds
    {
        private static Sounds? singleton = null;
        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        public static Sounds GetInstance()
        {
            if (singleton == null)
            {
                singleton = new Sounds();
            }
            return singleton;
        }
        public Config Config = new();
        public class SoundList
        {
            public SoundList(string name)
            {
                Name = name;
                controller = new();
            }
            public readonly SoundController controller;
            public string Name { get; }
        }
        readonly List<SoundList> sounds = new();
        public Sounds()
        {


        }
        public async Task<SoundController> GetSound(string str)
        {
            return await Task.Run(() =>
            {
                for (int i = 0; i < sounds.Count; i++)
                {
                    if (sounds[i].Name == str) return sounds[i].controller;
                }
                sounds.Add(new(str));
                return sounds[^1].controller;
            });

        }
        public async Task<bool> DeleteSound(string str)
        {
            return await Task.Run(() =>
            {
                for (int i = 0; i < sounds.Count; i++)
                {
                    if (sounds[i].Name == str)
                    {
                        sounds.RemoveAt(i);
                        return true;
                    }
                }
                return false;
            });
        }
        //Todo:デバイスデータを返すようにする。GUIDで指定する予定
        public class DeviceData{
            public DeviceData(string name)
            {

            }
        }
        public List<string> GetSoundDevice()
        {
            var list = new List<string>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);
                //capabilities.NameGuid
                list.Add(capabilities.ProductName);
            }
            return list;
        }
        public int GetSoundListCount()
        {
            return sounds.Count;
        }
        public List<string> GetSoundList()
        {
            List<string> list = new();
            for (int i = 0; i < sounds.Count; i++) list.Add(sounds[i].Name);
            return list;
        }
        public void AllDeleteSound()
        {
            sounds.Clear();
        }

    }
}
