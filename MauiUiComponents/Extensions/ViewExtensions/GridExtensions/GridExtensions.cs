namespace MauiUiComponents;

public static class GridExtensions
{
    public static T AddRow<T>(
        this T grid,
        GridLength height)
        where T : Grid
    {
        grid.RowDefinitions.Add(
            new RowDefinition(height));

        return grid;
    }

    public static T AddColumn<T>(
        this T grid,
        GridLength width)
        where T : Grid
    {
        grid.ColumnDefinitions.Add(
            new ColumnDefinition(width));

        return grid;
    }

    public static T AddRow<T>(
        this T grid,
        double height)
        where T : Grid
    {
        return grid.AddRow(new GridLength(height));
    }

    public static T AddColumn<T>(
        this T grid,
        double width)
        where T : Grid
    {
        return grid.AddColumn(new GridLength(width));
    }



    public static T AddStarRow<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddRow(GridLength.Star);
    }

    public static T AddAutoRow<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddRow(GridLength.Auto);
    }

    public static T AddStarColumn<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddColumn(GridLength.Star);
    }

    public static T AddAutoColumn<T>(
        this T grid)
        where T : Grid
    {
        return grid.AddColumn(GridLength.Auto);
    }



    public static T AddChild<T>(
        this T grid,
        View view,
        int row = 0,
        int column = 0,
        int rowSpan = 0,
        int columnSpan = 0)
        where T : Grid
    {
        grid.AddChild(view);

        view.GridPosition(
            row,
            column,
            rowSpan,
            columnSpan);

        return grid;
    }

    public static T AddChild<T>(
        this T grid,
        View view)
        where T : Grid
    {
        grid.Add(view);

        return grid;
    }
}