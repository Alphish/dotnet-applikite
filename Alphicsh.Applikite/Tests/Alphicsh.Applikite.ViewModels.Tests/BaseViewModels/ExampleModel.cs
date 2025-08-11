using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Tests.BaseViewModels;

public class ExampleModel
{
    public ValueSource<int> ExampleValueSource { get; } = new ValueSource<int>(123);
    public int ExampleValue { get => ExampleValueSource.Value; set => ExampleValueSource.Value = value; }
}
