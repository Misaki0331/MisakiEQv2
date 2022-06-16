﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using MisakiEQ.Log;

namespace MisakiEQ.Lib
{
    internal class SoundController
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
                if (audio != null) audio.CurrentTime = TimeSpan.FromSeconds(sec);
                if (wav != null) wav.CurrentTime = TimeSpan.FromSeconds(sec);
                if (mp3 != null) mp3.CurrentTime = TimeSpan.FromSeconds(sec);
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
        readonly WaveOut wav=new();
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
                Logger.GetInstance().Debug($"\"{FileName}\"のwaveファイルを読み込みました。");
                return true;
            }catch(Exception ex)
            {
                Logger.GetInstance().Error(ex);
                return false;
            }
        }
        public bool WaveInit(byte[] stream)
        {
            try
            {
                using var str = new MemoryStream(stream);
                var reader = new WaveFileReader(str);
                readers.Init(reader);
                wav.Init(reader);
                Logger.GetInstance().Debug($"waveストリームを読み込みました。");
                return true;
            }
            catch(Exception ex)
            {
                Logger.GetInstance().Error(ex);
                return false;
            }
        }
        public bool MP3Init(byte[] stream)
        {
            try
            {
                using var str = new MemoryStream(stream);
                var reader = new Mp3FileReader(str);
                readers.Init(reader);
                wav.Init(reader);
                Logger.GetInstance().Debug($"waveストリームを読み込みました。");
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Error(ex);
                return false;
            }
        }
        public void Dispose(){
            wav.Dispose();
        }
        public bool IsPlaying { get { return wav.PlaybackState == PlaybackState.Playing; } }
        public bool IsPaused { get { return wav.PlaybackState == PlaybackState.Paused; } }
        public void Play()
        {
            if (readers.CanPlay)
            {
                if (IsPaused) wav.Resume();
                else wav.Play();
            }
        }
        public void Replay()
        {
            if (readers.CanPlay)
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
            wav.Stop();
            readers.SetPosition(0);
        }
        public double Position { get { return readers.GetPosition(); } set { readers.SetPosition(value); } }
        public double Length { get { return readers.GetLength(); } }
        public float Volume { get { return wav.Volume; } set { wav.Volume = value; } }


    }
}
