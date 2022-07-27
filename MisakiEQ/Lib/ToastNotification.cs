using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MisakiEQ.Lib
{
    internal class ToastNotification
    {
        public class ToastProgress
        {
            private void Initial(string status, double percent)
            {
                this.Status = status;
                if (double.IsNaN(percent))
                {
                    percent = 0;
                    IsIndeterminate = true;
                }
                if (percent < 0) percent = 0;
                if (percent > 1) percent = 1;
                Percent = percent;
            }
            public ToastProgress(double percent,string status)
            {
                Initial(status, percent);
            }
            public ToastProgress(string title, double percent,string status)
            {
                Title = title;
                Initial(status, percent);
            }
            public ToastProgress(string title, double percent, string status,string valueOverride)
            {
                ValueString = valueOverride;
                Title = title;
                Initial(status, percent);
            }
            public ToastProgress(double percent, string status, string valueOverride)
            {
                ValueString = valueOverride;
                Initial(status, percent);
            }
            public double Percent;
            public string? Title=null;
            public string Status = "";
            public string? ValueString = null;
            public bool IsIndeterminate = false;

        }
        public static async void PostNotification(string title, string? index=null,string? attribution=null,DateTime? customTime=null,ToastProgress? progress=null,Image? HeroImage=null, Image? IndexImage=null, Image? Icon=null)
        {
            await Task.Run(() => {
                List<string> TempFiles = new();
                var a = new ToastContentBuilder()
                .AddText(title);
                if (index != null) a.AddText(index);
                if(attribution != null) a.AddAttributionText(attribution);
                if (progress != null) a.AddProgressBar(progress.Title, progress.Percent, progress.IsIndeterminate, progress.ValueString, progress.Status);
                if (customTime != null) a.AddCustomTimeStamp((DateTime)customTime);
                if (HeroImage != null)
                {
                    string name = Path.GetTempFileName();
                    TempFiles.Add(name);
                    HeroImage.Save(name);
                    Uri uri = new(name);
                    a.AddHeroImage(uri);
                }
                if (IndexImage != null)
                {
                    string name = Path.GetTempFileName();
                    TempFiles.Add(name);
                    IndexImage.Save(name);
                    Uri uri = new(name);
                    a.AddInlineImage(uri);
                }

                if (Icon != null)
                {
                    string name = Path.GetTempFileName();
                    TempFiles.Add(name);
                    Icon.Save(name);
                    Uri uri = new(name);
                    a.AddAppLogoOverride(uri);
                }
                a.Show();
                Log.Instance.Debug($"トースト送信 : {title}");
                Thread.Sleep(5000);
                for(int i=0;i<TempFiles.Count;i++) File.Delete(TempFiles[i]);
                TempFiles.Clear();
            });
        }
    }
}
