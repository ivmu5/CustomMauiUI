using System.ComponentModel;

namespace MauiUiComponents;

public abstract class BaseEntry<TValue> : BaseEntry, IValueView<TValue>
{
    public TValue Value
    {
        get => (TValue)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(TValue), typeof(BaseEntry<TValue>));

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
        if (e.PropertyName != nameof(Value))
            return;

        if (_isInternalUpdate)
            return;

        SafeUpdate(
            () => Text = Format(Value));
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isInternalUpdate)
            return;

        var filteredText = Filter(e.NewTextValue ?? string.Empty);
        if (filteredText != e.NewTextValue)
            SafeUpdate(() => Text = filteredText);

        var newValue = Parse(filteredText);
        if (EqualityComparer<TValue>.Default.Equals(Value, newValue))
            return;

        SafeUpdate(() => Value = newValue);
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
