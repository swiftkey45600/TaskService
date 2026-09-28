namespace ToDover2.Models;

public sealed record LoadingStep(string Message, int DelayMilliseconds, string ErrorMessage);
