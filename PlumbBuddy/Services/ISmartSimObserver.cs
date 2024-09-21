namespace PlumbBuddy.Services;

public interface ISmartSimObserver :
    IDisposable,
    INotifyPropertyChanged
{
    bool IsCurrentlyScanning { get; }
    bool IsModsDisabledGameSettingOn { get; }
    bool IsScriptModsEnabledGameSettingOn { get; }
    IReadOnlyList<ScanIssue> ScanIssues { get; }

    void ClearCache();
    void OpenDownloadsFolder();
    void OpenModsFolder();
}
