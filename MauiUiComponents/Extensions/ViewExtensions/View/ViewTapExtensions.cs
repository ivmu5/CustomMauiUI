namespace MauiUiComponents;

public static class ViewTapExtensions
{
    public static T ViewOnTapped<T>(
        this T view, 
        Action<View> startAction,
        Action<View>? finishAction = null)
        where T : View
    {
        var existing = view.GestureRecognizers
            .OfType<TapGestureRecognizer>()
            .FirstOrDefault();

        if (existing != null)
            view.GestureRecognizers.Remove(existing);

        var tap = new TapGestureRecognizer();

        tap.Tapped += async (_, _) =>
        {
            startAction(view);
            await Task.Yield(); // даём UI завершить обработку
            finishAction?.Invoke(view);
        };

        view.GestureRecognizers.Add(tap);
        return view;
    }
}
