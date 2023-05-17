namespace DelApp
{
    partial class OpenFileDialogLite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenFileDialogLite));
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ListViewMain = new System.Windows.Forms.ListView();
            this.ColumnHeaderBack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImageListMain = new System.Windows.Forms.ImageList(this.components);
            this.TreeViewMain = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(329, 275);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.TabStop = false;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(12, 275);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // ListViewMain
            // 
            this.ListViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeaderBack});
            this.ListViewMain.FullRowSelect = true;
            this.ListViewMain.HideSelection = false;
            this.ListViewMain.Location = new System.Drawing.Point(143, 12);
            this.ListViewMain.Name = "ListViewMain";
            this.ListViewMain.Size = new System.Drawing.Size(261, 253);
            this.ListViewMain.TabIndex = 4;
            this.ListViewMain.UseCompatibleStateImageBehavior = false;
            this.ListViewMain.View = System.Windows.Forms.View.Details;
            this.ListViewMain.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewMain_ColumnClick);
            this.ListViewMain.SelectedIndexChanged += new System.EventHandler(this.ListViewMain_SelectedIndexChanged);
            this.ListViewMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewMain_MouseDoubleClick);
            // 
            // ColumnHeaderBack
            // 
            this.ColumnHeaderBack.Text = "↑";
            // 
            // ImageListMain
            // 
            this.ImageListMain.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageListMain.ImageSize = new System.Drawing.Size(16, 16);
            this.ImageListMain.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TreeViewMain
            // 
            this.TreeViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeViewMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeViewMain.FullRowSelect = true;
            this.TreeViewMain.HideSelection = false;
            this.TreeViewMain.Location = new System.Drawing.Point(12, 12);
            this.TreeViewMain.Name = "TreeViewMain";
            this.TreeViewMain.Size = new System.Drawing.Size(125, 253);
            this.TreeViewMain.TabIndex = 5;
            this.TreeViewMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewMain_NodeMouseClick);
            // 
            // OpenFileDialogLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(411, 304);
            this.Controls.Add(this.TreeViewMain);
            this.Controls.Add(this.ListViewMain);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(323, 213);
            this.Name = "OpenFileDialogLite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open...";
            this.Load += new System.EventHandler(this.OpenFileDialogLite_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.ListView ListViewMain;
        public System.Windows.Forms.ImageList ImageListMain;
        private System.Windows.Forms.TreeView TreeViewMain;
        private System.Windows.Forms.ColumnHeader ColumnHeaderBack;
    }
}