using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace ExternalEditor
{
    public partial class Editor : Form
    {
        private int tileSize;
        private int xTiles, yTiles;
        private int xMin, xMax;
        private int yMin, yMax;
        private bool spawnPlaced;
        private int[] tiles;
        private List<Button> tileButtons;
        private MouseEventArgs mouse;

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
            tileButtons = new List<Button>();
            state = ProgramState.startup;
            currentTool = Tool.wall;
            spawnPlaced = false;
            mouse = new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0);
            tileSize = 20;
            xMin = 10;
            yMin = 10;
            xMax = 40;
            yMax = 25;
        }

        private void wallToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.wall;
            CurrentToolLabel.Text = "Current Tool: Wall";
            CurrentToolLabel.ForeColor = SystemColors.MenuHighlight;
        }

        private void deleteToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.delete;
            CurrentToolLabel.Text = "Current Tool: Delete";
            CurrentToolLabel.ForeColor = Color.Red;
        }

        private void spawnToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.spawn;
            CurrentToolLabel.Text = "Current Tool: Spawn";
            CurrentToolLabel.ForeColor = Color.LightGreen;
        }

        private void enemyToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.enemy;
            CurrentToolLabel.Text = "Current Tool: Enemy";
            CurrentToolLabel.ForeColor = Color.OrangeRed;
        }

        private void currentLevelXTilesBox_SelectedItemChanged(object sender, EventArgs e)
        {
            
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
                this.Width = (this.Width + (tileSize * xTiles) + 12);
                currentLevelXTilesBox.Text = xTiles + "";
                currentLevelYTilesBox.Text = yTiles + "";
                InitializeGrid(xTiles, yTiles);
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

        public void TileClicked(object sender, EventArgs e)
        {
            if (mouse.Button ==  MouseButtons.Left)
            {
                Button b = (Button)sender;
                switch (currentTool)
                {
                    case (Tool.delete):
                        if (b.BackColor == Color.LightGreen)
                        {
                            spawnPlaced = false;
                        }
                        b.BackColor = SystemColors.ButtonHighlight;
                        break;

                    case (Tool.enemy):
                        b.BackColor = Color.OrangeRed;
                        break;

                    case (Tool.spawn):
                        if (!spawnPlaced)
                        {
                            b.BackColor = Color.LightGreen;
                            spawnPlaced = true;
                        }
                        break;

                    case (Tool.wall):
                        b.BackColor = SystemColors.MenuHighlight;
                        break;
                }
            }
        }

        public void InitializeGrid(int width, int height)
        {
            int gridX = 195;
            int gridY = 17;
            for(int i = 0; i < height * tileSize; i += tileSize)
            {
                for(int j = 0; j < width * tileSize; j += tileSize)
                {
                    Button b = new Button();
                    b.Text = "";
                    b.BackColor = SystemColors.ButtonHighlight;
                    b.ForeColor = SystemColors.ButtonHighlight;
                    b.Size = new Size(tileSize, tileSize);
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.MouseOverBackColor = b.BackColor;
                    b.BackColorChanged += (s, e) => {b.FlatAppearance.MouseOverBackColor = b.BackColor;};
                    b.Location = new Point(i + gridX, j + gridY);
                    tileButtons.Add(b);
                    this.Controls.Add(b);
                    b.MouseEnter += new EventHandler(this.TileClicked);
                }
            }
        }
    }
}
