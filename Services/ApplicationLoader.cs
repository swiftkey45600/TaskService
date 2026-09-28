using ToDover2.Interfaces;

namespace ToDover2.Services;

public sealed class ApplicationLoader
{
    private readonly ILoadingStepRepository _stepRepository;
    private readonly ILoadingStepExecutor _stepExecutor;

    public ApplicationLoader(
        ILoadingStepRepository stepRepository,
        ILoadingStepExecutor stepExecutor)
    {
        _stepRepository = stepRepository;
        _stepExecutor = stepExecutor;
    }

    public void Run()
    {
        bool loadingSucceeded = true;

        Task showSplashTask = Task.Run(() =>
        {
            _stepExecutor.Execute(_stepRepository.ShowSplash);
        });

        Task showSplashErrorTask = showSplashTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task requestLicenseTask = showSplashTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.RequestLicense),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task requestLicenseErrorTask = requestLicenseTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task setupMenusTask = requestLicenseTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.SetupMenus),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task setupMenusErrorTask = setupMenusTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task checkForUpdateTask = showSplashTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.CheckForUpdate),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task checkForUpdateErrorTask = checkForUpdateTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task downloadUpdateTask = checkForUpdateTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.DownloadUpdate),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task downloadUpdateErrorTask = downloadUpdateTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task allMainBranchesTask = Task.WhenAll(setupMenusTask, downloadUpdateTask);

        Task displayWelcomeTask = allMainBranchesTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.DisplayWelcome),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task displayWelcomeErrorTask = displayWelcomeTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task finalErrorTask = allMainBranchesTask.ContinueWith(
            failedTask =>
            {
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task hideSplashTask = displayWelcomeTask.ContinueWith(
            previousTask => _stepExecutor.Execute(_stepRepository.HideSplash),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task hideSplashErrorTask = hideSplashTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        try
        {
            Task.WaitAll(
                hideSplashTask,
                showSplashErrorTask,
                requestLicenseErrorTask,
                setupMenusErrorTask,
                checkForUpdateErrorTask,
                downloadUpdateErrorTask,
                displayWelcomeErrorTask,
                hideSplashErrorTask,
                finalErrorTask
            );
        }
        catch
        {
            loadingSucceeded = false;
        }

        Console.WriteLine();

        if (loadingSucceeded && hideSplashTask.Status == TaskStatus.RanToCompletion)
        {
            Console.WriteLine("Программа успешно загрузилась.");
        }
        else
        {
            Console.WriteLine("Программа загрузилась с ошибками.");
        }
    }

    private static void HandleError(Task task)
    {
        if (task.IsFaulted)
        {
            Console.WriteLine(task.Exception?.InnerException?.Message);
            var ignored = task.Exception;
        }
    }
}
