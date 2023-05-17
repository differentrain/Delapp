namespace DelApp
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ToolStripMain = new System.Windows.Forms.ToolStrip();
            this.ToolStripDropDownButtonFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemRightClickContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSource = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripDropDownButtonEdit = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemClearList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemFix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonFastDelete = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.ListViewMain = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BackgroundWorkerMain = new System.ComponentModel.BackgroundWorker();
            this.ToolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStripMain
            // 
            this.ToolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripDropDownButtonFile,
            this.ToolStripDropDownButtonEdit,
            this.toolStripSeparator4,
            this.ToolStripButtonFastDelete,
            this.ToolStripButtonDelete});
            this.ToolStripMain.Location = new System.Drawing.Point(0, 0);
            this.ToolStripMain.Name = "ToolStripMain";
            this.ToolStripMain.Size = new System.Drawing.Size(433, 25);
            this.ToolStripMain.TabIndex = 0;
            // 
            // ToolStripDropDownButtonFile
            // 
            this.ToolStripDropDownButtonFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripDropDownButtonFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemOpen,
            this.toolStripSeparator1,
            this.ToolStripMenuItemRightClickContextMenu,
            this.ToolStripMenuItemSource,
            this.toolStripSeparator3,
            this.ToolStripMenuItemExitApp});
            this.ToolStripDropDownButtonFile.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripDropDownButtonFile.Image")));
            this.ToolStripDropDownButtonFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDropDownButtonFile.Name = "ToolStripDropDownButtonFile";
            this.ToolStripDropDownButtonFile.Size = new System.Drawing.Size(40, 22);
            this.ToolStripDropDownButtonFile.Text = "File";
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItemOpen.Text = "Open...";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // ToolStripMenuItemRightClickContextMenu
            // 
            this.ToolStripMenuItemRightClickContextMenu.CheckOnClick = true;
            this.ToolStripMenuItemRightClickContextMenu.Name = "ToolStripMenuItemRightClickContextMenu";
            this.ToolStripMenuItemRightClickContextMenu.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItemRightClickContextMenu.Text = "Right click context menu";
            // 
            // ToolStripMenuItemSource
            // 
            this.ToolStripMenuItemSource.Name = "ToolStripMenuItemSource";
            this.ToolStripMenuItemSource.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItemSource.Text = "Source code";
            this.ToolStripMenuItemSource.Click += new System.EventHandler(this.ToolStripMenuItemSource_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // ToolStripMenuItemExitApp
            // 
            this.ToolStripMenuItemExitApp.Name = "ToolStripMenuItemExitApp";
            this.ToolStripMenuItemExitApp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ToolStripMenuItemExitApp.Size = new System.Drawing.Size(217, 22);
            this.ToolStripMenuItemExitApp.Text = "Exit";
            this.ToolStripMenuItemExitApp.Click += new System.EventHandler(this.ToolStripMenuItemExitApp_Click);
            // 
            // ToolStripDropDownButtonEdit
            // 
            this.ToolStripDropDownButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripDropDownButtonEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemRemove,
            this.ToolStripMenuItemClearList,
            this.toolStripSeparator2,
            this.ToolStripMenuItemFix});
            this.ToolStripDropDownButtonEdit.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripDropDownButtonEdit.Image")));
            this.ToolStripDropDownButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDropDownButtonEdit.Name = "ToolStripDropDownButtonEdit";
            this.ToolStripDropDownButtonEdit.Size = new System.Drawing.Size(43, 22);
            this.ToolStripDropDownButtonEdit.Text = "Edit";
            // 
            // ToolStripMenuItemRemove
            // 
            this.ToolStripMenuItemRemove.Enabled = false;
            this.ToolStripMenuItemRemove.Name = "ToolStripMenuItemRemove";
            this.ToolStripMenuItemRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.ToolStripMenuItemRemove.Size = new System.Drawing.Size(220, 22);
            this.ToolStripMenuItemRemove.Text = "Remove from list";
            this.ToolStripMenuItemRemove.Click += new System.EventHandler(this.ToolStripMenuItemRemove_Click);
            // 
            // ToolStripMenuItemClearList
            // 
            this.ToolStripMenuItemClearList.Enabled = false;
            this.ToolStripMenuItemClearList.Name = "ToolStripMenuItemClearList";
            this.ToolStripMenuItemClearList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.ToolStripMenuItemClearList.Size = new System.Drawing.Size(220, 22);
            this.ToolStripMenuItemClearList.Text = "Clear list";
            this.ToolStripMenuItemClearList.Click += new System.EventHandler(this.ToolStripMenuItemClearList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
            // 
            // ToolStripMenuItemFix
            // 
            this.ToolStripMenuItemFix.Enabled = false;
            this.ToolStripMenuItemFix.Name = "ToolStripMenuItemFix";
            this.ToolStripMenuItemFix.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ToolStripMenuItemFix.Size = new System.Drawing.Size(220, 22);
            this.ToolStripMenuItemFix.Text = "Fix invalid path";
            this.ToolStripMenuItemFix.Click += new System.EventHandler(this.ToolStripMenuItemFix_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripButtonFastDelete
            // 
            this.ToolStripButtonFastDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButtonFastDelete.Enabled = false;
            this.ToolStripButtonFastDelete.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonFastDelete.Image")));
            this.ToolStripButtonFastDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonFastDelete.Name = "ToolStripButtonFastDelete";
            this.ToolStripButtonFastDelete.Size = new System.Drawing.Size(76, 22);
            this.ToolStripButtonFastDelete.Text = "Fast Delete";
            this.ToolStripButtonFastDelete.Click += new System.EventHandler(this.ToolStripButtonDelete_Click);
            // 
            // ToolStripButtonDelete
            // 
            this.ToolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripButtonDelete.Enabled = false;
            this.ToolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonDelete.Image")));
            this.ToolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonDelete.Name = "ToolStripButtonDelete";
            this.ToolStripButtonDelete.Size = new System.Drawing.Size(49, 22);
            this.ToolStripButtonDelete.Text = "Delete";
            this.ToolStripButtonDelete.Click += new System.EventHandler(this.ToolStripButtonDelete_Click);
            // 
            // ListViewMain
            // 
            this.ListViewMain.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.ListViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ListViewMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ListViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewMain.FullRowSelect = true;
            this.ListViewMain.GridLines = true;
            this.ListViewMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListViewMain.HideSelection = false;
            this.ListViewMain.Location = new System.Drawing.Point(0, 25);
            this.ListViewMain.Name = "ListViewMain";
            this.ListViewMain.ShowGroups = false;
            this.ListViewMain.ShowItemToolTips = true;
            this.ListViewMain.Size = new System.Drawing.Size(433, 261);
            this.ListViewMain.TabIndex = 1;
            this.ListViewMain.UseCompatibleStateImageBehavior = false;
            this.ListViewMain.View = System.Windows.Forms.View.Details;
            this.ListViewMain.SelectedIndexChanged += new System.EventHandler(this.ListViewMain_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Delete list";
            this.columnHeader1.Width = 10000;
            // 
            // BackgroundWorkerMain
            // 
            this.BackgroundWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerMain_DoWork);
            this.BackgroundWorkerMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerMain_RunWorkerCompleted);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 286);
            this.Controls.Add(this.ListViewMain);
            this.Controls.Add(this.ToolStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delapp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ToolStripMain.ResumeLayout(false);
            this.ToolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolStripMain;
        private System.Windows.Forms.ToolStripDropDownButton ToolStripDropDownButtonFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExitApp;
        private System.Windows.Forms.ToolStripButton ToolStripButtonDelete;
        private System.Windows.Forms.ListView ListViewMain;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRightClickContextMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.ComponentModel.BackgroundWorker BackgroundWorkerMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton ToolStripDropDownButtonEdit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRemove;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClearList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFix;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSource;
        private System.Windows.Forms.ToolStripButton ToolStripButtonFastDelete;
    }
}

