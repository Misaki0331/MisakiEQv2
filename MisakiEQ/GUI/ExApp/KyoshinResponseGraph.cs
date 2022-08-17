using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI.ExApp
{
    public partial class KyoshinResponseGraph : Form
    {
        public KyoshinResponseGraph()
        {
            InitializeComponent();
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin += UpdateImage;
            Icon = Properties.Resources.Logo_MainIcon;
        }
        private async void UpdateImage(object? sender, EventArgs args)
        {
            Stopwatch sw = new();
            sw.Start();
            List<Task<Lib.KyoshinAPI.KyoshinObervation.AnalysisResult>> tasks = new()
            {
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0125_s),
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0250_s),
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp0500_s),
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp1000_s),
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp2000_s),
                Lib.KyoshinAPI.KyoshinObervation.GetPoints(Background.API.KyoshinAPI.KyoshinAPI.KyoshinImageType.rsp4000_s)
            };
            await Task.WhenAll(tasks);
            Bitmap bitmap = new(Width, Height);
            var g=Graphics.FromImage(bitmap);
            g.FillRectangle(Brushes.Black, 0, 0, Width, Height);
            g.DrawString($"0.125Hz:{tasks[0].Result.NearPoint.String}\n" +
                $"0.250Hz:{tasks[1].Result.NearPoint.String}\n" +
                $"0.500Hz:{tasks[2].Result.NearPoint.String}\n" +
                $"1.000Hz:{tasks[3].Result.NearPoint.String}\n" +
                $"2.000Hz:{tasks[4].Result.NearPoint.String}\n" +
                $"4.000Hz:{tasks[5].Result.NearPoint.String}\n" , 
                new("Arial", 9), Brushes.White, new PointF(0, 0));
            g.Dispose();
            var old = pictureBox1.Image;
            pictureBox1.Image=bitmap;
            if(old!=null)old.Dispose();
            sw.Stop();
            //Log.Instance.Debug($"全ての解析完了:{sw.Elapsed}");
        }

        private void KyoshinResponseGraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            Background.APIs.GetInstance().KyoshinAPI.UpdatedKyoshin -= UpdateImage;
        }
    }
}
