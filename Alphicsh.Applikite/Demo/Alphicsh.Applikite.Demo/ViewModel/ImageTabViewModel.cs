using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Imaging;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class ImageTabViewModel : BaseViewModel
{
    public ImageViewModel Image { get; }
    public ImageViewModel PlaceholderImage { get; }

    public ImageTabViewModel(AppModel model)
    {
        Image = new ImageViewModel(model.ImagePathSource);
        PlaceholderImage = new ImageViewModel(model.PlaceholderImagePathSource);
    }
}
