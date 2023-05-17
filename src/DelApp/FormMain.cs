using DelApp.Internals;
using DelApp.Internals.Win32;
using DelApp.Locals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;


namespace DelApp
{
    internal partial class FormMain : Form, IMessageFilter
    {
        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint WM_COPYDATA = 0x004A;
        private const uint WM_DROPFILES = 0x0233;
        private const uint MSGFLT_ALLOW = 1;
        private const uint MSGFLT_RESET = 0;

        private readonly bool _canDrag;
        private readonly OpenFileDialogLite _openPathDialog = new OpenFileDialogLite();

        public FormMain()
        {
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            _openPathDialog.Owner = this;

            PipeService.PathRecived += PipeService_PathRecived;

            ListViewMain.Columns[0].Width = Screen.GetWorkingArea(this).Width;
            ListViewMain.SmallImageList = _openPathDialog.ImageListMain;

            _canDrag = RegDragDropMsgFilter(ListViewMain);

            try
            {
                ToolStripMenuItemRightClickContextMenu.Checked = RightClickMenuHelper.HasRightClickMenu;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            finally
            {
                ToolStripMenuItemRightClickContextMenu.CheckedChanged += ToolStripMenuItemRightClickContextMenu_CheckedChanged;
            }

            if (Program.FilePath != null)
            {
                ListViewMain.BeginUpdate();
                OpenFileDialogLite.TryAddToList(ListViewMain, new FileNDir(Program.FilePath));
                ListViewMain.EndUpdate();
            }

            IAppLanguageProvider lp = AppLanguageService.LanguageProvider;

            ToolStripDropDownButtonFile.Text = lp.UI_File;
            ToolStripMenuItemOpen.Text = lp.UI_File_Open;
            ToolStripMenuItemRightClickContextMenu.Text = lp.UI_File_RightClickContextMenu;
            ToolStripMenuItemSource.Text = lp.UI_File_SourceCode;
            ToolStripMenuItemExitApp.Text = lp.UI_File_Exit;

            ToolStripDropDownButtonEdit.Text = lp.UI_Edit;
            ToolStripMenuItemRemove.Text = lp.UI_Edit_RemoveFromList;
            ToolStripMenuItemClearList.Text = lp.UI_Edit_ClearList;
            ToolStripMenuItemFix.Text = lp.UI_Edit_FixPath;

            ToolStripButtonFastDelete.Text = lp.UI_FastDelete;
            ToolStripButtonDelete.Text = lp.UI_Delete;

        }

        private void PipeService_PathRecived(object sender, FileNDir e)
        {
            if (ListViewMain.Enabled && PipeService.PathQueue.TryDequeue(out FileNDir fd))
            {
                ListViewMain.BeginUpdate();
                OpenFileDialogLite.TryAddToList(ListViewMain, fd);
                ListViewMain.EndUpdate();
                EnsureButtonForHoleList();
            }
        }

        private void ToolStripMenuItemExitApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_DROPFILES)
            {
                ListViewMain.BeginUpdate();
                foreach (FileNDir item in Utils.GetDrapDropFiles(m.WParam))
                {
                    OpenFileDialogLite.TryAddToList(ListViewMain, item);
                }
                ListViewMain.EndUpdate();
                EnsureButtonForHoleList();
                return true;
            }
            return false;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_canDrag)
            {
                RemoveDragDropMsgFilter(ListViewMain);
            }
        }

        private bool RegDragDropMsgFilter(Control control)
        {
            IntPtr handle = control.Handle;
            var status = new ChangeFilterStruct() { CbSize = 8 };

            if (NativeMethods.ChangeWindowMessageFilterEx(handle, WM_DROPFILES, MSGFLT_ALLOW, in status) &&
                NativeMethods.ChangeWindowMessageFilterEx(handle, WM_COPYGLOBALDATA, MSGFLT_ALLOW, in status) &&
                NativeMethods.ChangeWindowMessageFilterEx(handle, WM_COPYDATA, MSGFLT_ALLOW, in status))
            {
                NativeMethods.DragAcceptFiles(handle, true);
                Application.AddMessageFilter(this);
                return true;
            }
            return false;
        }

        private void RemoveDragDropMsgFilter(Control control)
        {
            IntPtr handle = control.Handle;
            NativeMethods.DragAcceptFiles(handle, false);
            var status = new ChangeFilterStruct() { CbSize = 8 };
            NativeMethods.ChangeWindowMessageFilterEx(handle, WM_DROPFILES, MSGFLT_RESET, in status);
            NativeMethods.ChangeWindowMessageFilterEx(handle, WM_COPYGLOBALDATA, MSGFLT_RESET, in status);
            NativeMethods.ChangeWindowMessageFilterEx(handle, WM_COPYDATA, MSGFLT_RESET, in status);
            Application.RemoveMessageFilter(this);
        }



        private void ToolStripMenuItemRightClickContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItemRightClickContextMenu.CheckedChanged -= ToolStripMenuItemRightClickContextMenu_CheckedChanged;
            try
            {
                RightClickMenuHelper.HasRightClickMenu = ToolStripMenuItemRightClickContextMenu.Checked;
                ToolStripMenuItemRightClickContextMenu.Checked = RightClickMenuHelper.HasRightClickMenu;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            finally
            {
                ToolStripMenuItemRightClickContextMenu.CheckedChanged += ToolStripMenuItemRightClickContextMenu_CheckedChanged;
            }
        }

        private void ListViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selCount = ListViewMain.SelectedItems.Count;
            if (selCount == 0)
            {
                ToolStripMenuItemFix.Enabled = ToolStripMenuItemRemove.Enabled = false;
            }
            else
            {
                ToolStripMenuItemRemove.Enabled = true;
                ToolStripMenuItemFix.Enabled = selCount == 1 && (ListViewMain.SelectedItems[0].Tag as FileNDir).IsInvalidPath;
            }

        }

        private void ToolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            while (ListViewMain.SelectedItems.Count > 0)
            {
                ListViewMain.SelectedItems[0].Remove();
            }
            EnsureButtonForHoleList();
        }

        private void ToolStripMenuItemClearList_Click(object sender, EventArgs e)
        {
            ListViewMain.SelectedItems.Clear();
            ListViewMain.Items.Clear();
            EnsureButtonForHoleList();
        }


        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            if (_openPathDialog.ShowDialog() == DialogResult.OK)
            {
                int count = _openPathDialog.Pathes.Count;
                for (int i = 0; i < count; i++)
                {
                    OpenFileDialogLite.TryAddToList(ListViewMain, _openPathDialog.Pathes[i]);
                }
            }
            EnsureButtonForHoleList();
        }

        private void ToolStripMenuItemFix_Click(object sender, EventArgs e)
        {
            bool ret = (ListViewMain.SelectedItems[0].Tag as FileNDir).FixPath(out FileNDir newFile);

            if (ret)
            {
                ListViewMain.SelectedItems[0].Remove();
                OpenFileDialogLite.TryAddToList(ListViewMain, newFile);
            }
            else
            {
                MessageBox.Show(AppLanguageService.LanguageProvider.Message_FailToFixPath);
            }
        }

        private void ToolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppLanguageService.LanguageProvider.Message_Confirmation, "Del App", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ToolStripDropDownButtonFile.Enabled = false;
                ToolStripDropDownButtonEdit.Enabled = false;
                ListViewMain.Enabled = false;
                ToolStripButtonDelete.Enabled = false;
                ToolStripButtonFastDelete.Enabled = false;

                BackgroundWorkerMain.RunWorkerAsync(
                    (sender as ToolStripButton).Name == "ToolStripButtonFastDelete");
            }
        }

        private bool EnsureButtonForHoleList()
        {
            return ToolStripButtonFastDelete.Enabled = ToolStripButtonDelete.Enabled = ToolStripMenuItemClearList.Enabled = ListViewMain.Items.Count > 0;
        }

        private void BackgroundWorkerMain_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((bool)e.Argument)
                FastDel(e);
            else
                NormalDel(e);


        }

        private void FastDel(DoWorkEventArgs e)
        {
            ListView.ListViewItemCollection items = ListViewMain.Items;
            List<FileNDir> files = ObjPool.RentFDList();
            List<FileNDir> dirs = ObjPool.RentFDList();
            FileNDir fd;
            ListViewMain.BeginUpdate();
            for (int i = 0; i < items.Count; i++)
            {
                fd = items[i].Tag as FileNDir;
                if (fd.FastDelete(files, dirs))
                    items.RemoveAt(i--);
            }
            ListViewMain.EndUpdate();
            if (files.Count > 0)
            {
                InternelDriveInfo.RefreshDriveCache();

                int[] locker = RestartManagerHelper.Shared.GetHolderList(out _, files.Select(f => f.FullPath).ToArray());
                FileUnlocker.UnlockModuleAndMemory(files.Select(fd1 => fd1.FullPath).ToDictionary(s => s, s => 0), locker);
                DeleteLockedFile(files, items);
                if (files.Count > 0)
                {
                    FileUnlocker.UnlockHandle(files.Select(fd1 => fd1.FullPath).ToDictionary(s => s, s => 0), locker);
                    DeleteLockedFile(files, items);
                }
                DeleteLockedFile(dirs, items);
            }
            e.Result = files.Count + dirs.Count;
            ObjPool.ReturnFDList(files);
            ObjPool.ReturnFDList(dirs);

            void DeleteLockedFile(List<FileNDir> fl, ListView.ListViewItemCollection ob)
            {
                ListViewMain.BeginUpdate();
                FileNDir fd1;
                FileNDir fd2;
                for (int i = 0; i < fl.Count; i++)
                {
                    fd1 = fl[i];
                    if (fd1.FastDelete(null, null))
                    {
                        fl.RemoveAt(i--);
                        for (int j = 0; j < ob.Count; j++)
                        {
                            fd2 = ob[j].Tag as FileNDir;
                            if (fd2.Equals(fd1))
                            {
                                ob.RemoveAt(j);
                                break;
                            }
                        }
                    }
                }
                ListViewMain.EndUpdate();
            }
        }



        private void NormalDel(DoWorkEventArgs e)
        {
            ListView.ListViewItemCollection items = ListViewMain.Items;
            List<FileNDir> pathes = ObjPool.RentFDList();
            FileNDir fd;
            ListViewMain.BeginUpdate();
            for (int i = 0; i < items.Count; i++)
            {
                fd = items[i].Tag as FileNDir;
                if (fd.Delete(pathes))
                    items.RemoveAt(i--);
            }
            ListViewMain.EndUpdate();
            if (pathes.Count > 0)
            {
                InternelDriveInfo.RefreshDriveCache();
                FileUnlocker.UnlockModuleAndMemory(pathes.Select(pt => pt.FullPath).ToDictionary(s => s, s => 0));
                DeleteLockedFile(pathes, items);
                if (pathes.Count > 0)
                {
                    FileUnlocker.UnlockHandle(pathes.Select(pt => pt.FullPath).ToDictionary(s => s, s => 0));
                    DeleteLockedFile(pathes, items);
                }
                void DeleteLockedFile(List<FileNDir> pl, ListView.ListViewItemCollection ob)
                {
                    ListViewMain.BeginUpdate();
                    FileNDir fd1;
                    FileNDir fd2;
                    for (int i = 0; i < pl.Count; i++)
                    {
                        fd1 = pl[i];
                        if (fd1.Delete(null))
                        {
                            pl.RemoveAt(i--);
                            for (int j = 0; j < ob.Count; j++)
                            {
                                fd2 = ob[j].Tag as FileNDir;
                                if (fd2.Equals(fd1))
                                {
                                    ob.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }
                    ListViewMain.EndUpdate();
                }
            }
            e.Result = pathes.Count;
            ObjPool.ReturnFDList(pathes);
        }



        private void BackgroundWorkerMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((int)e.Result != 0)
            {
                MessageBox.Show(AppLanguageService.LanguageProvider.Message_NotFullyCompleted, "Delapp");
            }
            else
                SoundPlayHelper.Shared.TryPlayEmptyRecyclebin(true);

            ToolStripDropDownButtonFile.Enabled = true;
            ToolStripDropDownButtonEdit.Enabled = true;
            ListViewMain.Enabled = true;
            ListViewMain.BeginUpdate();
            while (PipeService.PathQueue.Count > 0)
            {
                if (PipeService.PathQueue.TryDequeue(out FileNDir fd))
                    OpenFileDialogLite.TryAddToList(ListViewMain, fd);
            }
            ListViewMain.EndUpdate();
            EnsureButtonForHoleList();
        }

        private void ToolStripMenuItemSource_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.baidu.com").Dispose();
        }
    }
}
