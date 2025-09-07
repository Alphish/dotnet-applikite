using Alphicsh.Applikite.ViewModels.Collections;

namespace Alphicsh.Applikite.ViewModels.Tests.Collections;

public class TestViewModel : BaseViewModel<TestModel>
{
    public TestViewModel(TestModel model) : base(model)
    {
        Items = RelayCollection<TestItemModel, TestItemViewModel>(model.Items);
    }

    public ICollectionViewModel<TestItemModel, TestItemViewModel> Items { get; }
}
