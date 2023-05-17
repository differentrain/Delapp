namespace DelApp.Locals
{
    internal interface IAppLanguageProvider
    {
        string TwoLetterISOLanguageName { get; }
        int LCID { get; }

        string UI_File { get; }
        string UI_File_Open { get; }
        string UI_File_RightClickContextMenu { get; }
        string UI_File_SourceCode { get; }
        string UI_File_Exit { get; }

        string UI_Edit { get; }
        string UI_Edit_RemoveFromList { get; }
        string UI_Edit_ClearList { get; }
        string UI_Edit_FixPath { get; }

        string UI_FastDelete { get; }
        string UI_Delete { get; }

        string UI_OpenDialog_Title { get; }
        string UI_OpenDialog_OKButton { get; }
        string UI_OpenDialog_CancelButton { get; }
        string UI_OpenDialog_FastAccess { get; }
        string UI_OpenDialog_FastAccess_Desktop { get; }
        string UI_OpenDialog_FastAccess_Document { get; }
        string UI_OpenDialog_Drives { get; }

        string Shell_RightClickContextText { get; }

        string Message_FailToFixPath { get; }
        string Message_Confirmation { get; }
        string Message_NotFullyCompleted { get; }
    }
}
