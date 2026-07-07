using MauiUiSettings.Resources.Localization.MaterialSymbols;
using System.Numerics;

namespace MauiUiComponents;

public class CustomTextSlider<TValue> : BaseGrid, IBindableValue<TValue>
    where TValue : INumber<TValue>, IMinMaxValue<TValue>
{
    private readonly ComponentStore _componentStore;

    public readonly ValueSynchronizer<TValue> ValueSynchronizer;

    public BaseLabel TextLabel { get; private set; }
    public BaseBorder<BaseEntry<TValue>> ValueEntryBorder { get; private set; }
    public BaseSlider<TValue> ValueSlider { get; private set; }

    public BaseButton? ResetButton { get; private set; }
    public TValue? DefaultValue { get; private set; }

    public TValue MinValue
    {
        get;
        set
        {
            if (value > MaxValue)
                value = MaxValue;

            field = value;

            ValueSlider.Minimum = Convert.ToDouble(field);

            if (BindableValue < field)
                BindableValue = field;
        }
    }
    public TValue MaxValue
    {
        get;
        set
        {
            if (value < MinValue)
                value = MinValue;

            field = value;

            ValueSlider.Maximum = Convert.ToDouble(field);

            if (BindableValue > field)
                BindableValue = field;
        }
    }
    public TValue BindableValue
    {
        get => (TValue)GetValue(BindableValueProperty);
        set => SetValue(BindableValueProperty, value);
    }

    public static readonly BindableProperty BindableValueProperty =
        BindableProperty.Create(
            nameof(BindableValue),
            typeof(TValue),
            typeof(CustomTextSlider<TValue>),
            TValue.Zero,
            propertyChanged: OnBindableValueChanged);

    private static void OnBindableValueChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (CustomTextSlider<TValue>)bindable;
        var value = (TValue)newValue;

        var clamped = TValue.Clamp(
            value,
            control.MinValue,
            control.MaxValue);

        if (EqualityComparer<TValue>.Default.Equals(value, clamped))
            return;

        control.BindableValue = clamped;
    }



    public CustomTextSlider(ComponentStore componentStore)
    {
        _componentStore = componentStore;

        ValueSlider = _componentStore.Base.Slider<TValue>();
        TextLabel ??= _componentStore.Base.Label();
        ValueEntryBorder = _componentStore.Base.Entry<TValue>().WithBorder(componentStore);

        BindableValue = TValue.Zero;
        MinValue = TValue.MinValue;
        MaxValue = TValue.MaxValue;

        BuildLayout();

        ValueSynchronizer = new ValueSynchronizer<TValue>(
            new List<IBindableValue<TValue>>
            {
                this,
                ValueEntryBorder.View,
                ValueSlider
            });
    }

    private void BuildLayout()
    {
        TextLabel.ViewCenter().TextCenter();

        ValueEntryBorder.HorizontalOptions = LayoutOptions.Fill;
        ValueEntryBorder.View.TextCenter();

        this.AddColumn(50);
        this.AddStarColumn();
        this.For(2, (g, i) => g.AddAutoRow());

        this.AddChild(TextLabel, 0, 0, 0, 3);
        this.AddChild(ValueEntryBorder, 1, 0);
        this.AddChild(ValueSlider, 1, 1);
    }

    public void AddResetValueButton(TValue resetValue)
    {
        if (ResetButton != null)
            throw new InvalidOperationException("Reset button already exists.");

        DefaultValue = resetValue;

        ResetButton = _componentStore.Base
            .Button(fontVariant: MauiUiSettings.FontVariant.Icon)
            .TextBind(
                _componentStore.ResourcesStore.MaterialSymbolsManager,
                nameof(MaterialSymbols.Refresh));
        ResetButton.Clicked +=
            (s, e) => BindableValue = DefaultValue;


        this.AddAutoColumn();
        this.AddChild(ResetButton.WithBorder(_componentStore), 1, 2);
    }

    public void SetRange(TValue min, TValue max)
    {
        MinValue = min;
        MaxValue = max;
    }
}