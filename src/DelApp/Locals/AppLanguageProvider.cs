namespace DelApp.Locals
{
    public abstract class AppLanguageProvider<TSelf> : IAppLanguageProvider
        where TSelf : AppLanguageProvider<TSelf>, new()
    {

        public static readonly TSelf Instance = new TSelf();

        public abstract string TwoLetterISOLanguageName { get; }

        public abstract int LCID { get; }

        public abstract string UI_File { get; }
        public abstract string UI_File_Open { get; }
        public abstract string UI_File_RightClickContextMenu { get; }
        public abstract string UI_File_SourceCode { get; }
        public abstract string UI_File_Exit { get; }

        public abstract string UI_Edit { get; }
        public abstract string UI_Edit_RemoveFromList { get; }
        public abstract string UI_Edit_ClearList { get; }
        public abstract string UI_Edit_FixPath { get; }

        public abstract string UI_FastDelete { get; }
        public abstract string UI_Delete { get; }

        public abstract string UI_OpenDialog_Title { get; }
        public abstract string UI_OpenDialog_OKButton { get; }
        public abstract string UI_OpenDialog_CancelButton { get; }
        public abstract string UI_OpenDialog_FastAccess { get; }
        public abstract string UI_OpenDialog_FastAccess_Desktop { get; }
        public abstract string UI_OpenDialog_FastAccess_Document { get; }
        public abstract string UI_OpenDialog_Drives { get; }

        public abstract string Shell_RightClickContextText { get; }

        public abstract string Message_FailToFixPath { get; }
        public abstract string Message_Confirmation { get; }
        public abstract string Message_NotFullyCompleted { get; }

        public void Register() => AppLanguageService.RegisterLanguageProvider(Instance);

    }
}
