using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.Demo.ViewModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Alphicsh.Applikite.Demo;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var model = new AppModel();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new AppViewModel(model)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}