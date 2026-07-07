using MauiUiSettings;
using Microsoft.Maui.Layouts;

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

    protected internal UiServiceStore _uiServices;
    protected internal ComponentStore _componentStore;

    private readonly Grid _rootLayout;
    public readonly TLayout HostLayout;

    public IOverlayService OverlayService { get; }



    public BasePage(
        UiServiceStore uiServices,
        ComponentStore componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;

        _rootLayout = new Grid();

        HostLayout = new TLayout();

        _rootLayout.AddChild(HostLayout);

        Content = _rootLayout;


        OverlayService = new OverlayService(
            _rootLayout,
            componentStore);

        this.ColorBind(
            _uiServices,
            x => x.BackgroundColor,
            ColorVariant.Background);
    }

    public void AddChildren(params View[] views)
    {
        switch (HostLayout)
        {
            case Layout layout:
                foreach (var view in views)
                    layout.Add(view);
                break;

            case ScrollView contentView:
                contentView.Content = views.FirstOrDefault();
                break;

            default:
                throw new InvalidOperationException("Unsupported layout type.");
        }
    }
}
