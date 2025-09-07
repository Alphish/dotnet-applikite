using System.ComponentModel;
using Alphicsh.Applikite.ViewModels.Collections;

namespace Alphicsh.Applikite.ViewModels.Tests.Collections;

public class TestItemViewModel : IItemViewModel<TestItemModel, TestItemViewModel>
{
    public TestItemModel Model { get; }
    public bool IsDisposed { get; private set; } = false;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string UpperName => Model.Name.ToUpperInvariant();
    public string UpperDescription => Model.Description.ToUpperInvariant();

    public TestItemViewModel(TestItemModel model)
    {
        Model = model;
    }

    public static TestItemViewModel FromModel(TestItemModel model)
        => new TestItemViewModel(model);

    public void RaisePropertyChanged(string propertyName)
    {
        // not relevant
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        IsDisposed = true;
    }
}
