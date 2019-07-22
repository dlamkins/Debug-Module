namespace Debug_Module {
    partial class RuntimeToolbox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.treeControlsAndEntities = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bttnSelectControl = new System.Windows.Forms.Button();
            this.bttnRefresh = new System.Windows.Forms.Button();
            this.cbAutoRefresh = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeControlsAndEntities
            // 
            this.treeControlsAndEntities.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeControlsAndEntities.FullRowSelect = true;
            this.treeControlsAndEntities.HideSelection = false;
            this.treeControlsAndEntities.HotTracking = true;
            this.treeControlsAndEntities.Location = new System.Drawing.Point(3, 32);
            this.treeControlsAndEntities.Name = "treeControlsAndEntities";
            this.treeControlsAndEntities.Size = new System.Drawing.Size(260, 383);
            this.treeControlsAndEntities.TabIndex = 0;
            this.treeControlsAndEntities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeControlsAndEntities_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bttnSelectControl);
            this.splitContainer1.Panel1.Controls.Add(this.bttnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.treeControlsAndEntities);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbAutoRefresh);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.propGrid);
            this.splitContainer1.Size = new System.Drawing.Size(786, 418);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 1;
            // 
            // bttnSelectControl
            // 
            this.bttnSelectControl.Location = new System.Drawing.Point(3, 3);
            this.bttnSelectControl.Name = "bttnSelectControl";
            this.bttnSelectControl.Size = new System.Drawing.Size(89, 23);
            this.bttnSelectControl.TabIndex = 2;
            this.bttnSelectControl.Text = "Select Control";
            this.bttnSelectControl.UseVisualStyleBackColor = true;
            this.bttnSelectControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BttnSelectControl_Click);
            // 
            // bttnRefresh
            // 
            this.bttnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bttnRefresh.Location = new System.Drawing.Point(188, 3);
            this.bttnRefresh.Name = "bttnRefresh";
            this.bttnRefresh.Size = new System.Drawing.Size(75, 23);
            this.bttnRefresh.TabIndex = 0;
            this.bttnRefresh.Text = "Refresh";
            this.bttnRefresh.UseVisualStyleBackColor = true;
            this.bttnRefresh.Click += new System.EventHandler(this.BttnRefresh_Click);
            // 
            // cbAutoRefresh
            // 
            this.cbAutoRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoRefresh.AutoSize = true;
            this.cbAutoRefresh.Location = new System.Drawing.Point(389, 7);
            this.cbAutoRefresh.Name = "cbAutoRefresh";
            this.cbAutoRefresh.Size = new System.Drawing.Size(124, 17);
            this.cbAutoRefresh.TabIndex = 4;
            this.cbAutoRefresh.Text = "Enable Auto-Refresh";
            this.cbAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(380, 23);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // propGrid
            // 
            this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propGrid.Location = new System.Drawing.Point(3, 32);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(510, 383);
            this.propGrid.TabIndex = 0;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Runtime Viewer";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // RuntimeToolbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "RuntimeToolbox";
            this.Text = "Runtime Toolbox";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.RuntimeToolbox_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeControlsAndEntities;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button bttnRefresh;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.CheckBox cbAutoRefresh;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button bttnSelectControl;
    }
}