namespace MauiUiComponents;

public static class ViewGridPositionExtensions
{
    public static T GridPosition<T>(
        this T view,
        int row,
        int column)
        where T : View
    {
        Grid.SetRow(view, row);
        Grid.SetColumn(view, column);

        return view;
    }

    public static T GridColumnSpan<T>(
        this T view,
        int span)
        where T : View
    {
        Grid.SetColumnSpan(view, span);

        return view;
    }

    public static T GridRowSpan<T>(
        this T view,
        int span)
        where T : View
    {
        Grid.SetRowSpan(view, span);

        return view;
    }

    public static T GridPosition<T>(
        this T view,
        int row,
        int column,
        int rowSpan = 1,
        int columnSpan = 1)
        where T : View
    {
        return view
            .GridPosition(row, column)
            .GridRowSpan(rowSpan)
            .GridColumnSpan(columnSpan);
    }
}
