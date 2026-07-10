using MauiUiSettings;
using System.Numerics;

namespace MauiUiComponents;

public class BaseComponentStore
{
    private readonly UiServiceStore _uiServices;

    public BaseComponentStore(UiServiceStore uiServices)
    {
        _uiServices = uiServices;
    }

    private T BaseBind<T>(
        T view,
        ColorVariant backgroundColor,
        FontVariant fontVariant)
        where T : View, ITextComponent
    {
        view
            .ColorBackgroundBind(_uiServices, backgroundColor)
            .TextStyleBind(_uiServices, fontVariant);

        return view;
    }

    public BaseEntry Entry(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
        => BaseBind(new BaseEntry(), backgroundColor, fontVariant);

    public BaseEntry<TValue> Entry<TValue>(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
        => BaseBind(EntryFactory.Create<TValue>(), backgroundColor, fontVariant);



    public BaseLabel Label(
        ColorVariant backgroundColor = ColorVariant.None,
        FontVariant fontVariant = FontVariant.Text)
        => BaseBind(new BaseLabel(), backgroundColor, fontVariant);

    public BaseButton Button(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
        => BaseBind(new BaseButton(), backgroundColor, fontVariant);

    public BaseEditor Editor(
        ColorVariant backgroundColor = ColorVariant.Secondary,
        FontVariant fontVariant = FontVariant.Text)
        => BaseBind(new BaseEditor(), backgroundColor, fontVariant);
    


    public BaseBorder<T> Border<T>(
        T view,
        ColorVariant strokeColor = ColorVariant.Primary,
        ColorVariant backgroundColor = ColorVariant.None)
        where T : View
    {
        var border = new BaseBorder<T>(view);
        return border
            .ColorBackgroundBind(_uiServices, backgroundColor)
            .ColorBind(_uiServices, x => x.Stroke, strokeColor)
            .BorderRoundRectangleBind(_uiServices);
    }

    public BaseSlider<TValue> Slider<TValue>(
        ColorVariant minimumTrackColor = ColorVariant.Primary,
        ColorVariant maximumTrackColor = ColorVariant.Secondary,
        ColorVariant thumbColor = ColorVariant.Primary)
        where TValue : INumber<TValue>
    {
        var slider = new BaseSlider<TValue>();
        return slider
            .ColorBind(_uiServices, x => x.MinimumTrackColor, minimumTrackColor)
            .ColorBind(_uiServices, x => x.MaximumTrackColor, maximumTrackColor)
            .ColorBind(_uiServices, x => x.ThumbColor, thumbColor);
    }
}
