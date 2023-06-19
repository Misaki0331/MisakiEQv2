using System.Text;
using System.Windows.Media;

namespace MisakiEQ.src.GUI
{
    public partial class Map : Form
    {
        string MapStream;
        public Map()
        {
            InitializeComponent();
            string tempFileName = Path.GetTempFileName();
            using BinaryWriter sw = new(File.OpenWrite(tempFileName));
            sw.Write(Encoding.UTF8.GetBytes(Resources.Map.MapResource.Japan));
            sw.Close();
            geoMap1.Source = tempFileName;

            geoMap1.LandStrokeThickness = 0.6;
            geoMap1.LandStroke = System.Windows.Media.Brushes.Gray;
            geoMap1.Parent = this;
            geoMap1.DefaultLandFill = (System.Windows.Media.Brush?)new System.Windows.Media.BrushConverter().ConvertFromString("#FF999999"); //new System.Windows.Media.SolidBrush(Color.FromArgb(255, 151, 151, 151));
            geoMap1.LandStroke = (System.Windows.Media.Brush?)new System.Windows.Media.BrushConverter().ConvertFromString("#FF606060");
            MapStream = tempFileName;
            geoMap1.DisableAnimations = true;
            geoMap1.EnableZoomingAndPanning = true;
            geoMap1.Margin = new Padding(4, 5, 4, 5);
            geoMap1.Name = "geoMap1";
            geoMap1.TabIndex = 0;
            timer1.Enabled = true;
            geoMap1.Base.Background = (System.Windows.Media.Brush?)new System.Windows.Media.BrushConverter().ConvertFromString("#FF515A91");
            Index.Text = "東北 関東 新潟";
            Description.Text = "福島県で地震 強い揺れに警戒";
            this.ShowInTaskbar = false;
            this.Text = "緊急地震速報";
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Map_FormClosed(object sender, FormClosedEventArgs e)
        {

            File.Delete(MapStream);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           // geoMap1.LandStrokeThickness = (double)numericUpDown1.Value;
        }

        bool isf = false;
        private void Map_Timer(object sender, EventArgs e)
        {
            if (!isf)
            {
                this.Location = new(Location.X, Location.Y + 270);
                isf = true;
            }
            Dictionary<string, double> values = new Dictionary<string, double>();

            // 4. Fill the specific keys of the countries with a random number
            //values["0"] = 0;
            values["1000000"] = 100;
            values["1000001"] = 0;
            for (int i = 0; i < 60; i++) values[$"{i}"] = 0;
            values[$"{27}"] = 100;
            for (int i = 14; i < 22; i++) values[$"{i}"] = 100;
            for (int i = 22; i < 25; i++) values[$"{i}"] = DateTime.Now.Millisecond % 1000 < 500 ? 100 : 0;
            //Log.Debug($"Test {values["1"]}");
            // 5. Assign data and map file
            geoMap1.HeatMap = values;
            geoMap1.GradientStopCollection = new GradientStopCollection
                {
                    new GradientStop(Colors.DarkGray, 0),
                    new GradientStop(Colors.Green, 0.5),
                    new GradientStop(Colors.Yellow, 1),
                };
        }
    }
}
