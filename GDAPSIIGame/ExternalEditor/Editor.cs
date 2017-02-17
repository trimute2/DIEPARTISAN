using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalEditor
{
    public partial class Editor : Form
    {
        private string currentTool;
        private int xTiles, yTiles;
        private int xMin, xMax;
        private int yMin, yMax;
        private int[] tiles;

        public Editor()
        {
            InitializeComponent();

            currentLevelNameTextbox.Enabled = false;
            currentLevelXTilesBox.Enabled = false;
            currentLevelYTilesBox.Enabled = false;
            wallToolButton.Enabled = false;
            deleteToolButton.Enabled = false;
            spawnToolButton.Enabled = false;
            enemyToolButton.Enabled = false;
            currentTool = "Wall";
            xMin = 10;
            yMin = 10;
            xMax = 40;
            yMax = 25;
        }

        private void newLevelCollisionButton_Click(object sender, EventArgs e)
        {
            if(int.TryParse(openLevelXTilesTextbox.Text, out xTiles) &&
               int.TryParse(openLevelYTilesTextbox.Text, out yTiles) &&

               int.Parse(openLevelXTilesTextbox.Text) <= xMax &&
               int.Parse(openLevelXTilesTextbox.Text) >= xMin &&
               int.Parse(openLevelYTilesTextbox.Text) <= yMax &&
               int.Parse(openLevelYTilesTextbox.Text) >= yMin)
            {
                xTiles = int.Parse(openLevelXTilesTextbox.Text);
                yTiles = int.Parse(openLevelYTilesTextbox.Text);
                tiles = new int[xTiles * yTiles];
            }
            else
            {
                MessageBox.Show("Error! X values must be positive integers with a value between " + xMin + " and " +xMax + ", " + 
                                "Y values must be positive integers with a value between " + yMin + " and " + yMax + ".",
                                "Error!",
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
            }
        }

        public void InitializeButtons()
        {
            
        }
    }
}
