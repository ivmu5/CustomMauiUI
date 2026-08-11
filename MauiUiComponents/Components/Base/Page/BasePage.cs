using MauiUiSettings;

namespace MauiUiComponents;

public class BasePage<TLayout> : ContentPage
    where TLayout : View, new()
{
    //public new View Content
    //{
    //    get;
    //    set
    //    {
    //        base.Content = value;
    //    }
    //}

    protected internal ComponentStore _componentStore;

    public readonly Grid _rootLayout;
    public readonly TLayout HostLayout;

    public IOverlayService OverlayService { get; }



    public BasePage(ComponentStore componentStore)
    {
        _componentStore = componentStore;

        _rootLayout = new Grid();

        HostLayout = new TLayout();

        _rootLayout.AddChild(HostLayout);

        Content = _rootLayout;


        OverlayService = new OverlayService(
            _rootLayout,
            componentStore);

        this.ColorBind(
            _componentStore.UiServices,
            x => x.BackgroundColor,
            ColorVariant.Background);
    }
}
