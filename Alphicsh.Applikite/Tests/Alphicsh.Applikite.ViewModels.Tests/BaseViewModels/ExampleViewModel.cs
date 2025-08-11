using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.ViewModels.Tests.BaseViewModels;

public class ExampleViewModel : BaseViewModel
{
    public ExampleViewModel(ExampleModel model)
    {
        Model = model;
        ExampleValueProperty = RelayProperty(nameof(ExampleValue), Model.ExampleValueSource);
    }

    public ExampleModel Model { get; }

    public RelayViewModelProperty<int> ExampleValueProperty { get; }
    public int ExampleValue { get => ExampleValueProperty.Value; set => ExampleValueProperty.Value = value; }
}
