using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Tests.Collections;

public class TestModel
{
    public ICollectionSource<TestItemModel> Items { get; } = new CollectionSource<TestItemModel>();
}
