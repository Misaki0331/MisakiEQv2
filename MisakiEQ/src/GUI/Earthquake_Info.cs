using MisakiEQ.Struct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MisakiEQ.GUI
{
    public partial class Earthquake_Info : Form
    {
        public Earthquake_Info()
        {
            InitializeComponent();
        }
        public void UpdateInfomation(object sender, EarthQuake e)
        {
            DetailOriginTime.Text = $"{e.Details.OriginTime}"; //発生時刻 ----/--/-- --:--
            DetailInfo.Text = EarthQuake.TypeToString(e.Issue.Type); //地震の情報
            _ = e.Details.Hypocenter; //震源地
            _ = e.Details.Depth; //震源の深さ
            _ = e.Details.Magnitude; //地震の規模
            _ = e.Details.MaxIntensity; //最大震度
            _ = e.Details.ForeignTsunami; //津波情報
        }
    }
}
