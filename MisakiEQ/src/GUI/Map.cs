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
            MapStream = tempFileName;
            geoMap1.DisableAnimations = true;
            geoMap1.EnableZoomingAndPanning = true;
            timer1.Enabled = true;
        }

        private void Map_FormClosed(object sender, FormClosedEventArgs e)
        {

            File.Delete(MapStream);
        }
        private void Map_Timer(object sender, EventArgs e)
        {

            Dictionary<string, double> values = new Dictionary<string, double>();

            // 4. Fill the specific keys of the countries with a random number
            values["0"] = 0;
            values["1000000"] = 100;
            values["866"] = DateTime.Now.Millisecond%1000 < 500 ? 100 : 0;
            Log.Debug($"Test {values["866"]}");
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
