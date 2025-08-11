using Shouldly;

namespace Alphicsh.Applikite.ViewModels.Tests.BaseViewModels;

public class BaseViewModelTests
{
    private ExampleViewModel ViewModel { get; }
    private List<string> ReceivedPropertyChanges { get; } = new List<string>();
    private int ReceivedValue { get; set; } = default;

    public BaseViewModelTests()
    {
        var model = new ExampleModel();
        ViewModel = new ExampleViewModel(model);
        ViewModel.PropertyChanged += (sender, e) =>
        {
            ReceivedPropertyChanges.Add(e.PropertyName!);
            ReceivedValue = ViewModel.ExampleValue;
        };
    }

    [Fact]
    public void ExampleValue_ShouldHaveCorrectInitialValue()
    {
        ViewModel.ExampleValue.ShouldBe(123);
    }

    [Fact]
    public void ExampleValue_ShouldProcessPropertyChange()
    {
        ViewModel.ExampleValue = 456;

        ReceivedPropertyChanges.ShouldHaveSingleItem();
        ReceivedPropertyChanges.ShouldContain(nameof(ViewModel.ExampleValue));
        ReceivedValue.ShouldBe(456);
    }

    [Fact]
    public void ExampleValue_ShouldNotProcessPropertyChangeAfterDisposal()
    {
        ViewModel.Dispose();

        ViewModel.ExampleValue = 456;

        ReceivedPropertyChanges.ShouldBeEmpty();
        ReceivedValue.ShouldBe(default);
    }
}
