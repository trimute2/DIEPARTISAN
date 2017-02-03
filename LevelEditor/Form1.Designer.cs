namespace LevelEditor
{
    partial class LevelEditor
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importLevellvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importLeveltxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportLevellvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportLeveltxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportLevelButton = new System.Windows.Forms.Button();
            this.ExportLevelButton = new System.Windows.Forms.Button();
            this.ImportTilesetButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MenuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1179, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "Menu Strip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importLevellvToolStripMenuItem,
            this.importLeveltxtToolStripMenuItem,
            this.exportLevellvToolStripMenuItem,
            this.exportLeveltxtToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // importLevellvToolStripMenuItem
            // 
            this.importLevellvToolStripMenuItem.Name = "importLevellvToolStripMenuItem";
            this.importLevellvToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importLevellvToolStripMenuItem.Text = "Import Level.lv...";
            // 
            // importLeveltxtToolStripMenuItem
            // 
            this.importLeveltxtToolStripMenuItem.Name = "importLeveltxtToolStripMenuItem";
            this.importLeveltxtToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importLeveltxtToolStripMenuItem.Text = "Import Level.txt...";
            // 
            // exportLevellvToolStripMenuItem
            // 
            this.exportLevellvToolStripMenuItem.Name = "exportLevellvToolStripMenuItem";
            this.exportLevellvToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportLevellvToolStripMenuItem.Text = "Export Level.lv...";
            // 
            // exportLeveltxtToolStripMenuItem
            // 
            this.exportLeveltxtToolStripMenuItem.Name = "exportLeveltxtToolStripMenuItem";
            this.exportLeveltxtToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportLeveltxtToolStripMenuItem.Text = "Export Level.txt...";
            // 
            // ImportLevelButton
            // 
            this.ImportLevelButton.Location = new System.Drawing.Point(12, 27);
            this.ImportLevelButton.Name = "ImportLevelButton";
            this.ImportLevelButton.Size = new System.Drawing.Size(192, 36);
            this.ImportLevelButton.TabIndex = 1;
            this.ImportLevelButton.Text = "Import Level...";
            this.ImportLevelButton.UseVisualStyleBackColor = true;
            // 
            // ExportLevelButton
            // 
            this.ExportLevelButton.Location = new System.Drawing.Point(12, 69);
            this.ExportLevelButton.Name = "ExportLevelButton";
            this.ExportLevelButton.Size = new System.Drawing.Size(192, 36);
            this.ExportLevelButton.TabIndex = 2;
            this.ExportLevelButton.Text = "Export Level...";
            this.ExportLevelButton.UseVisualStyleBackColor = true;
            // 
            // ImportTilesetButton
            // 
            this.ImportTilesetButton.Location = new System.Drawing.Point(9, 461);
            this.ImportTilesetButton.Name = "ImportTilesetButton";
            this.ImportTilesetButton.Size = new System.Drawing.Size(195, 46);
            this.ImportTilesetButton.TabIndex = 4;
            this.ImportTilesetButton.Text = "Import Tileset...";
            this.ImportTilesetButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(210, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(960, 480);
            this.panel2.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(190, 324);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(190, 324);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 108);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(198, 350);
            this.tabControl1.TabIndex = 0;
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 514);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ImportTilesetButton);
            this.Controls.Add(this.ExportLevelButton);
            this.Controls.Add(this.ImportLevelButton);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "LevelEditor";
            this.Text = "GDAPS Level Editor";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importLevellvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importLeveltxtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportLevellvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportLeveltxtToolStripMenuItem;
        private System.Windows.Forms.Button ImportLevelButton;
        private System.Windows.Forms.Button ExportLevelButton;
        private System.Windows.Forms.Button ImportTilesetButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

