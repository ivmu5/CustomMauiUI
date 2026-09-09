using MauiUiSettings;
using MauiUiSettings.Resources.Localization.MaterialSymbols;
using System.Numerics;

namespace MauiUiComponents;

/// <summary>
/// Составной компонент для редактирования числового значения,
/// объединяющий подпись, поле ввода, слайдер и кнопку сброса.
/// </summary>
/// <typeparam name="TValue">
/// Тип числового значения компонента.
/// </typeparam>
public class CustomTextSlider<TValue> :
    ContentView,
    IBindableValue<TValue>,
    IDisposable
    where TValue :
        INumber<TValue>,
        IMinMaxValue<TValue>
{
    #region Fields

    private TValue _minimum =
        TValue.MinValue;

    private TValue _maximum =
        TValue.MaxValue;

    private TValue _defaultValue =
        TValue.Zero;

    private bool _hasDefaultValue;
    private bool _disposed;

    #endregion

    #region Properties

    /// <summary>
    /// Получает внешнюю рамку всего компонента.
    /// </summary>
    public BaseBorder<Grid> RootBorder { get; }

    /// <summary>
    /// Получает текстовую подпись компонента.
    /// </summary>
    public BaseLabel TextLabel { get; }

    /// <summary>
    /// Получает поле ручного ввода числового значения.
    /// </summary>
    public BaseEntry<TValue> Entry { get; }

    /// <summary>
    /// Получает рамку поля ручного ввода.
    /// </summary>
    public BaseBorder<BaseEntry<TValue>> EntryBorder { get; }

    /// <summary>
    /// Получает слайдер изменения числового значения.
    /// </summary>
    public BaseSlider<TValue> Slider { get; }

    /// <summary>
    /// Получает кнопку сброса значения.
    /// </summary>
    public BaseButton ResetButton { get; }

    /// <summary>
    /// Получает рамку кнопки сброса значения.
    /// </summary>
    public BaseBorder<BaseButton> ResetButtonBorder { get; }

    /// <summary>
    /// Получает синхронизатор значения между
    /// компонентом, полем ввода и слайдером.
    /// </summary>
    public ValueSynchronizer<TValue> ValueSynchronizer { get; }

    /// <summary>
    /// Получает минимальное допустимое значение.
    /// </summary>
    public TValue Minimum =>
        _minimum;

    /// <summary>
    /// Получает максимальное допустимое значение.
    /// </summary>
    public TValue Maximum =>
        _maximum;

    /// <summary>
    /// Получает или устанавливает шаг изменения значения слайдера.
    /// </summary>
    public double Step
    {
        get =>
            Slider.Step;

        set =>
            Slider.Step = value;
    }

    /// <summary>
    /// Получает или устанавливает значение,
    /// используемое при нажатии кнопки сброса.
    /// </summary>
    public TValue DefaultValue
    {
        get =>
            _defaultValue;

        set
        {
            _defaultValue =
                value;

            _hasDefaultValue =
                true;
        }
    }

    /// <summary>
    /// Получает или устанавливает текущее значение компонента.
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
            typeof(CustomTextSlider<TValue>),
            TValue.Zero,
            BindingMode.TwoWay,
            coerceValue: CoerceBindableValue);

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт составной компонент
    /// для редактирования числового значения.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    public CustomTextSlider(
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(
            componentStore);

        TextLabel =
            componentStore.Base.Label();

        Entry =
            componentStore.Base.Entry<TValue>();

        EntryBorder =
            Entry.WithBorder(
                componentStore);

        Slider =
            componentStore.Base.Slider<TValue>();

        ResetButton =
            componentStore.Base
                .Button(fontVariant: FontVariant.Icon)
                .TextIconBind(
                    componentStore,
                    nameof(MaterialSymbols.Refresh));

        ResetButtonBorder =
            ResetButton.WithBorder(
                componentStore);

        ValueSynchronizer =
            new ValueSynchronizer<TValue>(
                [
                    this,
                    Entry,
                    Slider
                ]);

        RootBorder =
            BuildLayout(
                componentStore);

        Content =
            RootBorder;

        SubscribeEvents();
    }

    #endregion

    #region Configuration

    /// <summary>
    /// Устанавливает допустимый диапазон значений компонента.
    /// </summary>
    /// <param name="minimum">
    /// Минимальное допустимое значение.
    /// </param>
    /// <param name="maximum">
    /// Максимальное допустимое значение.
    /// </param>
    /// <returns>
    /// Исходный компонент для продолжения fluent-цепочки.
    /// </returns>
    public CustomTextSlider<TValue> SetRange(
        TValue minimum,
        TValue maximum)
    {
        if (minimum > maximum)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minimum),
                "Минимальное значение не может быть больше максимального.");
        }

        _minimum =
            minimum;

        _maximum =
            maximum;

        SetSliderRange(
            double.CreateChecked(
                minimum),
            double.CreateChecked(
                maximum));

        CoerceValue(
            BindableValueProperty);

        return this;
    }

    /// <summary>
    /// Устанавливает диапазон внутреннего MAUI Slider,
    /// не создавая временно недопустимое состояние
    /// Minimum больше Maximum.
    /// </summary>
    private void SetSliderRange(
        double minimum,
        double maximum)
    {
        if (minimum > Slider.Maximum)
        {
            Slider.Maximum =
                maximum;

            Slider.Minimum =
                minimum;

            return;
        }

        if (maximum < Slider.Minimum)
        {
            Slider.Minimum =
                minimum;

            Slider.Maximum =
                maximum;

            return;
        }

        Slider.Minimum =
            minimum;

        Slider.Maximum =
            maximum;
    }

    #endregion

    #region Bindable Value

    /// <summary>
    /// Ограничивает значение компонента
    /// установленным диапазоном.
    /// </summary>
    private static object CoerceBindableValue(
        BindableObject bindable,
        object value)
    {
        var slider =
            (CustomTextSlider<TValue>)bindable;

        return TValue.Clamp(
            (TValue)value,
            slider._minimum,
            slider._maximum);
    }

    #endregion

    #region Layout

    /// <summary>
    /// Формирует внутреннюю структуру компонента
    /// и оборачивает её в стандартную рамку.
    /// </summary>
    /// <param name="componentStore">
    /// Центральное хранилище UI-компонентов и сервисов.
    /// </param>
    /// <returns>
    /// Рамка с внутренним Grid компонента.
    /// </returns>
    private BaseBorder<Grid> BuildLayout(
        ComponentStore componentStore)
    {
        var grid =
            new Grid()
                .AddAutoColumn()
                .AddStarColumn()
                .AddAutoColumn()
                .AddAutoRow()
                .AddAutoRow();

        TextLabel
            .TextCenter()
            .ViewFillHorizontal()
            .ViewVerticalCenter();

        Entry
            .ViewVerticalCenter();

        EntryBorder
            .ViewVerticalCenter();

        Slider
            .ViewFillHorizontal()
            .ViewVerticalCenter();

        ResetButton
            .ViewCenter();

        ResetButtonBorder
            .ViewCenter();

        grid
            .AddChild(
                TextLabel,
                0,
                0,
                columnSpan: 3)

            .AddChild(
                EntryBorder,
                1,
                0)

            .AddChild(
                Slider,
                1,
                1)

            .AddChild(
                ResetButtonBorder,
                1,
                2);

        return grid.WithBorder(
            componentStore);
    }

    #endregion

    #region Events

    /// <summary>
    /// Регистрирует обработчики событий компонента.
    /// </summary>
    private void SubscribeEvents()
    {
        ResetButton.Clicked +=
            OnResetButtonClicked;
    }

    /// <summary>
    /// Восстанавливает значение по умолчанию
    /// после нажатия кнопки сброса.
    /// </summary>
    private void OnResetButtonClicked(
        object? sender,
        EventArgs e)
    {
        if (!_hasDefaultValue)
            return;

        BindableValue =
            TValue.Clamp(
                _defaultValue,
                _minimum,
                _maximum);
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Освобождает обработчики событий
    /// и внутренний синхронизатор значений.
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed)
            return;

        ResetButton.Clicked -=
            OnResetButtonClicked;

        ValueSynchronizer.Dispose();

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}