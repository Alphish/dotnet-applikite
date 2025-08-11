using System.ComponentModel;

namespace Alphicsh.Applikite.ViewModels;

public interface IViewModel : INotifyPropertyChanged, IDisposable
{
    void RaisePropertyChanged(string propertyName);
}

