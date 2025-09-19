using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Alphicsh.Applikite.Controls;

public class ImpImage : Image
{
    public static readonly new StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImpImage, IImage?>(nameof(Source), defaultValue: null);

    public static readonly StyledProperty<IImage?> PlaceholderProperty =
        AvaloniaProperty.Register<ImpImage, IImage?>(nameof(Placeholder), defaultValue: null);

    static ImpImage()
    {
        SourceProperty.Changed.AddClassHandler<ImpImage>(OnImageChanged);
        PlaceholderProperty.Changed.AddClassHandler<ImpImage>(OnImageChanged);
    }

    public new IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public IImage? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private static void OnImageChanged(ImpImage target, AvaloniaPropertyChangedEventArgs _)
    {
        var baseTarget = target as Image;
        var currentSource = baseTarget.Source;
        var newSource = target.Source ?? target.Placeholder;
        if (currentSource == newSource)
            return;

        baseTarget.Source = newSource;
    }
}
