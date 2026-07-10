namespace MauiUiComponents;

public class ViewTemplate<T>
{
    private readonly Func<T, View> _factory;

    public ViewTemplate(Func<T, View> factory)
    {
        _factory = factory;
    }

    public DataTemplate ToDataTemplate()
    {
        return new DataTemplate(() =>
        {
            var host = new ContentView();

            host.BindingContextChanged += (_, _) =>
            {
                if (host.BindingContext is T item)
                    host.Content = _factory(item);
            };

            return host;
        });
    }
}