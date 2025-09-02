using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Collections;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class ItemViewModel : BaseViewModel<ItemModel>, IItemViewModel<ItemModel, ItemViewModel>
{
    public ItemViewModel(ItemModel model) : base(model)
    {
        NameProperty = RelayProperty(nameof(Name), model.NameSource);
        DescriptionProperty = RelayProperty(nameof(Description), model.DescriptionSource);
    }

    public RelayViewModelProperty<string> NameProperty { get; }
    public string Name { get => NameProperty.Value; set => NameProperty.Value = value; }

    public RelayViewModelProperty<string> DescriptionProperty { get; }
    public string Description { get => DescriptionProperty.Value; set => DescriptionProperty.Value = value; }

    public static ItemViewModel FromModel(ItemModel model)
        => new ItemViewModel(model);
}
