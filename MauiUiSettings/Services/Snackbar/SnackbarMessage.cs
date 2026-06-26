namespace MauiUiSettings;

public class SnackbarMessage
{
    public string Message { get; set; } = string.Empty;

    public string ActionText { get; set; } = "OK";

    public Func<Task>? Action { get; set; }

    public SnackbarType Type { get; set; } = SnackbarType.Info;

    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(5);

    public IView? Anchor { get; set; }

    public bool IsPriority { get; set; } = false;
}