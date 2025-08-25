namespace Alphicsh.Applikite.Tasks.Progress;

public struct IntegerProgress : IProgressData<IntegerProgress>
{
    public required int Current { get; init; }
    public required int Target { get; init; }

    public static IntegerProgress CreateDefault()
        => new IntegerProgress { Current = 0, Target = 1 };
}
