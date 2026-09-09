using System.Numerics;

namespace MauiUiComponents;

/// <summary>
/// Типизированная обёртка над стандартным <see cref="Slider"/>,
/// предоставляющая значение типа <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">
/// Числовой тип значения слайдера.
/// </typeparam>
public class BaseSlider<TValue> :
    Slider,
    IBindableValue<TValue>
    where TValue :
        INumber<TValue>,
        IMinMaxValue<TValue>
{
    #region Fields

    private double _step = 1d;

    private bool _isInternalUpdate;

    #endregion

    #region Properties

    /// <summary>
    /// Получает или устанавливает шаг изменения значения слайдера.
    /// </summary>
    /// <remarks>
    /// Шаг хранится как <see cref="double"/>,
    /// поскольку стандартный MAUI <see cref="Slider"/>
    /// работает с этим числовым типом.
    /// </remarks>
    public double Step
    {
        get =>
            _step;

        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Шаг слайдера должен быть больше нуля.");
            }

            if (_step.Equals(value))
                return;

            _step =
                value;

            NormalizeCurrentValue();
        }
    }

    /// <summary>
    /// Получает или устанавливает типизированное
    /// значение слайдера.
    /// </summary>
    public TValue BindableValue
    {
        get =>
            (TValue)GetValue(
                BindableValueProperty);

        set =>
            SetValue(
                BindableValueProperty,
                value);
    }

    /// <summary>
    /// Bindable-свойство для <see cref="BindableValue"/>.
    /// </summary>
    public static readonly BindableProperty BindableValueProperty =
        BindableProperty.Create(
            nameof(BindableValue),
            typeof(TValue),
            typeof(BaseSlider<TValue>),
            TValue.Zero,
            BindingMode.TwoWay,
            propertyChanged: OnBindableValueChanged);

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт типизированный слайдер
    /// и подключает синхронизацию с нативным значением MAUI Slider.
    /// </summary>
    public BaseSlider()
    {
        ValueChanged +=
            OnSliderValueChanged;
    }

    #endregion

    #region Value Synchronization

    /// <summary>
    /// Обрабатывает изменение типизированного
    /// <see cref="BindableValue"/> и передаёт его в MAUI Slider.
    /// </summary>
    private static void OnBindableValueChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        if (Equals(
                oldValue,
                newValue))
        {
            return;
        }

        var slider =
            (BaseSlider<TValue>)bindable;

        slider.UpdateFromBindableValue(
            (TValue)newValue);
    }

    /// <summary>
    /// Преобразует типизированное значение в <see cref="double"/>,
    /// нормализует его и обновляет значение базового Slider.
    /// </summary>
    private void UpdateFromBindableValue(
        TValue value)
    {
        if (_isInternalUpdate)
            return;

        var doubleValue =
            double.CreateChecked(
                value);

        var normalizedValue =
            Normalize(
                doubleValue);

        SafeUpdate(
            () =>
            {
                Value =
                    normalizedValue;

                var normalizedTypedValue =
                    TValue.CreateChecked(
                        normalizedValue);

                if (!EqualityComparer<TValue>.Default.Equals(
                        BindableValue,
                        normalizedTypedValue))
                {
                    BindableValue =
                        normalizedTypedValue;
                }
            });
    }

    /// <summary>
    /// Обрабатывает изменение значения стандартного MAUI Slider
    /// и преобразует его обратно в <typeparamref name="TValue"/>.
    /// </summary>
    private void OnSliderValueChanged(
        object? sender,
        ValueChangedEventArgs e)
    {
        if (_isInternalUpdate)
            return;

        var normalizedValue =
            Normalize(
                e.NewValue);

        SafeUpdate(
            () =>
            {
                /*
                 * Значение, полученное от платформенного Slider,
                 * может находиться между шагами, поэтому сначала
                 * приводим сам Slider к нормализованному значению.
                 */
                if (!Value.Equals(normalizedValue))
                {
                    Value =
                        normalizedValue;
                }

                var typedValue =
                    TValue.CreateChecked(
                        normalizedValue);

                if (!EqualityComparer<TValue>.Default.Equals(
                        BindableValue,
                        typedValue))
                {
                    BindableValue =
                        typedValue;
                }
            });
    }

    #endregion

    #region Normalization

    /// <summary>
    /// Повторно нормализует текущее значение слайдера,
    /// например после изменения <see cref="Step"/>.
    /// </summary>
    private void NormalizeCurrentValue()
    {
        var normalizedValue =
            Normalize(
                Value);

        SafeUpdate(
            () =>
            {
                Value =
                    normalizedValue;

                BindableValue =
                    TValue.CreateChecked(
                        normalizedValue);
            });
    }

    /// <summary>
    /// Приводит значение к ближайшему допустимому шагу
    /// относительно <see cref="Slider.Minimum"/>
    /// и ограничивает его текущим диапазоном Slider.
    /// </summary>
    private double Normalize(
        double value)
    {
        var normalizedValue =
            Minimum +
            Math.Round(
                (value - Minimum) / _step) *
            _step;

        return Math.Clamp(
            normalizedValue,
            Minimum,
            Maximum);
    }

    #endregion

    #region Internal Update

    /// <summary>
    /// Выполняет внутреннее обновление значения,
    /// временно блокируя взаимные обработчики синхронизации.
    /// </summary>
    private void SafeUpdate(
        Action action)
    {
        try
        {
            _isInternalUpdate =
                true;

            action();
        }
        finally
        {
            _isInternalUpdate =
                false;
        }
    }

    #endregion
}