namespace DelApp.Locals
{
    internal sealed class AppLanguageProviderChs : AppLanguageProvider<AppLanguageProviderChs>
    {
        public override string TwoLetterISOLanguageName => "zh";

        public override int LCID => 2052;

        public override string UI_File => "文件";
        public override string UI_File_Open => "打开...";
        public override string UI_File_RightClickContextMenu => "添加到右键菜单";
        public override string UI_File_SourceCode => "源代码";
        public override string UI_File_Exit => "退出";

        public override string UI_Edit => "编辑";
        public override string UI_Edit_RemoveFromList => "移除项目";
        public override string UI_Edit_ClearList => "清空列表";
        public override string UI_Edit_FixPath => "修复错误路径";

        public override string UI_FastDelete => "快速删除";
        public override string UI_Delete => "删除";

        public override string UI_OpenDialog_Title => "打开...";
        public override string UI_OpenDialog_OKButton => "确定";
        public override string UI_OpenDialog_CancelButton => "取消";
        public override string UI_OpenDialog_FastAccess => "快速访问";
        public override string UI_OpenDialog_FastAccess_Desktop => "桌面";
        public override string UI_OpenDialog_FastAccess_Document => "我的文档";
        public override string UI_OpenDialog_Drives => "驱动器";

        public override string Shell_RightClickContextText => "加入 Delapp 删除列表";

        public override string Message_FailToFixPath => "修复路径失败。";
        public override string Message_Confirmation => "我知道我在干啥。";
        public override string Message_NotFullyCompleted => "只完成了部分的删除任务。";
    }
}
