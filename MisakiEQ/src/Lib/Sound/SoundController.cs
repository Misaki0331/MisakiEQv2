using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.VisualBasic.Devices;
using MisakiEQ.Properties;
using NAudio.Wave;
using Windows.Devices.Radios;

namespace MisakiEQ.Lib.Sound
{
    public class SoundController
    {
        class Reader
        {
            public virtual double Position { get; set; }
            public virtual double Length { get; }
        }
        class AudioReader : Reader
        {
            readonly AudioFileReader aud;
            public AudioReader(AudioFileReader audio)
            {
                aud = audio;
            }
            public override double Position { get => aud.CurrentTime.TotalSeconds; set => aud.CurrentTime = TimeSpan.FromSeconds(value); }
            public override double Length { get => aud.TotalTime.TotalSeconds; }
        }
        class WaveReader : Reader
        {
            readonly WaveFileReader aud;
            public WaveReader(WaveFileReader audio)
            {
                aud = audio;
            }
            public override double Position { get => aud.CurrentTime.TotalSeconds; set => aud.CurrentTime = TimeSpan.FromSeconds(value); }
            public override double Length { get => aud.TotalTime.TotalSeconds; }
        }
        class Mp3Reader : Reader
        {
            readonly Mp3FileReader aud;
            public Mp3Reader(Mp3FileReader audio)
            {
                aud = audio;
            }
            public override double Position { get => aud.CurrentTime.TotalSeconds; set => aud.CurrentTime = TimeSpan.FromSeconds(value); }
            public override double Length { get => aud.TotalTime.TotalSeconds; }
        }
        readonly WaveOut wav = new();
        Reader readers = new();
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
                readers = new AudioReader(reader);
                wav.Init(reader);
                Log.Debug($"\"{FileName}\"のwaveファイルを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
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
                readers = new WaveReader(reader);
                wav.Init(reader);
                Log.Debug($"waveストリームを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        public bool MP3Init(byte[] stream)
        {
            try
            {
                streams = new MemoryStream(stream);
                var reader = new Mp3FileReader(streams);
                readers = new Mp3Reader(reader);
                wav.Init(reader);
                Log.Debug($"mp3ストリームを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
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
            if (!Sounds.GetInstance().Config.IsMute)
            {
                if (IsPaused) wav.Resume();
                else wav.Play();
            }
        }
        public void Replay()
        {
            if (!Sounds.GetInstance().Config.IsMute)
            {
                readers.Position = 0;
                wav.Play();
            }
        }
        public void Pause()
        {
            wav.Pause();
        }
        public void Stop()
        {
            wav.Stop();
            readers.Position = 0;
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
        public double Position { get { return readers.Position; } set { readers.Position = value; } }
        public double Length { get { return readers.Length; } }
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
            singleton ??= new Sounds();
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
                var id = sounds.Find(a => a.Name == str);
                if (id != null) return id.controller;
                sounds.Add(new(str));
                return sounds[^1].controller;
            });

        }
        public async Task<bool> DeleteSound(string str)
        {
            return await Task.Run(() =>
            {
                var id = sounds.Find(a => a.Name == str);
                if (id == null) return false;
                sounds.Remove(id);
                return true;
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
            foreach (var sound in sounds) list.Add(sound.Name);
            return list;
        }
        public void AllDeleteSound()
        {
            sounds.Clear();
        }

    }
}
