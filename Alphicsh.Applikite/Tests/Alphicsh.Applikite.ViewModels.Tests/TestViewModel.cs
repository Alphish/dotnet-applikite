using System.ComponentModel;

namespace Alphicsh.Applikite.ViewModels.Tests;

public class TestViewModel : IViewModel
{
    private List<string> ReceivedPropertiesList { get; } = new List<string>();
    public IReadOnlyCollection<string> ReceivedProperties => ReceivedPropertiesList;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void RaisePropertyChanged(string propertyName)
    {
        ReceivedPropertiesList.Add(propertyName);
    }
}
