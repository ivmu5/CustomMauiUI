using MauiUiSettings;

namespace MauiUiComponents;

public static class TextComponentExtensions
{
    public static T TextStyleTextBind<T>(
        this T view,
        UiServiceStore uiServices)
        where T : BindableObject, ITextComponent
    {
        return view
            .TextStyleBind(uiServices, uiServices.FontService);
    }

    public static T TextStyleIconBind<T>(
        this T view,
        UiServiceStore uiServices)
        where T : BindableObject, ITextComponent
    {
        return view
            .TextStyleBind(uiServices, uiServices.IconService);
    }

    public static T TextStyleBind<T>(
        this T view,
        UiServiceStore uiServices,
        FontVariant fontVariant)
        where T : BindableObject, ITextComponent
    {
        return fontVariant switch
        {
            FontVariant.Text => TextStyleTextBind(view, uiServices),
            FontVariant.Icon => TextStyleIconBind(view, uiServices),
            _ => throw new NotImplementedException()
        };
    }

    public static T TextStyleBind<T, TFontService>(
        this T view,
        UiServiceStore uiServices,
        BaseFontService<TFontService> fontService)
        where T : BindableObject, ITextComponent
    {
        return view
            .TextColorBind(uiServices.ColorService)
            .FontSizeBind(fontService)
            .FontFamilyBind(fontService);
    }



    public static T TextColorBind<T>(
        this T view,
        ColorService colorService)
        where T : BindableObject, ITextComponent
    {
        return view.Bind(
            v => v.TextColor,
            colorService,
            s => s.Text);
    }

    public static T FontSizeBind<T, TService>(
        this T view,
        BaseFontService<TService> fontService)
        where T : BindableObject, ITextComponent
    {
        return view.Bind(
            v => v.FontSize,
            fontService,
            s => s.FontSize);
    }

    public static T FontFamilyBind<T, TService>(
        this T view,
        BaseFontService<TService> fontService)
        where T : BindableObject, ITextComponent
    {
        return view.Bind(
            v => v.FontFamily,
            fontService,
            s => s.FontFamily);
    }

    public static T TextBind<T>(
        this T view,
        ILocalizationResourceManager locManager,
        string key)
        where T : BindableObject, ITextComponent
    {
        view.SetBinding(BindingExtensions.GetBindableProperty<T, string>(v => v.Text),
            new Binding($"[{key}]", source: locManager));

        return view;
    }
}