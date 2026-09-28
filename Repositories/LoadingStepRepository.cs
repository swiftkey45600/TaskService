using ToDover2.Interfaces;
using ToDover2.Models;

namespace ToDover2.Repositories;

public sealed class LoadingStepRepository : ILoadingStepRepository
{
    public LoadingStep ShowSplash { get; } = new(
        "Show Splash", 500, "Ошибка: не удалось отобразить заставку.");

    public LoadingStep RequestLicense { get; } = new(
        "Request License", 500, "Ошибка: лицензия не найдена.");

    public LoadingStep CheckForUpdate { get; } = new(
        "Check for Update", 500, "Ошибка: нет доступа в сеть.");

    public LoadingStep SetupMenus { get; } = new(
        "Setup Menus", 500, "Ошибка: не удалось загрузить меню.");

    public LoadingStep DownloadUpdate { get; } = new(
        "Download Update", 1200, "Ошибка: не удалось скачать обновление.");

    public LoadingStep DisplayWelcome { get; } = new(
        "Display Welcome", 700, "Ошибка: не удалось отобразить приветствие.");

    public LoadingStep HideSplash { get; } = new(
        "Hide Splash", 500, "Ошибка: не удалось скрыть заставку.");
}
