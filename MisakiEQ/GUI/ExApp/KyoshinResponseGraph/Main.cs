﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.ExApp.KyoshinGraphWindow
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin += UpdateImage;
            Icon = Properties.Resources.Logo_MainIcon;
        }
        private async void UpdateImage(object? sender, EventArgs args)
        {
            Stopwatch sw = new();
            sw.Start();
            List<Task<Lib.KyoshinAPI.KyoshinObervation.AnalysisResult>> tasks = new();
            List<string> taskstr = new();
            switch (SettingWindow.DisplayMode)
            {
                //リアルタイム震度
                case 0:
                    if (SettingWindow.Check3Mode && SettingWindow.Check4Mode)
                        tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg));
                    var a = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_s;
                    var b = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.jma_b;
                    if (SettingWindow.Check2Mode) (b, a) = (a, b);
                    tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(a));
                    if (SettingWindow.Check1Mode) tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(b));
                    if (SettingWindow.Check3Mode && !SettingWindow.Check4Mode)
                        tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.EstShindoImg));
                    break;
                //加速度
                case 1:
                    a = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_s;
                    b = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.acmap_b;
                    if (SettingWindow.Check2Mode) (b, a) = (a, b);
                    tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(a));
                    if (SettingWindow.Check1Mode) tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(b));
                    break;
                //速度
                case 2:
                    a = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.vcmap_s;
                    b = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.vcmap_b;
                    if (SettingWindow.Check2Mode) (b, a) = (a, b);
                    tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(a)); 
                    if (SettingWindow.Check1Mode) tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(b));
                    break;
                //変位
                case 3:
                    a = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_s;
                    b = Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.dcmap_b;
                    if (SettingWindow.Check2Mode) (b, a) = (a, b);
                    tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(a));
                    if (SettingWindow.Check1Mode) tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(b));
                    break;
                //応答速度
                case 4:
                    List<Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType> c = new()
                    {
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_s,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_s,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_s,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_s,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_s,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_s
                    };
                    List<Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType> d = new()
                    {
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_b,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_b,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_b,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_b,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_b,
                        Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_b
                    };
                    if (SettingWindow.Check3Mode)
                    {
                        c.Reverse();
                        d.Reverse();
                    }
                    if (SettingWindow.Check4Mode) d.Reverse();
                    if (SettingWindow.Check2Mode) (d, c) = (c, d);
                    for (int i = 0; i < c.Count; i++)tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(c[i]));
                    if(SettingWindow.Check1Mode) for (int i = 0; i < d.Count; i++) tasks.Add(Lib.KyoshinAPI.KyoshinObervation.GetPoints(d[i]));
                    break;
            }
            await Task.WhenAll(tasks);
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            double WindowTopBottom = 0.05;
            var create = await Task.Run(() =>
            {
                float space =  (h * (float)WindowTopBottom > 20 ? h * (float)WindowTopBottom : 20) ;
                float sth = (h-(h*(float)WindowTopBottom / tasks.Count * 2)- space * (tasks.Count>6?2:1) ) / (tasks.Count);
                if (sth < 0) sth = 1;
                Bitmap bitmap = new(w, h);
                var g = Graphics.FromImage(bitmap);
                g.FillRectangle(Brushes.Black, 0, 0, w,h);
                using Font font1 = new("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
                var rect1 = new RectangleF(0, (h * (float)WindowTopBottom / tasks.Count), w,space);
                var stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Far
                };
                var stringname = "";
                switch (SettingWindow.DisplayMode)
                {
                    case 0:
                        stringname = "リアルタイム震度";
                        break;
                    case 1:
                        stringname = "最大加速度 [PGA] (gal)";
                        break;
                    case 2:
                        stringname = "最大速度 [PGV] (cm/s)";
                        break;
                    case 3:
                        stringname = "最大変位 [PGD] (cm)";
                        break;
                    case 4:
                        stringname = $"応答速度({(SettingWindow.Check2Mode ? "地中" : "地表")}) [PGV] (cm/s)";
                        break;

                }
                stringname += $" {(SettingWindow.MaxValueMode?"観測最大地点":$"最寄り地点")}";
                g.DrawString(stringname, font1, Brushes.White, rect1, stringFormat);
                for (int i = 0; i < tasks.Count; i++)
                {
                    var near = tasks[i].Result.NearPoint;
                    if (SettingWindow.MaxValueMode) near = tasks[i].Result.MaxPoint;
                    rect1 = new RectangleF(0, space + (h * (float)WindowTopBottom / tasks.Count) + i * sth + (i >= 6 ? space : 0), 40, sth);
                    stringFormat = new StringFormat()
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString($"{tasks[i].Result.Graph.ShortTitle}:", font1, Brushes.White, rect1, stringFormat);
                    rect1 = new RectangleF(40, space + (h * (float)WindowTopBottom / tasks.Count) + i * sth + (i >= 6 ? space : 0), 50, sth);
                    stringFormat = new()
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString($"{near.Value.ToString(tasks[i].Result.Graph.Format)}", font1, Brushes.White, rect1, stringFormat);
                    if (tasks.Count > 6)
                    {
                        stringFormat = new StringFormat()
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Far
                        };
                        rect1 = new RectangleF(0, space + (h * (float)WindowTopBottom / tasks.Count) + 6 * sth, w, space); 
                        stringname = $"応答速度({(SettingWindow.Check2Mode ? "地表" : "地中")}) [PGV] (cm/s)";
                        stringname += $" {(SettingWindow.MaxValueMode ? "観測最大地点" : $"最寄り地点")}";
                        g.DrawString(stringname, font1, Brushes.White, rect1, stringFormat);

                    }
                    double val = near.RawValue;
                    if (val < tasks[i].Result.Graph.ColorOffset) val = 0;
                    else val = (val - tasks[i].Result.Graph.ColorOffset) / (1.0 - tasks[i].Result.Graph.ColorOffset);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(16, 16, 16)), new RectangleF(40 + 50, space+(h * (float)WindowTopBottom / tasks.Count) + i * sth + (i >= 6 ? space : 0) + 1, bitmap.Width - (40 + 50), sth - 2));
                    g.FillRectangle(new SolidBrush(near.PointColor), new RectangleF(40 + 50, space+(h * (float)WindowTopBottom / tasks.Count) + i * sth + (i >= 6 ? space : 0) + 1, (float)(val * (bitmap.Width - (40 + 50))), sth - 2));
                }
                g.Dispose();
                return bitmap;
            });
            var old = pictureBox1.Image;
            pictureBox1.Image=create;
            if(old!=null)old.Dispose();
            sw.Stop();
        }

        private void KyoshinResponseGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin -= UpdateImage;
        }

        private void CreateWindowMenu_Click(object sender, EventArgs e)
        {

        }
        Setting SettingWindow = new();
        private void OpenSetting_Click(object sender, EventArgs e)
        {
            SettingWindow.ShowDialog();
        }
    }
}
