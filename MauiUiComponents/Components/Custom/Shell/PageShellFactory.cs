namespace MauiUiComponents;

public class PageShellFactory
{
    public readonly Func<ContentPage> PageFactory;
    public readonly Func<IToggleItem> PageButtonFactory;

    public PageShellFactory(
        Func<ContentPage> pageFactory,
        Func<IToggleItem> pageButtonFactory)
    {
        PageFactory = pageFactory;
        PageButtonFactory = pageButtonFactory;
    }
}
