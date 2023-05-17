using DelApp.Internals;
using DelApp.Locals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;




namespace DelApp
{
    internal partial class OpenFileDialogLite : Form
    {
        private const int FILE_ICON_INDEX = 0;
        private const int DIR_ICON_INDEX = 1;

        private readonly FileNDir _defaultDir;
        private FileNDir _currentDir;

        public OpenFileDialogLite()
        {
            InitializeComponent();


            IAppLanguageProvider lp = AppLanguageService.LanguageProvider;
            Text = lp.UI_OpenDialog_Title;
            ButtonOK.Text = lp.UI_OpenDialog_OKButton;
            ButtonCancel.Text = lp.UI_OpenDialog_CancelButton;

            ListViewMain.ListViewItemSorter = new ListViewComparer();
            ImageListMain.Images.Add("file", FromIconToBitmap(DefaultIcons.FileSmall));
            ImageListMain.Images.Add("dir", FromIconToBitmap(DefaultIcons.DirSmall));
            ListViewMain.SmallImageList = ImageListMain;
            ListViewMain.Columns[0].Width = Screen.PrimaryScreen.WorkingArea.Width;
            TreeViewMain.BeginUpdate();
            var rootFastAccess = new TreeNode(lp.UI_OpenDialog_FastAccess);
            _defaultDir = AddEnvFolder(rootFastAccess, Environment.SpecialFolder.DesktopDirectory, lp.UI_OpenDialog_FastAccess_Desktop);
            AddEnvFolder(rootFastAccess, Environment.SpecialFolder.MyDocuments, lp.UI_OpenDialog_FastAccess_Document);
            TreeViewMain.Nodes.Add(rootFastAccess);
            TreeViewMain.Nodes.Add(lp.UI_OpenDialog_Drives);
            TreeViewMain.EndUpdate();

        }

        public List<FileNDir> Pathes { get; } = new List<FileNDir>();




        internal static void TryAddToList(ListView listview, FileNDir fd, bool check = true)
        {
            int itemIconIdx;
            if (fd.IsFile)
            {
                itemIconIdx = FILE_ICON_INDEX;
                if (check && ListViewContainsFile(listview, fd.FullPath))
                    return;
            }
            else
            {
                itemIconIdx = DIR_ICON_INDEX;
                if (check && ListViewContainsDir(listview, fd.FullPath))
                    return;
            }

            var listItem = new ListViewItem(fd.FullPath, itemIconIdx)
            {
                Tag = fd,
                ToolTipText = fd.FullPath
            };
            listItem.SubItems.Add(fd.FullPath);
            listview.Items.Add(listItem);

        }


        private static bool ListViewContainsFile(ListView listview, string path)
        {
            ListView.ListViewItemCollection listViewItemCollection = listview.Items;
            ListViewItem item;
            FileNDir file;
            for (int i = 0; i < listViewItemCollection.Count; i++)
            {
                item = listViewItemCollection[i];
                file = item.Tag as FileNDir;
                if (file.IsFile)
                {
                    if (item.Text == path)
                        return true;
                }
                else if (path.StartsWith(item.Text))
                    return true;
            }
            return false;
        }

        private static bool ListViewContainsDir(ListView listview, string path)
        {
            ListView.ListViewItemCollection listViewItemCollection = listview.Items;
            ListViewItem item;
            FileNDir file;

            int length = listViewItemCollection.Count;
            for (int i = 0; i < length; i++)
            {
                item = listViewItemCollection[i];
                file = item.Tag as FileNDir;
                if (file.IsFile)
                {
                    if (item.Text.StartsWith(path))
                    {
                        listview.Items.RemoveAt(i);
                        --i;
                        --length;
                    }

                }
                else
                {
                    if (path.Length >= item.Text.Length)
                    {
                        if (path.StartsWith(item.Text))
                            return true;
                    }
                    else if (item.Text.StartsWith(path))
                    {
                        listview.Items.RemoveAt(i);
                        --i;
                        --length;
                    }
                }
            }
            return false;
        }


        private void ListViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = ListViewMain.SelectedIndices.Count;
            ButtonOK.Enabled = count != 0;
        }

        private void ListViewMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = ListViewMain.HitTest(e.X, e.Y);
            ListViewItem item = info.Item;

            if (item != null && item.Tag is FileNDir file && !file.IsFile)
            {
                _currentDir = file;
                RefreshListView();
            }

        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Pathes.Clear();

            var items = ListViewMain.SelectedItems;
            var length = items.Count;
            FileNDir file;
            for (int i = 0; i < length; i++)
            {
                file = (items[i].Tag as FileNDir);
                if (file.Exists)
                    Pathes.Add(file);
            }
            this.Close();
        }

        private void ListViewMain_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (ListViewMain.Columns[0].Tag is FileNDir file)
            {
                _currentDir = file;
                RefreshListView();
            }
        }

        private void TreeViewMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var selNode = e.Node;
            if (selNode.Tag is FileNDir file)
            {
                _currentDir = file;
                RefreshListView();
            }
        }


        private void OpenFileDialogLite_Load(object sender, EventArgs e)
        {
            TreeViewMain.BeginUpdate();
            TreeNode root = TreeViewMain.Nodes[1];
            root.Nodes.Clear();
            foreach (string item in InternelDriveInfo.RefreshDriveCacheAndReturnsDriveName())
            {
                AddNode(root, item, item);
            }
            TreeViewMain.ExpandAll();
            TreeViewMain.EndUpdate();

            RefreshListView();
        }


        private void RefreshListView()
        {
            if (_currentDir == null)
            {
                _currentDir = _defaultDir;
            }
            else
            {
                if (!_currentDir.Exists)
                    _currentDir = _defaultDir;
            }

            ListViewMain.BeginUpdate();
            ListViewMain.SelectedItems.Clear();
            ListViewMain.Items.Clear();
            ListViewMain.Columns[0].Tag = _currentDir.PreviewDir;
            foreach (FileNDir item in _currentDir.GetChilds())
            {
                TryAddToList(ListViewMain, item, false);
            }

            ListViewMain.EndUpdate();
        }

        private FileNDir AddNode(TreeNode node, string path, string text)
        {
            var file = new FileNDir(path);
            node.Nodes.Add(new TreeNode(text)
            {
                Tag = file
            });
            return file;
        }

        private FileNDir AddEnvFolder(TreeNode node, Environment.SpecialFolder folder, string text)
        {
            try
            {
                string path = Environment.GetFolderPath(folder);
                return path == string.Empty ? null : AddNode(node, path, text);
            }
            catch (Exception ecx)
            {
                Utils.WriteErrorLog(ecx.Message);
                return null;
            }

        }

        private static Bitmap FromIconToBitmap(Icon icon)
        {
            Bitmap bmp = new Bitmap(icon.Width, icon.Height);
            using (Graphics gp = Graphics.FromImage(bmp))
            {
                gp.Clear(Color.Transparent);
                gp.DrawIcon(icon, new Rectangle(0, 0, icon.Width, icon.Height));
            }
            return bmp;
        }


        private sealed class ListViewComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var a = (x as ListViewItem);
                var b = (y as ListViewItem);
                int ret = -(a.ImageIndex.CompareTo(b.ImageIndex));
                return ret == 0 ?
                          a.Name.CompareTo(b.Name) :
                          ret;
            }
        }


    }
}
