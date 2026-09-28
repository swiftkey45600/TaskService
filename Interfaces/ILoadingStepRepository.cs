using ToDover2.Models;

namespace ToDover2.Interfaces;

public interface ILoadingStepRepository
{
    LoadingStep ShowSplash { get; }
    LoadingStep RequestLicense { get; }
    LoadingStep CheckForUpdate { get; }
    LoadingStep SetupMenus { get; }
    LoadingStep DownloadUpdate { get; }
    LoadingStep DisplayWelcome { get; }
    LoadingStep HideSplash { get; }
}
