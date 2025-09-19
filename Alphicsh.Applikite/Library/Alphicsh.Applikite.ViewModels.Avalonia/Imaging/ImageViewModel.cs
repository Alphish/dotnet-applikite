using System.ComponentModel;
using Alphicsh.Applikite.Models;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Alphicsh.Applikite.ViewModels.Imaging;

public class ImageViewModel : IViewModel
{
    private IValueSource<string> PathSource { get; }
    public string Path { get => PathSource.Value; set => PathSource.Value = value; }

    private Bitmap? Bitmap { get; set; }
    public IImage? Source => Bitmap;
    public IImageBrushSource? BrushSource => Bitmap;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ImageViewModel(IValueSource<string> pathSource)
    {
        PathSource = pathSource;
        PathSource.ValueChanged += OnPathChanged;
        Bitmap = ResolveImage();
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnPathChanged(object? sender, ValueChangedEventArgs<string> e)
    {
        RaisePropertyChanged(nameof(Path));

        var previousBitmap = Bitmap;
        previousBitmap?.Dispose();
        Bitmap = ResolveImage();
        if (previousBitmap != Bitmap)
            RaisePropertyChanged(nameof(Source));
    }

    private Bitmap? ResolveImage()
    {
        if (Path == null || !File.Exists(Path))
            return null;

        try
        {
            return new Bitmap(Path);
        }
        catch
        {
            return null;
        }
    }

    public void Dispose()
    {
        PathSource.ValueChanged -= OnPathChanged;
        Bitmap?.Dispose();
    }
}
