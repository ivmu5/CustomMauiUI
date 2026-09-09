namespace MauiUiComponents;

/// <summary>
/// Базовое типизированное поле ввода, связывающее текстовое представление
/// <see cref="Entry"/> со значением типа <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">
/// Тип значения, редактируемого компонентом.
/// </typeparam>
public abstract class BaseEntry<TValue> :
    BaseEntry,
    IBindableValue<TValue>
{
    #region Fields

    private bool _isInternalUpdate;

    #endregion

    #region Properties

    /// <summary>
    /// Получает или устанавливает типизированное значение поля ввода.
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
            typeof(BaseEntry<TValue>),
            default(TValue),
            BindingMode.TwoWay,
            propertyChanged: OnBindableValueChanged);

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт типизированное поле ввода
    /// и подключает обработку изменения текста.
    /// </summary>
    protected BaseEntry()
    {
        TextChanged +=
            OnTextChanged;
    }

    #endregion

    #region Value Synchronization

    /// <summary>
    /// Обрабатывает изменение <see cref="BindableValue"/>
    /// и обновляет текстовое представление значения.
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

        var entry =
            (BaseEntry<TValue>)bindable;

        if (newValue is not TValue value)
        {
            entry.UpdateText(
                string.Empty);

            return;
        }

        entry.UpdateText(
            entry.Format(value));
    }

    /// <summary>
    /// Обрабатывает пользовательское изменение текста,
    /// фильтрует ввод и при успешном преобразовании
    /// обновляет <see cref="BindableValue"/>.
    /// </summary>
    private void OnTextChanged(
        object? sender,
        TextChangedEventArgs e)
    {
        if (_isInternalUpdate)
            return;

        var text =
            e.NewTextValue ?? string.Empty;

        var filteredText =
            Filter(text);

        if (!string.Equals(
                text,
                filteredText,
                StringComparison.Ordinal))
        {
            UpdateText(
                filteredText);
        }

        /*
         * Некоторые промежуточные состояния ввода,
         * например пустая строка или одиночный знак минуса,
         * могут быть допустимы для редактирования,
         * но ещё не представляют корректное TValue.
         */
        if (!Parse(
                filteredText,
                out var value))
        {
            return;
        }

        if (EqualityComparer<TValue>.Default.Equals(
                BindableValue,
                value))
        {
            return;
        }

        SafeUpdate(
            () =>
                BindableValue = value);
    }

    /// <summary>
    /// Обновляет текст поля без повторного запуска
    /// логики синхронизации значения.
    /// </summary>
    private void UpdateText(
        string text)
    {
        if (string.Equals(
                Text,
                text,
                StringComparison.Ordinal))
        {
            return;
        }

        SafeUpdate(
            () =>
                Text = text);
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Пытается преобразовать текстовое представление
    /// в значение типа <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="text">
    /// Текст после применения <see cref="Filter"/>.
    /// </param>
    /// <param name="value">
    /// Полученное значение при успешном преобразовании.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, если преобразование выполнено успешно;
    /// иначе <see langword="false"/>.
    /// </returns>
    protected abstract bool Parse(
        string text,
        out TValue value);

    /// <summary>
    /// Преобразует типизированное значение
    /// в текст для отображения в поле ввода.
    /// </summary>
    protected abstract string Format(
        TValue value);

    /// <summary>
    /// Фильтрует пользовательский ввод,
    /// оставляя только допустимые для конкретного типа символы.
    /// </summary>
    protected abstract string Filter(
        string text);

    #endregion

    #region Internal Update

    /// <summary>
    /// Выполняет внутреннее изменение состояния,
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