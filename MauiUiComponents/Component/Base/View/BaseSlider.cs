using System.ComponentModel;
using System.Numerics;

namespace MauiUiComponents;

public class BaseSlider<TValue> : Slider, IValueView<TValue>
    where TValue : INumber<TValue>
{
    public double Step = 1;

    public TValue BoundValue
    {
        get => (TValue)GetValue(BoundValueProperty);
        set => SetValue(BoundValueProperty, value);
    }
    public static readonly BindableProperty BoundValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(TValue),
            typeof(BaseSlider<TValue>),
            TValue.Zero);

    public BaseSlider()
    {
        PropertyChanged += OnValueChanged;
        ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(BoundValue): 
                break;

            case nameof(Value): 
                break;
        }
    }

    private void OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var rounded = Math.Round(e.NewValue / Step) * Step;

        if (Value != rounded)
        {
            var args = new ValueChangedEventArgs(Value, rounded);
            Value = rounded;
            BoundValue = TValue.CreateSaturating(rounded);
        }
    }
}
