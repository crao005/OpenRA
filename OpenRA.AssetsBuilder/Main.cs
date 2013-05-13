using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRA.AssetsBuilder
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private YamlHelper ym;
        private void Main_Load(object sender, EventArgs e)
        {
            cBXType.SelectedIndex = 0;
            ym = new YamlHelper();

            lBoxItems.DataSource = ym.getSequences();            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewAsset fm = new NewAsset();
            fm.Show();
        }

        private void cBXType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cBXType.SelectedItem.ToString())
            {
                case "Ships":
                    lBoxItems.DataSource = ym.getShips();                    
                    break;
                case "Vehicles":
                    lBoxItems.DataSource = ym.getVehicles();                   
                    break;
                default:
                    break;
            }

        }
    }
}
