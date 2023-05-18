namespace DelApp.Locals
{
    internal sealed class AppLanguageProviderEn : AppLanguageProvider<AppLanguageProviderEn>
    {
        public override string TwoLetterISOLanguageName => "en";

        public override int LCID => 9;

        public override string UI_File => "File";
        public override string UI_File_Open => "Open...";
        public override string UI_File_RightClickContextMenu => "Add to right-click menu";
        public override string UI_File_SourceCode => "Source code";
        public override string UI_File_Exit => "Exit";

        public override string UI_Edit => "Edit";
        public override string UI_Edit_RemoveFromList => "Remove from list";
        public override string UI_Edit_ClearList => "Clear list";
        public override string UI_Edit_FixPath => "Fix invalid path";

        public override string UI_FastDelete => "Fast delete";
        public override string UI_Delete => "Delete";

        public override string UI_OpenDialog_Title => "Open...";
        public override string UI_OpenDialog_OKButton => "OK";
        public override string UI_OpenDialog_CancelButton => "Cancel";
        public override string UI_OpenDialog_FastAccess => "Fast Access";
        public override string UI_OpenDialog_FastAccess_Desktop => "Desktop";
        public override string UI_OpenDialog_FastAccess_Document => "My Documents";
        public override string UI_OpenDialog_Drives => "Drives";

        public override string Shell_RightClickContextText => "Delapp - Add to delete-list";

        public override string Message_FailToFixPath => "Fail to fix path.";
        public override string Message_Confirmation => "I know what I want to do.";
        public override string Message_NotFullyCompleted => "Only part of delete tasks were completed.";
    }
}
