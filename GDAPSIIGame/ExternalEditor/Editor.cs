using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExternalEditor
{
    /// <summary>
    /// Partial class for the editor tool. Handles all program arithmetic.
    /// </summary>
    public partial class Editor : Form
    {
        private int tileSize;               //How large the tiles are
        private int xTiles, yTiles;         //How many X and Y tiles are in the current scene
        private int xMin, xMax;             //Minimum scene tiles for the X direction.
        private int yMin, yMax;             //Minimum scene tiles for the Y direction.
        private bool spawnPlaced;           //Determines whether a spawn tile has been placed yet.
        private int[,] tiles;               //Integer array of tiles on the screen.
        private List<Button> tileButtons;   //List of buttons, being shown as tiles.
        private StreamWriter sw;            //Stream Writer for file opening.
        private MouseState ms;              //State of the mouse for clicking actions.
        private bool mouseDown;             //Determines globally if the mouse is down.
        private bool newOpen;

        private string wallTileText = "";   //Text overwrite for the wall tile.
        private string enemyTileText = "";  //Text overwrite for the enemy tile.
        private string spawnTileText = "";  //Text overwrite for the spawn tile.

        /// <summary>
        /// The states of the program, to allow/disallow button actions.
        /// </summary>

        enum ProgramState
        {
            startup,
            editing
        }

        /// <summary>
        /// Tool enumeration for the different button options in the left column.
        /// </summary>

        enum Tool
        {
            wall,
            delete,
            spawn,
            enemy
        }

        ProgramState state;
        Tool currentTool;

        /// <summary>
        /// Initialize a new Editor window.
        /// </summary>

        public Editor()
        {
            InitializeComponent();
            UpdateStateVisuals();
            tileButtons = new List<Button>();
            ms = Mouse.GetState();
            state = ProgramState.startup;
            this.timer.Interval = 20;
            currentTool = Tool.wall;
            spawnPlaced = false;
            mouseDown = false;
            tileSize = 20;
            xMin = 10;
            yMin = 10;
            xMax = 40;
            yMax = 25;
			enemyDropDownBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Click method the the wall tool button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void wallToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.wall;
            CurrentToolLabel.Text = toolStripTextBox.Text = "Changed Tool: Wall";
            CurrentToolLabel.ForeColor = SystemColors.MenuHighlight;
			enemyDropDownBox.Enabled = false;
        }

        /// <summary>
        /// CLick method for the delete tool button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void deleteToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.delete;
            CurrentToolLabel.Text = toolStripTextBox.Text = "Changed Tool: Eraser";
            CurrentToolLabel.ForeColor = Color.Red;
			enemyDropDownBox.Enabled = false;
		}

        /// <summary>
        /// Click method for the spawn tool button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void spawnToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.spawn;
            CurrentToolLabel.Text = toolStripTextBox.Text = "Changed Tool: Spawn";
            CurrentToolLabel.ForeColor = Color.LightGreen;
			enemyDropDownBox.Enabled = false;
		}

        /// <summary>
        /// Click button for the enemy tool button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void enemyToolButton_Click(object sender, EventArgs e)
        {
            currentTool = Tool.enemy;
            CurrentToolLabel.Text = toolStripTextBox.Text = "Changed Tool: Enemy";
            CurrentToolLabel.ForeColor = Color.OrangeRed;
			enemyDropDownBox.Enabled = true;
		}

        /// <summary>
        /// Update the button access of the program based on the program state.
        /// </summary>

        public void UpdateStateVisuals()
        {
            switch(state)
            {
                case (ProgramState.startup):
                    CurrentToolLabel.ForeColor = SystemColors.MenuHighlight;
                    //Sets "New Level" and "Open Level" buttons to true.
                    newLevelCollisionButton.Enabled = true;
                    openLevelCollisionButton.Enabled = true;
                    //Sets all other enabled values to false while a new project is being created.
                    currentLevelNameTextbox.Enabled = false;
                    wallToolButton.Enabled = false;
                    deleteToolButton.Enabled = false;
                    spawnToolButton.Enabled = false;
                    enemyToolButton.Enabled = false;
                    loadingBar.Enabled = false;
                    clearLevelButton.Enabled = false;
                    saveLevelButton.Enabled = false;
                    currentLevelNameTextbox.Enabled = false;
                    CurrentToolLabel.Text = "";
                    break;

                case (ProgramState.editing):
                    //Sets all button enabled values to true while editing.
                    timer.Enabled = true;
                    newLevelCollisionButton.Enabled = true;
                    currentLevelNameTextbox.Enabled = true;
                    wallToolButton.Enabled = true;
                    deleteToolButton.Enabled = true;
                    spawnToolButton.Enabled = true;
                    enemyToolButton.Enabled = true;
                    loadingBar.Enabled = true;
                    clearLevelButton.Enabled = true;
                    currentLevelNameTextbox.Enabled = true;
                    saveLevelButton.Enabled = true;
                    CurrentToolLabel.Text = "Current Tool: Wall";
                    break;
            }
        }

        /// <summary>
        /// Creates a new matrix of buttons for tile editing, and expands the screen accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void newLevelCollisionButton_Click(object sender, EventArgs e)
        {
            //Try parsing the string values of the textboxes, and seeing if they're in bounds.
            if(int.TryParse(newLevelXTilesTextbox.Text, out xTiles) &&
               int.TryParse(newLevelYTilesTextbox.Text, out yTiles) &&
               int.Parse(newLevelXTilesTextbox.Text) <= xMax &&
               int.Parse(newLevelXTilesTextbox.Text) >= xMin &&
               int.Parse(newLevelYTilesTextbox.Text) <= yMax &&
               int.Parse(newLevelYTilesTextbox.Text) >= yMin)
            {
                //Set up new tile map.
                xTiles = int.Parse(newLevelXTilesTextbox.Text);
                yTiles = int.Parse(newLevelYTilesTextbox.Text);
                tiles = new int[xTiles , yTiles];
                InitializeGrid(xTiles, yTiles);
                state = ProgramState.editing;
                UpdateStateVisuals();
                this.Width = (this.Width + (tileSize * xTiles)) / 2 + 15;
                loadingBar.Maximum = xTiles * yTiles;
                InitializeGrid(xTiles, yTiles);
                currentLevelNameTextbox.Text = "NewCollision";
                timer.Start();
            }
            else
            {
                //If the values were not an integer or were our of bounds, show an error message box.
                System.Windows.Forms.MessageBox.Show("X values must be positive integers with a value between " + xMin + " and " +xMax + ", " + 
                                "Y values must be positive integers with a value between " + yMin + " and " + yMax + ".",
                                "Error!",
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Calls the export method to save the matrix as a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void saveLevelButton_Click(object sender, EventArgs e)
        {
            //May have other functions later in development. Right now it's only a method pointer.
            ExportGridToFile();
        }

        /// <summary>
        /// Opens an explorer window to open a .cmap file for editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

        /// <summary>
        /// Fills in tiles in the matrix accordingly when the mouse is dragging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void TileDragged(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            Point p = new Point(b.Location.X + me.Location.X, b.Location.Y + me.Location.Y);
            if (mouseDown)
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
                                item.Text = "";
                                break;

                            case (Tool.enemy):
                                item.BackColor = Color.OrangeRed;
                                item.Text = enemyTileText;
                                break;

                            case (Tool.spawn):
                                if (!spawnPlaced)
                                {
                                    item.BackColor = Color.LightGreen;
                                    item.Text = spawnTileText;
                                    spawnPlaced = true;
                                }
                                break;

                            case (Tool.wall):
                                item.BackColor = SystemColors.MenuHighlight;
                                item.Text = wallTileText;
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills in a tile when it is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
                    b.Text = "";
                    break;

                case (Tool.enemy):
					switch(enemyDropDownBox.SelectedIndex)
					{
						case 0:
							b.BackColor = Color.OrangeRed;
							break;
						case 1:
							b.BackColor = Color.DarkOrange;
							break;
					}
                    b.Text = enemyTileText;
                    break;

                case (Tool.spawn):
                    if (!spawnPlaced)
                    {
                        b.BackColor = Color.LightGreen;
                        b.Text = spawnTileText;
                        spawnPlaced = true;
                    }
                    break;

                case (Tool.wall):
                    b.BackColor = SystemColors.MenuHighlight;
                    b.Text = wallTileText;
                    break;
            }
        }

        /// <summary>
        /// Clears the level if the user selects the option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void clearLevelButton_Click(object sender, EventArgs e)
        {
            DialogResult delete = System.Windows.Forms.MessageBox.Show("Are you sure you want to clear the current level? No changes will be saved.", "Warning!", MessageBoxButtons.YesNo);
            if (delete == DialogResult.Yes)
            {
                for (int i = 0; i < tileButtons.Count; i++)
                {
                    tileButtons[i].BackColor = SystemColors.ButtonHighlight;
                }
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            
        }

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Initialized the buttons for the matrix of tiles, and sets events accordingly.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>

		public void InitializeGrid(int width, int height)
        {
            int gridX = 195;
            int gridY = 17;
            for(int i = 0; i < width * tileSize; i += tileSize)
            {
                for(int j = 0; j < height * tileSize; j += tileSize)
                {
                    Button b = new Button();
                    b.Text = "";
                    b.BackColor = SystemColors.ButtonHighlight;
                    b.ForeColor = Color.LightGray;
                    b.Size = new Size(tileSize, tileSize);
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.MouseOverBackColor = b.BackColor;
                    b.BackColorChanged += (s, e) => {b.FlatAppearance.MouseOverBackColor = b.BackColor;};
                    b.Location = new Point(i + gridX, j + gridY);
                    tileButtons.Add(b);
                    this.Controls.Add(b);
                    b.MouseClick += (s, e) => { mouseDown = true; TileClicked(s, e); };
                    b.MouseUp += (s, e) => { mouseDown = false; };
                    b.MouseDown += (s, e) => { mouseDown = true; };

                    b.MouseLeave += (s, e) => { this.Cursor = Cursors.Arrow; };
                    b.MouseEnter += (s, e) => { this.Cursor = Cursors.Hand; };

                    b.MouseMove += (s, e) => { TileDragged(s, e); };
                    if (loadingBar.Value < loadingBar.Maximum)
                        loadingBar.Value++;
                }
            }
        }

        /// <summary>
        /// Reads a collision matrix from a .cmap file, and creates a tile matrix from it.
        /// </summary>
        /// <param name="read"></param>

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
                    b.MouseClick += (s, e) => { mouseDown = true; TileClicked(s, e); };
                    b.MouseUp += (s, e) => { mouseDown = false; };
                    b.MouseDown += (s, e) => { mouseDown = true; };
                    b.MouseMove += (s, e) => { TileDragged(s, e); };
                    if (loadingBar.Value < loadingBar.Maximum)
                        loadingBar.Value++;
                    gridX += tileSize;
                }
                gridX = 195;
                gridY += tileSize;
            }
        }

        /// <summary>
        /// Exports the current tile matrix into a .cmap file.
        /// </summary>
 
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
					else if (BackColor == Color.LightGreen)
					{
						tiles[i, j] = 2;
					}
					else if (BackColor == Color.OrangeRed)
                    {
                        tiles[i, j] = 3;
                    }
					else if (BackColor == Color.DarkOrange)
					{
						tiles[i, j] = 4;
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
