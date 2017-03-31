using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExternalEditor
{
    public partial class Editor : Form
    {
        private int tileSize;
        private int xTiles, yTiles;
        private int xMin, xMax;
        private int yMin, yMax;
        private bool spawnPlaced;
        private int[,] tiles;
        private List<Button> tileButtons;
        private StreamWriter sw;
        private MouseState ms;
        bool mouse_down = false;

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
                currentLevelNameTextbox.Text = "NewCollision";
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

        private void saveLevelButton_Click(object sender, EventArgs e)
        {
            ExportGridToFile();
        }

        private void openLevelCollisionButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "cmap files (*.cmap)|*.cmap";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                string filename = dialog.FileName;
                string[] filelines = File.ReadAllLines(filename);
                InitializeGridFromFile(filelines);
                state = ProgramState.editing;
                UpdateStateVisuals();
            }
        }

        public void TileDragged(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            Point p = new Point(b.Location.X + me.Location.X, b.Location.Y + me.Location.Y);
            if (mouse_down)
            {
                foreach (var item in tileButtons)
                {
                    if (item.Bounds.Contains(p))
                    {
                        Debug.WriteLine("Mouse is in bounds.");
                        switch (currentTool)
                        {
                            case (Tool.delete):
                                if (item.BackColor == Color.LightGreen)
                                {
                                    spawnPlaced = false;
                                }
                                item.BackColor = SystemColors.ButtonHighlight;
                                break;

                            case (Tool.enemy):
                                item.BackColor = Color.OrangeRed;
                                break;

                            case (Tool.spawn):
                                if (!spawnPlaced)
                                {
                                    item.BackColor = Color.LightGreen;
                                    spawnPlaced = true;
                                }
                                break;

                            case (Tool.wall):
                                item.BackColor = SystemColors.MenuHighlight;
                                break;
                        }
                    }
                }
            }
        }

        public void TileClicked(object sender, EventArgs e)
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

        private void Editor_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_down = true;
        }

        private void Editor_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
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
                    b.ForeColor = Color.Gray;
                    b.BackColorChanged += (s, e) => {b.FlatAppearance.MouseOverBackColor = b.BackColor;};
                    b.Location = new Point(i + gridX, j + gridY);
                    tileButtons.Add(b);
                    this.Controls.Add(b);
                    b.MouseClick += (s, e) => { mouse_down = true; TileClicked(s, e); };
                    b.MouseUp += (s, e) => { mouse_down = false; };
                    b.MouseDown += (s, e) => { mouse_down = true; };
                    b.MouseMove += (s, e) => { TileDragged(s, e); };
                    if (loadingBar.Value < loadingBar.Maximum)
                        loadingBar.Value++;
                }
            }
        }

        public void InitializeGridFromFile(string[] read)
        {
            this.Width = (this.Width + (tileSize * read[0].Length) + 12);
            int gridX = 195;
            int gridY = 17;
            for (int i = 0; i < read.Length; i++)
            {
                for(int j = 0; j < read[0].Length; j++)
                {
                    Button b = new Button();
                    b.Text = "";
                    switch(read[i][j])
                    {
                        case '0':
                            b.BackColor = SystemColors.ButtonHighlight;
                            break;

                        case '1':
                            b.BackColor = SystemColors.MenuHighlight;
                            break;

                        case '2':
                            b.BackColor = Color.OrangeRed;
                            break;

                        case '3':
                            b.BackColor = Color.LightGreen;
                            break;
                    }
                    b.ForeColor = SystemColors.ButtonHighlight;
                    b.Size = new Size(tileSize, tileSize);
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.MouseOverBackColor = b.BackColor;
                    b.BackColorChanged += (s, e) => { b.FlatAppearance.MouseOverBackColor = b.BackColor; };
                    b.Location = new Point(gridX,gridY);
                    tileButtons.Add(b);
                    this.Controls.Add(b);
                    b.MouseEnter += new EventHandler(this.TileClicked);
                    if (loadingBar.Value < loadingBar.Maximum)
                        loadingBar.Value++;
                    gridX += tileSize;
                }
                gridX = 195;
                gridY += tileSize;
            }
        }
 
        public void ExportGridToFile()
        {
            var index = 0;
            for(int i = 0; i < tiles.GetLength(0); i++)
            {
                for(int j = 0; j < tiles.GetLength(1); j++)
                {
                    BackColor = tileButtons[index].BackColor;
                    if(BackColor == SystemColors.MenuHighlight) 
                    {
                        tiles[i, j] = 1;
                    }
                    else if (BackColor == Color.OrangeRed)
                    {
                        tiles[i, j] = 2;
                    }
                    else if (BackColor == Color.LightGreen)
                    {
                        tiles[i, j] = 3;
                    }
                    else
                    {
                        tiles[i, j] = 0;
                    }
                    index++;
                }
            }
            try
            {
                this.BackColor = Form.DefaultBackColor;
                sw = new StreamWriter(currentLevelNameTextbox.Text + ".cmap");
                for(int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        sw.Write(tiles[j,i]);
                    }
                    sw.Write("\n");
                }
                System.Windows.Forms.MessageBox.Show("New Collision map saved as " + currentLevelNameTextbox.Text + ".cmap.",
                                 "Success!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                this.BackColor = Form.DefaultBackColor;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with file write: " + e.Message);
                System.Windows.Forms.MessageBox.Show("Error with file write: " + e.Message,
                                "Error!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                this.BackColor = Form.DefaultBackColor;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }       
        }
    }
}
