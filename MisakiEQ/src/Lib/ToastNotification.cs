﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Microsoft.Toolkit.Uwp.Notifications;
using MisakiEQ.Background.API.EEW.OLD.JSON;
using System.Management.Automation.Runspaces;

namespace MisakiEQ.Lib
{
    public class ToastNotification
    {
        private static readonly AsyncLock NotificationLock = new();
        private static NotifyIcon? OldNotify = null;
        public static bool IsNewNotification = false;
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
            public ToastProgress(double percent, string status)
            {
                Initial(status, percent);
            }
            public ToastProgress(string title, double percent, string status)
            {
                Title = title;
                Initial(status, percent);
            }
            public ToastProgress(string title, double percent, string status, string valueOverride)
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
            public string? Title = null;
            public string Status = "";
            public string? ValueString = null;
            public bool IsIndeterminate = false;

        }
        public static void InitNotify(NotifyIcon notify)
        {
            OldNotify = notify;
            Log.Debug("フォームの通知欄を読み込みました。");
        }
        public static async void PostNotification(string title, string? index = null, string? attribution = null, DateTime? customTime = null, ToastProgress? progress = null, Image? HeroImage = null, Image? IndexImage = null, Image? Icon = null)
        {
            var notify = IsNewNotification ? NotificationDisplayMode.WPFToast : NotificationDisplayMode.WinFormToast;


            switch (notify)
            {
                case NotificationDisplayMode.WPFToast:

                    await Task.Run(async () =>
                    {

                        using (await NotificationLock.LockAsync())
                        {

                            List<string> TempFiles = new();
                            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
                            {
                                var a = new ToastContentBuilder()
                                .AddText(title);
                                if (!string.IsNullOrWhiteSpace(index)) a.AddText(index);
                                if (!string.IsNullOrWhiteSpace(attribution)) a.AddAttributionText(attribution);
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
                                Log.Debug($"トースト送信 : {title}");
                                await Task.Delay(5000);
                                for (int i = 0; i < TempFiles.Count; i++) File.Delete(TempFiles[i]);
                                TempFiles.Clear();
                            }
                        }
                    });
                    break;
                case NotificationDisplayMode.WinFormToast:
                    if (OldNotify != null)
                    {
                        string a = string.Empty;
                        if (!string.IsNullOrWhiteSpace(index))
                        {
                            a = index;
                            //if (!string.IsNullOrWhiteSpace(attribution)) a = $"{attribution}\n{index}";
                        }
                        OldNotify.ShowBalloonTip(3600000, title, a, ToolTipIcon.None);
                    }
                    else
                    {
                        Log.Error("Formの通知がnullです。");
                    }
                    break;
                case NotificationDisplayMode.PowerShell:
                    List<string> TempFiles = new();
                    if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
                    {
                        var a = new ToastContentBuilder()
                        .AddText(title);
                        if (!string.IsNullOrWhiteSpace(index)) a.AddText(index);
                        if (!string.IsNullOrWhiteSpace(attribution)) a.AddAttributionText(attribution);
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
                        var pws = $"$xml = @\"\n{a.GetXml().GetXml()}\n\"@\n" +
                            $"$XmlDocument = [Windows.Data.Xml.Dom.XmlDocument, Windows.Data.Xml.Dom.XmlDocument, ContentType = WindowsRuntime]::New()\n" +
                            $"$XmlDocument.loadXml($xml)\n" +
                            $"$AppId = @\"\n{Environment.ProcessPath}\n\"@\n" +
                            $"[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime]::CreateToastNotifier($AppId).Show($XmlDocument)";
                        var powerShell = PowerShell.Create();
                        powerShell.AddScript(pws);

                        foreach (var className in powerShell.Invoke())
                        {
                        };
                        Log.Debug($"トースト送信 : {title} {powerShell.HadErrors}");
                        if(powerShell.HadErrors)foreach(var error in powerShell.Streams.Error)
                            {
                                Log.Error(error.Exception.ToString());
                            }
                        await Task.Delay(5000);
                        for (int i = 0; i < TempFiles.Count; i++) File.Delete(TempFiles[i]);
                        TempFiles.Clear();
                    }
                    break;
                case NotificationDisplayMode.Window:
                    //ToDo:ここにウィンドウモードの通知を書く
                    break;
            }
        }
        public enum NotificationDisplayMode
        {
            WPFToast,
            WinFormToast,
            PowerShell,
            Window
        }
    }
}
