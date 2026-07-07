using MauiUiSettings;
using SQLiteStorage;

namespace MauiUiComponents;

public class ToggleView<TView> : IDisposable
    where TView : View
{
    private readonly UiServiceStore _uiServices;

    public TView View { get; init; }
    public List<ToggleViewStyle<TView>> Styles { get; init; }
    private ToggleGrid? _parent { get; set; }



    public ToggleView(
        TView view,
        UiServiceStore uiServices,
        List<ToggleViewStyle<TView>>? styles = null)
    {
        _uiServices = uiServices;
        View = view;
        Styles = styles ?? new();
    }

    public void AttachToParent(ToggleGrid parent)
    {
        Dispose();

        _parent = parent;
        _parent.SelectedChanged += OnParentSelectedChanged;
        UpdateView(_parent.IsSelected);
    }

    private void OnParentSelectedChanged(object? sender, ValueChangedEventArgs<bool> e)
    {
        UpdateView(e.NewValue);
    }

    private void UpdateView(bool isSelected)
    {
        foreach (var style in Styles)
        {
            View.ColorBind(
                _uiServices,
                style.PropertyExpression,
                isSelected
                    ? style.SelectedColor
                    : style.UnselectedColor);
        }
    }

    public void Dispose()
    {
        if (_parent != null)
        {
            _parent.SelectedChanged -= OnParentSelectedChanged;
            _parent = null;
        }
    }
}
