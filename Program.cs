static class Program
{
    static void ShowSplash()
    {
        Console.WriteLine("Show Splash");
        Thread.Sleep(500);
        MaybeThrowException("Ошибка: не удалось отобразить заставку.");
    }

    static void RequestLicense()
    {
        Console.WriteLine("Request License");
        Thread.Sleep(500);
        MaybeThrowException("Ошибка: лицензия не найдена.");
    }

    static void CheckForUpdate()
    {
        Console.WriteLine("Check for Update");
        Thread.Sleep(500);
        MaybeThrowException("Ошибка: нет доступа в сеть.");
    }

    static void SetupMenus()
    {
        Console.WriteLine("Setup Menus");
        Thread.Sleep(500);
        MaybeThrowException("Ошибка: не удалось загрузить меню.");
    }

    static void DownloadUpdate()
    {
        Console.WriteLine("Download Update");
        Thread.Sleep(1200);
        MaybeThrowException("Ошибка: не удалось скачать обновление.");
    }

    static void DisplayWelcome()
    {
        Console.WriteLine("Display Welcome");
        Thread.Sleep(700);
        MaybeThrowException("Ошибка: не удалось отобразить приветствие.");
    }

    static void HideSplash()
    {
        Console.WriteLine("Hide Splash");
        Thread.Sleep(500);
        MaybeThrowException("Ошибка: не удалось скрыть заставку.");
    }

    static void MaybeThrowException(string message)
    {
        int value = Random.Next(0, 4);
        if (value == 0)
        {
            throw new Exception(message);
        }
    }

    static void HandleError(Task task)
    {
        if (task.IsFaulted)
        {
            Console.WriteLine(task.Exception?.InnerException?.Message);
            var ignored = task.Exception;
        }
    }

    private static readonly Random Random = new Random();

    static void Main(string[] args)
    {
        bool loadingSucceeded = true;

        Task showSplashTask = Task.Run(() =>
        {
            ShowSplash();
        });

        Task showSplashErrorTask = showSplashTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task requestLicenseTask = showSplashTask.ContinueWith(
            previousTask => RequestLicense(),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task requestLicenseErrorTask = requestLicenseTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task setupMenusTask = requestLicenseTask.ContinueWith(
            previousTask => SetupMenus(),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task setupMenusErrorTask = setupMenusTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task checkForUpdateTask = showSplashTask.ContinueWith(
            previousTask => CheckForUpdate(),
            TaskContinuationOptions.OnlyOnRanToCompletion);

        Task checkForUpdateErrorTask = checkForUpdateTask.ContinueWith(
            failedTask =>
            {
                HandleError(failedTask);
                loadingSucceeded = false;
            },
            TaskContinuationOptions.OnlyOnFaulted);

        Task downloadUpdateTask = checkForUpdateTask.ContinueWith(
            previousTask => DownloadUpdate(),
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
            previousTask => DisplayWelcome(),
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
            previousTask => HideSplash(),
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
}