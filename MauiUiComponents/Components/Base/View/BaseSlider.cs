using System.Numerics;

namespace MauiUiComponents;

public class BaseSlider<TValue> : Slider, IBindableValue<TValue>
    where TValue : INumber<TValue>
{
    private bool _isInternalUpdate;

    public double Step
    {
        get;
        set
        {
            if (value <= 0)
                value = 1;
            field = value;
        }
    } = 1;

    public TValue BindableValue
    {
        get => (TValue)GetValue(BindableValueProperty);
        set => SetValue(BindableValueProperty, value);
    }
    public static readonly BindableProperty BindableValueProperty =
        BindableProperty.Create(
            nameof(BindableValue),
            typeof(TValue),
            typeof(BaseSlider<TValue>),
            default(TValue),
            propertyChanged: OnBindableValueChanged);

    private static void OnBindableValueChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var slider = (BaseSlider<TValue>)bindable;

        if (slider._isInternalUpdate)
            return;

        slider._isInternalUpdate = true;
        var normalizedValue = slider.Normalize((TValue)newValue);
        slider.Value = Convert.ToDouble(normalizedValue);
        slider._isInternalUpdate = false;
    }



    public BaseSlider()
    {
        ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        if (_isInternalUpdate)
            return;

        var normalizedValue = Normalize(TValue.CreateSaturating(e.NewValue));
        var rounded = Convert.ToDouble(normalizedValue);

        _isInternalUpdate = true;

        if (e.NewValue != rounded)
            Value = rounded;

        if (!EqualityComparer<TValue>.Default.Equals(BindableValue, normalizedValue))
            BindableValue = normalizedValue;

        _isInternalUpdate = false;
    }

    private TValue Normalize(TValue value)
    {
        double rounded =
            Math.Round(Convert.ToDouble(value) / Step) * Step;

        return TValue.CreateSaturating(rounded);
    }
}
