using ToDover2.Interfaces;
using ToDover2.Models;

namespace ToDover2.Services;

public sealed class LoadingStepExecutor : ILoadingStepExecutor
{
    private readonly Random _random = new();

    public void Execute(LoadingStep step)
    {
        Console.WriteLine(step.Message);
        Thread.Sleep(step.DelayMilliseconds);

        if (_random.Next(0, 4) == 0)
        {
            throw new Exception(step.ErrorMessage);
        }
    }
}
