using ToDover2.Models;

namespace ToDover2.Interfaces;

public interface ILoadingStepExecutor
{
    void Execute(LoadingStep step);
}
