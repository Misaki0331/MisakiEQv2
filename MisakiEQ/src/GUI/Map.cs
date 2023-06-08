using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            geoMap1.EnableZoomingAndPanning = true;
        }

        private void Map_FormClosed(object sender, FormClosedEventArgs e)
        {

            File.Delete(MapStream);
        }
    }
}
