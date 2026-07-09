//using MauiUiSettings;

//namespace MauiUiComponents;

//public class LanguageDropdown : CustomDropdown<SupportedLanguage>
//{
//    private readonly UiServiceStore _uiServices;
//    private readonly ComponentStore _comoponentStore;

//    public LanguageDropdown(
//        UiServiceStore uiServices,
//        ComponentStore componentStore)
//        : base(
//            )
//    {
//        this.ViewAddShadow();
//        this.ViewSetBorderRadius(10);
//        this.ViewSetBackgroundColor(ColorVariant.Background);
//    }

//    private static BaseLabel GetItemTempelate(ComponentStore componentStore, SupportedLanguage language)
//    {
//        var label = componentStore.Base.Label();
//        label.TextBind(componentStore.ResourcesStore.SettingsLocalization,);
//        return label;
//    }
//}









//public class ViewTemplate<T>
//{
//    private readonly Func<T, View> _factory;

//    public ViewTemplate(Func<T, View> factory)
//    {
//        _factory = factory;
//    }

//    public DataTemplate ToDataTemplate()
//    {
//        return new DataTemplate(() =>
//        {
//            var host = new ContentView();

//            host.BindingContextChanged += (_, _) =>
//            {
//                if (host.BindingContext is T item)
//                    host.Content = _factory(item);
//            };

//            return host;
//        });
//    }
//}