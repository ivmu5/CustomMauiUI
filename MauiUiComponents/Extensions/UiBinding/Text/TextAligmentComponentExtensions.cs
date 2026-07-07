namespace MauiUiComponents;

public static class TextAligmentComponentExtensions
{
    public static T TextCenter<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.TextCenterVertical()
            .TextCenterHorizontal();
        return view;
    }

    public static T TextCenterHorizontal<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment = TextAlignment.Center;
        return view;
    }

    public static T TextCenterVertical<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment = TextAlignment.Center;
        return view;
    }



    public static T TextLeft<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment = TextAlignment.Start;
        return view;
    }

    public static T TextRight<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment = TextAlignment.End;
        return view;
    }

    public static T TextFillHorizontal<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.HorizontalTextAlignment = TextAlignment.Justify;
        return view;
    }



    public static T TextTop<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment = TextAlignment.Start;
        return view;
    }

    public static T TextBottom<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment = TextAlignment.End;
        return view;
    }

    public static T TextFillVertical<T>(this T view)
        where T : ITextAlignmentComponent
    {
        view.VerticalTextAlignment = TextAlignment.Justify;
        return view;
    }
}
