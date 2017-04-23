namespace ExternalEditor
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.newLevelCollisionButton = new System.Windows.Forms.Button();
			this.openLevelXTilesLabel = new System.Windows.Forms.Label();
			this.openLevelYTilesLabel = new System.Windows.Forms.Label();
			this.newLevelXTilesTextbox = new System.Windows.Forms.TextBox();
			this.newLevelYTilesTextbox = new System.Windows.Forms.TextBox();
			this.createOpenLevelGroup = new System.Windows.Forms.GroupBox();
			this.openLevelCollisionButton = new System.Windows.Forms.Button();
			this.wallToolButton = new System.Windows.Forms.Button();
			this.enemyToolButton = new System.Windows.Forms.Button();
			this.deleteToolButton = new System.Windows.Forms.Button();
			this.spawnToolButton = new System.Windows.Forms.Button();
			this.CurrentToolLabel = new System.Windows.Forms.Label();
			this.currentLevelNameLabel = new System.Windows.Forms.Label();
			this.currentLevelNameTextbox = new System.Windows.Forms.TextBox();
			this.currentLevelGroup = new System.Windows.Forms.GroupBox();
			this.saveLevelButton = new System.Windows.Forms.Button();
			this.loadingBar = new System.Windows.Forms.ProgressBar();
			this.clearLevelButton = new System.Windows.Forms.Button();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripTextBox = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripXYPos = new System.Windows.Forms.ToolStripStatusLabel();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.enemyDropDownBox = new System.Windows.Forms.ComboBox();
			this.createOpenLevelGroup.SuspendLayout();
			this.currentLevelGroup.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// newLevelCollisionButton
			// 
			this.newLevelCollisionButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.newLevelCollisionButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this.newLevelCollisionButton.Location = new System.Drawing.Point(22, 115);
			this.newLevelCollisionButton.Name = "newLevelCollisionButton";
			this.newLevelCollisionButton.Size = new System.Drawing.Size(130, 26);
			this.newLevelCollisionButton.TabIndex = 1;
			this.newLevelCollisionButton.Text = "New Level Collision...";
			this.newLevelCollisionButton.UseVisualStyleBackColor = false;
			this.newLevelCollisionButton.Click += new System.EventHandler(this.newLevelCollisionButton_Click);
			// 
			// openLevelXTilesLabel
			// 
			this.openLevelXTilesLabel.AutoSize = true;
			this.openLevelXTilesLabel.Location = new System.Drawing.Point(22, 37);
			this.openLevelXTilesLabel.Name = "openLevelXTilesLabel";
			this.openLevelXTilesLabel.Size = new System.Drawing.Size(38, 14);
			this.openLevelXTilesLabel.TabIndex = 2;
			this.openLevelXTilesLabel.Text = "x tiles:";
			// 
			// openLevelYTilesLabel
			// 
			this.openLevelYTilesLabel.AutoSize = true;
			this.openLevelYTilesLabel.Location = new System.Drawing.Point(23, 70);
			this.openLevelYTilesLabel.Name = "openLevelYTilesLabel";
			this.openLevelYTilesLabel.Size = new System.Drawing.Size(38, 14);
			this.openLevelYTilesLabel.TabIndex = 3;
			this.openLevelYTilesLabel.Text = "y tiles:";
			// 
			// newLevelXTilesTextbox
			// 
			this.newLevelXTilesTextbox.Location = new System.Drawing.Point(64, 33);
			this.newLevelXTilesTextbox.Name = "newLevelXTilesTextbox";
			this.newLevelXTilesTextbox.Size = new System.Drawing.Size(88, 20);
			this.newLevelXTilesTextbox.TabIndex = 4;
			// 
			// newLevelYTilesTextbox
			// 
			this.newLevelYTilesTextbox.Location = new System.Drawing.Point(64, 67);
			this.newLevelYTilesTextbox.Name = "newLevelYTilesTextbox";
			this.newLevelYTilesTextbox.Size = new System.Drawing.Size(88, 20);
			this.newLevelYTilesTextbox.TabIndex = 5;
			// 
			// createOpenLevelGroup
			// 
			this.createOpenLevelGroup.Controls.Add(this.openLevelCollisionButton);
			this.createOpenLevelGroup.Controls.Add(this.newLevelXTilesTextbox);
			this.createOpenLevelGroup.Controls.Add(this.newLevelYTilesTextbox);
			this.createOpenLevelGroup.Controls.Add(this.newLevelCollisionButton);
			this.createOpenLevelGroup.Controls.Add(this.openLevelXTilesLabel);
			this.createOpenLevelGroup.Controls.Add(this.openLevelYTilesLabel);
			this.createOpenLevelGroup.Location = new System.Drawing.Point(12, 13);
			this.createOpenLevelGroup.Name = "createOpenLevelGroup";
			this.createOpenLevelGroup.Size = new System.Drawing.Size(172, 201);
			this.createOpenLevelGroup.TabIndex = 6;
			this.createOpenLevelGroup.TabStop = false;
			this.createOpenLevelGroup.Text = "Create/Open Level";
			// 
			// openLevelCollisionButton
			// 
			this.openLevelCollisionButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.openLevelCollisionButton.Location = new System.Drawing.Point(22, 157);
			this.openLevelCollisionButton.Name = "openLevelCollisionButton";
			this.openLevelCollisionButton.Size = new System.Drawing.Size(130, 25);
			this.openLevelCollisionButton.TabIndex = 6;
			this.openLevelCollisionButton.Text = "Open Level Collision...";
			this.openLevelCollisionButton.UseVisualStyleBackColor = false;
			this.openLevelCollisionButton.Click += new System.EventHandler(this.openLevelCollisionButton_Click);
			// 
			// wallToolButton
			// 
			this.wallToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.wallToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wallToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.wallToolButton.Location = new System.Drawing.Point(12, 223);
			this.wallToolButton.Name = "wallToolButton";
			this.wallToolButton.Size = new System.Drawing.Size(84, 45);
			this.wallToolButton.TabIndex = 8;
			this.wallToolButton.Text = "Wall";
			this.wallToolButton.UseVisualStyleBackColor = false;
			this.wallToolButton.Click += new System.EventHandler(this.wallToolButton_Click);
			// 
			// enemyToolButton
			// 
			this.enemyToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.enemyToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.enemyToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.enemyToolButton.Location = new System.Drawing.Point(99, 274);
			this.enemyToolButton.Name = "enemyToolButton";
			this.enemyToolButton.Size = new System.Drawing.Size(85, 45);
			this.enemyToolButton.TabIndex = 9;
			this.enemyToolButton.Text = "Enemy";
			this.enemyToolButton.UseVisualStyleBackColor = false;
			this.enemyToolButton.Click += new System.EventHandler(this.enemyToolButton_Click);
			// 
			// deleteToolButton
			// 
			this.deleteToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.deleteToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.deleteToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.deleteToolButton.Location = new System.Drawing.Point(99, 223);
			this.deleteToolButton.Name = "deleteToolButton";
			this.deleteToolButton.Size = new System.Drawing.Size(85, 45);
			this.deleteToolButton.TabIndex = 10;
			this.deleteToolButton.Text = "Eraser";
			this.deleteToolButton.UseVisualStyleBackColor = false;
			this.deleteToolButton.Click += new System.EventHandler(this.deleteToolButton_Click);
			// 
			// spawnToolButton
			// 
			this.spawnToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.spawnToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.spawnToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.spawnToolButton.Location = new System.Drawing.Point(12, 274);
			this.spawnToolButton.Name = "spawnToolButton";
			this.spawnToolButton.Size = new System.Drawing.Size(84, 45);
			this.spawnToolButton.TabIndex = 11;
			this.spawnToolButton.Text = "Spawn";
			this.spawnToolButton.UseVisualStyleBackColor = false;
			this.spawnToolButton.Click += new System.EventHandler(this.spawnToolButton_Click);
			// 
			// CurrentToolLabel
			// 
			this.CurrentToolLabel.AutoSize = true;
			this.CurrentToolLabel.Location = new System.Drawing.Point(12, 358);
			this.CurrentToolLabel.Name = "CurrentToolLabel";
			this.CurrentToolLabel.Size = new System.Drawing.Size(91, 14);
			this.CurrentToolLabel.TabIndex = 12;
			this.CurrentToolLabel.Text = "Current Tool: Wall";
			// 
			// currentLevelNameLabel
			// 
			this.currentLevelNameLabel.AutoSize = true;
			this.currentLevelNameLabel.Location = new System.Drawing.Point(116, 33);
			this.currentLevelNameLabel.Name = "currentLevelNameLabel";
			this.currentLevelNameLabel.Size = new System.Drawing.Size(36, 14);
			this.currentLevelNameLabel.TabIndex = 17;
			this.currentLevelNameLabel.Text = ".cmap";
			// 
			// currentLevelNameTextbox
			// 
			this.currentLevelNameTextbox.Location = new System.Drawing.Point(23, 30);
			this.currentLevelNameTextbox.Name = "currentLevelNameTextbox";
			this.currentLevelNameTextbox.Size = new System.Drawing.Size(87, 20);
			this.currentLevelNameTextbox.TabIndex = 18;
			// 
			// currentLevelGroup
			// 
			this.currentLevelGroup.Controls.Add(this.saveLevelButton);
			this.currentLevelGroup.Controls.Add(this.currentLevelNameTextbox);
			this.currentLevelGroup.Controls.Add(this.currentLevelNameLabel);
			this.currentLevelGroup.Location = new System.Drawing.Point(12, 447);
			this.currentLevelGroup.Name = "currentLevelGroup";
			this.currentLevelGroup.Size = new System.Drawing.Size(172, 117);
			this.currentLevelGroup.TabIndex = 19;
			this.currentLevelGroup.TabStop = false;
			this.currentLevelGroup.Text = "Current Level";
			// 
			// saveLevelButton
			// 
			this.saveLevelButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.saveLevelButton.Location = new System.Drawing.Point(22, 69);
			this.saveLevelButton.Name = "saveLevelButton";
			this.saveLevelButton.Size = new System.Drawing.Size(130, 25);
			this.saveLevelButton.TabIndex = 19;
			this.saveLevelButton.Text = "Save Level";
			this.saveLevelButton.UseVisualStyleBackColor = false;
			this.saveLevelButton.Click += new System.EventHandler(this.saveLevelButton_Click);
			// 
			// loadingBar
			// 
			this.loadingBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.loadingBar.ForeColor = System.Drawing.Color.MediumPurple;
			this.loadingBar.Location = new System.Drawing.Point(12, 571);
			this.loadingBar.Name = "loadingBar";
			this.loadingBar.Size = new System.Drawing.Size(172, 17);
			this.loadingBar.TabIndex = 20;
			// 
			// clearLevelButton
			// 
			this.clearLevelButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.clearLevelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clearLevelButton.Location = new System.Drawing.Point(12, 387);
			this.clearLevelButton.Name = "clearLevelButton";
			this.clearLevelButton.Size = new System.Drawing.Size(172, 46);
			this.clearLevelButton.TabIndex = 21;
			this.clearLevelButton.Text = "🗑️";
			this.clearLevelButton.UseVisualStyleBackColor = false;
			this.clearLevelButton.Click += new System.EventHandler(this.clearLevelButton_Click);
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(23, 23);
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(23, 23);
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox,
            this.toolStripXYPos});
			this.toolStrip.Location = new System.Drawing.Point(0, 596);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(196, 22);
			this.toolStrip.TabIndex = 22;
			this.toolStrip.Text = "statusStrip1";
			// 
			// toolStripTextBox
			// 
			this.toolStripTextBox.Name = "toolStripTextBox";
			this.toolStripTextBox.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripXYPos
			// 
			this.toolStripXYPos.Name = "toolStripXYPos";
			this.toolStripXYPos.Size = new System.Drawing.Size(0, 17);
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.Refresh);
			// 
			// enemyDropDownBox
			// 
			this.enemyDropDownBox.Enabled = false;
			this.enemyDropDownBox.FormattingEnabled = true;
			this.enemyDropDownBox.Items.AddRange(new object[] {
            "Melee Enemy",
            "Turret Enemy"});
			this.enemyDropDownBox.Location = new System.Drawing.Point(37, 325);
			this.enemyDropDownBox.Name = "enemyDropDownBox";
			this.enemyDropDownBox.Size = new System.Drawing.Size(121, 22);
			this.enemyDropDownBox.TabIndex = 23;
			this.enemyDropDownBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(196, 618);
			this.Controls.Add(this.enemyDropDownBox);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.clearLevelButton);
			this.Controls.Add(this.loadingBar);
			this.Controls.Add(this.currentLevelGroup);
			this.Controls.Add(this.CurrentToolLabel);
			this.Controls.Add(this.enemyToolButton);
			this.Controls.Add(this.spawnToolButton);
			this.Controls.Add(this.deleteToolButton);
			this.Controls.Add(this.wallToolButton);
			this.Controls.Add(this.createOpenLevelGroup);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Editor";
			this.Text = "DieEditor";
			this.createOpenLevelGroup.ResumeLayout(false);
			this.createOpenLevelGroup.PerformLayout();
			this.currentLevelGroup.ResumeLayout(false);
			this.currentLevelGroup.PerformLayout();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button newLevelCollisionButton;
        private System.Windows.Forms.Label openLevelXTilesLabel;
        private System.Windows.Forms.Label openLevelYTilesLabel;
        private System.Windows.Forms.TextBox newLevelXTilesTextbox;
        private System.Windows.Forms.TextBox newLevelYTilesTextbox;
        private System.Windows.Forms.GroupBox createOpenLevelGroup;
        private System.Windows.Forms.Button openLevelCollisionButton;
        private System.Windows.Forms.Button wallToolButton;
        private System.Windows.Forms.Button enemyToolButton;
        private System.Windows.Forms.Button deleteToolButton;
        private System.Windows.Forms.Button spawnToolButton;
        private System.Windows.Forms.Label CurrentToolLabel;
        private System.Windows.Forms.Label currentLevelNameLabel;
        private System.Windows.Forms.TextBox currentLevelNameTextbox;
        private System.Windows.Forms.GroupBox currentLevelGroup;
        private System.Windows.Forms.ProgressBar loadingBar;
        private System.Windows.Forms.Button saveLevelButton;
        private System.Windows.Forms.Button clearLevelButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip toolStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTextBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStripXYPos;
        private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.ComboBox enemyDropDownBox;
	}
}

