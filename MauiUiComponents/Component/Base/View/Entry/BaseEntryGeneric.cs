using System.ComponentModel;

namespace MauiUiComponents;

public abstract class BaseEntry<TValue> : BaseEntry, IBindableValue<TValue>
{
    public TValue BindableValue
    {
        get => (TValue)GetValue(BindableValueProperty);
        set => SetValue(BindableValueProperty, value);
    }
    public static readonly BindableProperty BindableValueProperty =
        BindableProperty.Create(nameof(BindableValue), typeof(TValue), typeof(BaseEntry<TValue>));

    private bool _isInternalUpdate;



    public abstract TValue Parse(string text);
    public abstract string Format(TValue value);
    public abstract string Filter(string text);



    public BaseEntry()
    {
        TextChanged += OnTextChanged;
        PropertyChanged += OnValueChanged;
    }

    private void OnValueChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(BindableValue))
            return;

        if (_isInternalUpdate)
            return;

        SafeUpdate(
            () => Text = Format(BindableValue));
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isInternalUpdate)
            return;

        var filteredText = Filter(e.NewTextValue ?? string.Empty);
        if (filteredText != e.NewTextValue)
            SafeUpdate(() => Text = filteredText);

        var newValue = Parse(filteredText);
        if (EqualityComparer<TValue>.Default.Equals(BindableValue, newValue))
            return;

        SafeUpdate(() => BindableValue = newValue);
    }

    private void SafeUpdate(Action action)
    {
        _isInternalUpdate = true;
        try
        {
            action();
        }
        finally
        {
            _isInternalUpdate = false;
        }
    }
}
