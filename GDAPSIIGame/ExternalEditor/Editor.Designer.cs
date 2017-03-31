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
            this.createOpenLevelGroup.SuspendLayout();
            this.currentLevelGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // newLevelCollisionButton
            // 
            this.newLevelCollisionButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.newLevelCollisionButton.Location = new System.Drawing.Point(22, 107);
            this.newLevelCollisionButton.Name = "newLevelCollisionButton";
            this.newLevelCollisionButton.Size = new System.Drawing.Size(130, 24);
            this.newLevelCollisionButton.TabIndex = 1;
            this.newLevelCollisionButton.Text = "New Level Collision...";
            this.newLevelCollisionButton.UseVisualStyleBackColor = true;
            this.newLevelCollisionButton.Click += new System.EventHandler(this.newLevelCollisionButton_Click);
            // 
            // openLevelXTilesLabel
            // 
            this.openLevelXTilesLabel.AutoSize = true;
            this.openLevelXTilesLabel.Location = new System.Drawing.Point(22, 34);
            this.openLevelXTilesLabel.Name = "openLevelXTilesLabel";
            this.openLevelXTilesLabel.Size = new System.Drawing.Size(36, 13);
            this.openLevelXTilesLabel.TabIndex = 2;
            this.openLevelXTilesLabel.Text = "x tiles:";
            // 
            // openLevelYTilesLabel
            // 
            this.openLevelYTilesLabel.AutoSize = true;
            this.openLevelYTilesLabel.Location = new System.Drawing.Point(23, 65);
            this.openLevelYTilesLabel.Name = "openLevelYTilesLabel";
            this.openLevelYTilesLabel.Size = new System.Drawing.Size(36, 13);
            this.openLevelYTilesLabel.TabIndex = 3;
            this.openLevelYTilesLabel.Text = "y tiles:";
            // 
            // newLevelXTilesTextbox
            // 
            this.newLevelXTilesTextbox.Location = new System.Drawing.Point(64, 31);
            this.newLevelXTilesTextbox.Name = "newLevelXTilesTextbox";
            this.newLevelXTilesTextbox.Size = new System.Drawing.Size(88, 20);
            this.newLevelXTilesTextbox.TabIndex = 4;
            // 
            // newLevelYTilesTextbox
            // 
            this.newLevelYTilesTextbox.Location = new System.Drawing.Point(64, 62);
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
            this.createOpenLevelGroup.Location = new System.Drawing.Point(12, 12);
            this.createOpenLevelGroup.Name = "createOpenLevelGroup";
            this.createOpenLevelGroup.Size = new System.Drawing.Size(172, 187);
            this.createOpenLevelGroup.TabIndex = 6;
            this.createOpenLevelGroup.TabStop = false;
            this.createOpenLevelGroup.Text = "Create/Open Level";
            // 
            // openLevelCollisionButton
            // 
            this.openLevelCollisionButton.Location = new System.Drawing.Point(22, 146);
            this.openLevelCollisionButton.Name = "openLevelCollisionButton";
            this.openLevelCollisionButton.Size = new System.Drawing.Size(130, 23);
            this.openLevelCollisionButton.TabIndex = 6;
            this.openLevelCollisionButton.Text = "Open Level Collision...";
            this.openLevelCollisionButton.UseVisualStyleBackColor = true;
            this.openLevelCollisionButton.Click += new System.EventHandler(this.openLevelCollisionButton_Click);
            // 
            // wallToolButton
            // 
            this.wallToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.wallToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wallToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.wallToolButton.Location = new System.Drawing.Point(12, 207);
            this.wallToolButton.Name = "wallToolButton";
            this.wallToolButton.Size = new System.Drawing.Size(84, 42);
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
            this.enemyToolButton.Location = new System.Drawing.Point(99, 254);
            this.enemyToolButton.Name = "enemyToolButton";
            this.enemyToolButton.Size = new System.Drawing.Size(85, 42);
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
            this.deleteToolButton.Location = new System.Drawing.Point(99, 207);
            this.deleteToolButton.Name = "deleteToolButton";
            this.deleteToolButton.Size = new System.Drawing.Size(85, 42);
            this.deleteToolButton.TabIndex = 10;
            this.deleteToolButton.Text = "Delete";
            this.deleteToolButton.UseVisualStyleBackColor = false;
            this.deleteToolButton.Click += new System.EventHandler(this.deleteToolButton_Click);
            // 
            // spawnToolButton
            // 
            this.spawnToolButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.spawnToolButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spawnToolButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.spawnToolButton.Location = new System.Drawing.Point(12, 254);
            this.spawnToolButton.Name = "spawnToolButton";
            this.spawnToolButton.Size = new System.Drawing.Size(84, 42);
            this.spawnToolButton.TabIndex = 11;
            this.spawnToolButton.Text = "Spawn";
            this.spawnToolButton.UseVisualStyleBackColor = false;
            this.spawnToolButton.Click += new System.EventHandler(this.spawnToolButton_Click);
            // 
            // CurrentToolLabel
            // 
            this.CurrentToolLabel.AutoSize = true;
            this.CurrentToolLabel.Location = new System.Drawing.Point(12, 309);
            this.CurrentToolLabel.Name = "CurrentToolLabel";
            this.CurrentToolLabel.Size = new System.Drawing.Size(92, 13);
            this.CurrentToolLabel.TabIndex = 12;
            this.CurrentToolLabel.Text = "Current Tool: Wall";
            // 
            // currentLevelNameLabel
            // 
            this.currentLevelNameLabel.AutoSize = true;
            this.currentLevelNameLabel.Location = new System.Drawing.Point(19, 36);
            this.currentLevelNameLabel.Name = "currentLevelNameLabel";
            this.currentLevelNameLabel.Size = new System.Drawing.Size(38, 13);
            this.currentLevelNameLabel.TabIndex = 17;
            this.currentLevelNameLabel.Text = "Name:";
            // 
            // currentLevelNameTextbox
            // 
            this.currentLevelNameTextbox.Location = new System.Drawing.Point(64, 33);
            this.currentLevelNameTextbox.Name = "currentLevelNameTextbox";
            this.currentLevelNameTextbox.Size = new System.Drawing.Size(88, 20);
            this.currentLevelNameTextbox.TabIndex = 18;
            // 
            // currentLevelGroup
            // 
            this.currentLevelGroup.Controls.Add(this.saveLevelButton);
            this.currentLevelGroup.Controls.Add(this.currentLevelNameTextbox);
            this.currentLevelGroup.Controls.Add(this.currentLevelNameLabel);
            this.currentLevelGroup.Location = new System.Drawing.Point(12, 386);
            this.currentLevelGroup.Name = "currentLevelGroup";
            this.currentLevelGroup.Size = new System.Drawing.Size(172, 109);
            this.currentLevelGroup.TabIndex = 19;
            this.currentLevelGroup.TabStop = false;
            this.currentLevelGroup.Text = "Current Level";
            // 
            // saveLevelButton
            // 
            this.saveLevelButton.Location = new System.Drawing.Point(22, 64);
            this.saveLevelButton.Name = "saveLevelButton";
            this.saveLevelButton.Size = new System.Drawing.Size(130, 23);
            this.saveLevelButton.TabIndex = 19;
            this.saveLevelButton.Text = "Save Level";
            this.saveLevelButton.UseVisualStyleBackColor = true;
            this.saveLevelButton.Click += new System.EventHandler(this.saveLevelButton_Click);
            // 
            // loadingBar
            // 
            this.loadingBar.ForeColor = System.Drawing.Color.MediumPurple;
            this.loadingBar.Location = new System.Drawing.Point(15, 344);
            this.loadingBar.Name = "loadingBar";
            this.loadingBar.Size = new System.Drawing.Size(169, 16);
            this.loadingBar.TabIndex = 20;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 506);
            this.Controls.Add(this.loadingBar);
            this.Controls.Add(this.currentLevelGroup);
            this.Controls.Add(this.CurrentToolLabel);
            this.Controls.Add(this.enemyToolButton);
            this.Controls.Add(this.spawnToolButton);
            this.Controls.Add(this.deleteToolButton);
            this.Controls.Add(this.wallToolButton);
            this.Controls.Add(this.createOpenLevelGroup);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Editor";
            this.Text = "DieEditor";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Editor_MouseUp);
            this.createOpenLevelGroup.ResumeLayout(false);
            this.createOpenLevelGroup.PerformLayout();
            this.currentLevelGroup.ResumeLayout(false);
            this.currentLevelGroup.PerformLayout();
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
    }
}

