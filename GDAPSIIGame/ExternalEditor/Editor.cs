using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace ExternalEditor
{
    public partial class Editor : Form
    {
        //Initialize the class components.
        private int tileSize;
        private int xTiles, yTiles;
        private int xMin, xMax;
        private int yMin, yMax;
        private bool spawnPlaced;
        private int[,] tiles;
        private List<Button> tileButtons;
        private StreamWriter sw;
        private MouseState ms;

        //Enums for the program state, to enable and disable buttons accordingly.
        enum ProgramState
        {
            startup,
            editing
        }

        //Enums for the current tool the user is on.
        enum Tool
        {
            wall,
            delete,
            spawn,
            enemy
        }

        ProgramState state;
        Tool currentTool;

        //Initialize the windows forms and its components.
        public Editor()
        {
            InitializeComponent();
            UpdateStateVisuals();
            tileButtons = new List<Button>();
            ms = Mouse.GetState();
            state = ProgramState.startup;
            currentTool = Tool.wall;
            spawnPlaced = false;
            tileSize = 20;
            xMin = 10;
            yMin = 10;
            xMax = 40;
            yMax = 25;
        }

        //Set the tool variable to the according tool when that button is pressed.
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

        //Changes the button visuals based on the program state of the form.
        public void UpdateStateVisuals()
        {
            switch(state)
            {
                case (ProgramState.startup):
                    CurrentToolLabel.ForeColor = SystemColors.MenuHighlight;
                    newLevelCollisionButton.Enabled = true;
                    currentLevelNameTextbox.Enabled = false;
                    wallToolButton.Enabled = false;
                    deleteToolButton.Enabled = false;
                    spawnToolButton.Enabled = false;
                    enemyToolButton.Enabled = false;
                    loadingBar.Enabled = false;
                    break;

                case (ProgramState.editing):
                    newLevelCollisionButton.Enabled = false;
                    currentLevelNameTextbox.Enabled = true;
                    wallToolButton.Enabled = true;
                    deleteToolButton.Enabled = true;
                    spawnToolButton.Enabled = true;
                    enemyToolButton.Enabled = true;
                    loadingBar.Enabled = true;
                    break;
            }
        }

        //Initializes a button grid based on the x and y input of the user. Also initializes some variables.
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
                tiles = new int[xTiles , yTiles];
                InitializeGrid(xTiles, yTiles);
                state = ProgramState.editing;
                UpdateStateVisuals();
                this.Width = (this.Width + (tileSize * xTiles) + 12);
                loadingBar.Maximum = xTiles * yTiles;
                InitializeGrid(xTiles, yTiles);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("X values must be positive integers with a value between " + xMin + " and " +xMax + ", " + 
                                "Y values must be positive integers with a value between " + yMin + " and " + yMax + ".",
                                "Error!",
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
            }
        }

        //Preforms the export function when the save button is clicked. 
        private void saveLevelButton_Click(object sender, EventArgs e)
        {
            ExportGridToFile();
        }

        //Action for when a tile is clicked, for hovered upon. Method is called every time the mouse hovers over a tile.
        public void TileClicked(object sender, EventArgs e)
        {
            ms = Mouse.GetState();
            if (state == ProgramState.editing)
            {
                //if() mouse - press
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

        //Initializes the grid visuals for the tile editor - a list set of buttons acting as tiles.
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
                    if (loadingBar.Value < loadingBar.Maximum)
                        loadingBar.Value++;
                }
            }
        }

        //0 = nothing (floor)
        //1 = wall
        //2 = enemy
        //3 = spawn

        //Exports the grid to the tile 2D array, and exports that 2D array to a .cmap file in the program directory.
        public void ExportGridToFile()
        {
            var index = 0;
            for(int i = 0; i < tiles.GetLength(0); i++)
            {
                for(int j = 0; j < tiles.GetLength(1); j++)
                {
                    BackColor = tileButtons[index].BackColor;
                    //If the tile is set to the wall color, set the grid info at [i][j] to a wall.
                    if(BackColor == SystemColors.MenuHighlight) 
                    {
                        tiles[i, j] = 1;
                    }
                    //If the tile is set to the enemy color, set the grid info at [i][j] to an enemy.
                    else if (BackColor == Color.OrangeRed)
                    {
                        tiles[i, j] = 2;
                    }
                    //If the tile is set to the spawn color, set the grid info at [i][j] to a spawn.
                    else if (BackColor == Color.LightGreen)
                    {
                        tiles[i, j] = 3;
                    }
                    //If the tile back is blank, set the grid info at [i][j] to 0.
                    else
                    {
                        tiles[i, j] = 0;
                    }
                    index++;
                }
            }

            try
            {
                sw = new StreamWriter(currentLevelNameTextbox.Text + ".cmap");
                for(int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        sw.Write(tiles[i, j]);
                    }
                    sw.Write("\n");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with file write: " + e.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }

            System.Windows.Forms.MessageBox.Show("New Collision map saved as " + currentLevelNameTextbox.Text + ".cmap.",
                                 "Success!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
            this.BackColor = Form.DefaultBackColor;
        }
    }
}
