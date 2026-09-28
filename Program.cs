using ToDover2.Interfaces;
using ToDover2.Repositories;
using ToDover2.Services;

namespace ToDover2;

internal static class Program
{
    private static void Main()
    {
        ILoadingStepRepository stepRepository = new LoadingStepRepository();
        ILoadingStepExecutor stepExecutor = new LoadingStepExecutor();
        var loader = new ApplicationLoader(stepRepository, stepExecutor);

        loader.Run();
    }
}
