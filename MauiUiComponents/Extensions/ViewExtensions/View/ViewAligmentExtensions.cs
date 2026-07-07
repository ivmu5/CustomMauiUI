namespace MauiUiComponents;

public static class ViewAlignmentExtensions
{
    public static T ViewAlignment<T>(
        this T view,
        LayoutOptions horizontal,
        LayoutOptions vertical)
        where T : View
    {
        return view
            .ViewHorizontalOptions(horizontal)
            .ViewVerticalOptions(vertical);
    }

    public static T ViewHorizontalOptions<T>(
        this T view,
        LayoutOptions horizontal)
        where T : View
    {
        view.HorizontalOptions = horizontal;
        return view;
    }

    public static T ViewVerticalOptions<T>(
        this T view,
        LayoutOptions vertical)
        where T : View
    {
        view.VerticalOptions = vertical;
        return view;
    }



    public static T ViewCenter<T>(this T view) where T : View
        => view
            .ViewHorizontalCenter()
            .ViewVerticalCenter();

    public static T ViewHorizontalCenter<T>(this T view) where T : View
        => view.ViewHorizontalOptions(LayoutOptions.Center);

    public static T ViewVerticalCenter<T>(this T view) where T : View
        => view.ViewVerticalOptions(LayoutOptions.Center);



    public static T ViewFillHorizontal<T>(this T view) where T : View
        => view.ViewHorizontalOptions(LayoutOptions.Fill);

    public static T ViewFillVertical<T>(this T view) where T : View
        => view.ViewVerticalOptions(LayoutOptions.Fill);

    public static T ViewFillBoth<T>(this T view) where T : View
        => view
            .ViewFillHorizontal()
            .ViewFillVertical();



    public static T ViewHorizontalStart<T>(this T view) where T : View
        => view.ViewHorizontalOptions(LayoutOptions.Start);

    public static T ViewHorizontalEnd<T>(this T view) where T : View
        => view.ViewHorizontalOptions(LayoutOptions.End);

    public static T ViewVerticalStart<T>(this T view) where T : View
        => view.ViewVerticalOptions(LayoutOptions.Start);

    public static T ViewVerticalEnd<T>(this T view) where T : View
        => view.ViewVerticalOptions(LayoutOptions.End);

    public static T ViewTopLeft<T>(this T view) where T : View
        => view
            .ViewHorizontalStart()
            .ViewVerticalStart();

    public static T ViewTopRight<T>(this T view) where T : View
        => view
            .ViewHorizontalEnd()
            .ViewVerticalStart();

    public static T ViewBottomLeft<T>(this T view) where T : View
        => view
            .ViewHorizontalStart()
            .ViewVerticalEnd();

    public static T ViewBottomRight<T>(this T view) where T : View
        => view
            .ViewHorizontalEnd()
            .ViewVerticalEnd();
}