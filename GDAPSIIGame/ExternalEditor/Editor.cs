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
        private int tileSize;
        private int xTiles, yTiles;
        private int xMin, xMax;
        private int yMin, yMax;
        private int[] tiles;
        private Button[] tileButtons;

        enum ProgramState
        {
            startup,
            editing
        }

        enum Tool
        {
            wall,
            delete,
            spawn,
            enemy
        }

        ProgramState state;
        Tool currentTool;

        public Editor()
        {
            InitializeComponent();
            UpdateStateVisuals();
            state = ProgramState.startup;
            currentTool = Tool.wall;
            tileSize = 10;
            xMin = 10;
            yMin = 10;
            xMax = 40;
            yMax = 25;
        }

        private void wallToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.wall;
        }

        private void deleteToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.delete;
        }

        private void spawnToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.spawn;
        }

        private void enemyToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.enemy;
        }

        private void currentLevelXTilesBox_SelectedItemChanged(object sender, EventArgs e)
        {
            DomainUpDown domain = (DomainUpDown)sender;
            if(domain.UpButton == DomainUpDown.)
        }

        public void UpdateStateVisuals()
        {
            switch(state)
            {
                case (ProgramState.startup):
                    newLevelCollisionButton.Enabled = true;
                    currentLevelNameTextbox.Enabled = false;
                    currentLevelXTilesBox.Enabled = false;
                    currentLevelYTilesBox.Enabled = false;
                    wallToolButton.Enabled = false;
                    deleteToolButton.Enabled = false;
                    spawnToolButton.Enabled = false;
                    enemyToolButton.Enabled = false;
                    break;

                case (ProgramState.editing):
                    newLevelCollisionButton.Enabled = false;
                    currentLevelNameTextbox.Enabled = true;
                    currentLevelXTilesBox.Enabled = true;
                    currentLevelYTilesBox.Enabled = true;
                    wallToolButton.Enabled = true;
                    deleteToolButton.Enabled = true;
                    spawnToolButton.Enabled = true;
                    enemyToolButton.Enabled = true;
                    break;
            }
        }

        private void newLevelCollisionButton_Click(object sender, EventArgs e)
        {
            if(int.TryParse(newLevelXTilesTextbox.Text, out xTiles) &&
               int.TryParse(newLevelYTilesTextbox.Text, out yTiles) &&
               int.Parse(newLevelXTilesTextbox.Text) <= xMax &&
               int.Parse(newLevelXTilesTextbox.Text) >= xMin &&
               int.Parse(newLevelYTilesTextbox.Text) <= yMax &&
               int.Parse(newLevelYTilesTextbox.Text) >= yMin)
            {
                xTiles = int.Parse(newLevelXTilesTextbox.Text);
                yTiles = int.Parse(newLevelYTilesTextbox.Text);
                tiles = new int[xTiles * yTiles];
                InitializeGrid(xTiles, yTiles);
                state = ProgramState.editing;
                UpdateStateVisuals();
                this.Width = (this.Width + (tileSize * xTiles) + 25);
                currentLevelXTilesBox.Text = xTiles + "";
                currentLevelYTilesBox.Text = yTiles + "";
            }
            else
            {
                MessageBox.Show("X values must be positive integers with a value between " + xMin + " and " +xMax + ", " + 
                                "Y values must be positive integers with a value between " + yMin + " and " + yMax + ".",
                                "Error!",
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
            }
        }

        public void InitializeGrid(int width, int height)
        {
            
        }
    }
}
