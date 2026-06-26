using SQLiteStorage;

namespace MauiUiSettings;

public class WindowService : UiServiceBase<WindowService>, IDisposable
{
    public const double SquareTolerance = 50;

    private Window _window { get; set; } = null!;

    public double Width
    {
        get => (double)GetValue(WidthProperty);
        private set => SetValue(WidthProperty, value);
    }
    public static readonly BindableProperty WidthProperty =
        CreateBindableProperty<double>(nameof(Width));

    public double Height
    {
        get => (double)GetValue(HeightProperty);
        private set => SetValue(HeightProperty, value);
    }
    public static readonly BindableProperty HeightProperty =
        CreateBindableProperty<double>(nameof(Height));

    public WindowOrientation Orientation
    {
        get => (WindowOrientation)GetValue(OrientationProperty);
        private set => SetValue(OrientationProperty, value);
    }
    public static readonly BindableProperty OrientationProperty =
        CreateBindableProperty<WindowOrientation>(nameof(Orientation));


    public bool IsVertical
    {
        get => (bool)GetValue(IsVerticalProperty);
        private set => SetValue(IsVerticalProperty, value);
    }
    public static readonly BindableProperty IsVerticalProperty =
        CreateBindableProperty<bool>(nameof(IsVertical));

    public bool IsHorizontal
    {
        get => (bool)GetValue(IsHorizontalProperty);
        private set => SetValue(IsHorizontalProperty, value);
    }
    public static readonly BindableProperty IsHorizontalProperty =
        CreateBindableProperty<bool>(nameof(IsHorizontal));

    public bool IsSquare
    {
        get => (bool)GetValue(IsSquareProperty);
        private set => SetValue(IsSquareProperty, value);
    }
    public static readonly BindableProperty IsSquareProperty =
        CreateBindableProperty<bool>(nameof(IsSquare));



    public WindowService(IInstanceRepository<UISettings> settings)
        : base(settings) { }

    public void Attach(Window window)
    {
        if (_window is not null)
            _window.SizeChanged -= OnWindowSizeChanged;
        _window = window;
        ApplySize(_window.Width, _window.Height);
        window.SizeChanged += OnWindowSizeChanged;
    }



    private void UpdateOrientation(WindowOrientation orientation)
    {
        Orientation = orientation;

        IsVertical = orientation == WindowOrientation.Vertical;
        IsHorizontal = orientation == WindowOrientation.Horizontal;
        IsSquare = orientation == WindowOrientation.Square;
    }

    private void ApplySize(double width, double height)
    {
        if (width == 0 && height == 0)
            return;

        Width = width;
        Height = height;

        UpdateOrientation(GetOrientation(width, height));
    }

    private static WindowOrientation GetOrientation(
        double width,
        double height)
    {
        if (Math.Abs(width - height) < SquareTolerance)
            return WindowOrientation.Square;

        return width > height
            ? WindowOrientation.Horizontal
            : WindowOrientation.Vertical;
    }



    private void OnWindowSizeChanged(object? sender, EventArgs e)
    {
        if (sender is Window window)
            ApplySize(window.Width, window.Height);
    }



    public void Dispose()
    {
        if (_window is not null)
            _window.SizeChanged -= OnWindowSizeChanged;
    }
}
