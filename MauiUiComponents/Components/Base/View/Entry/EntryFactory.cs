namespace MauiUiComponents;

/// <summary>
/// Предоставляет фабрику типизированных полей ввода
/// для поддерживаемых типов значений.
/// </summary>
public static class EntryFactory
{
    #region Fields

    private static readonly Dictionary<Type, Func<object>>
        _factories =
            new()
            {
                [typeof(int)] =
                    static () => new BaseIntEntry(),

                [typeof(double)] =
                    static () => new BaseDoubleEntry()
            };

    #endregion

    #region Creation

    /// <summary>
    /// Создаёт типизированное поле ввода
    /// для указанного типа значения.
    /// </summary>
    /// <typeparam name="TValue">
    /// Тип значения создаваемого поля ввода.
    /// </typeparam>
    /// <returns>
    /// Зарегистрированная реализация
    /// <see cref="BaseEntry{TValue}"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Для типа <typeparamref name="TValue"/>
    /// не зарегистрирована фабрика.
    /// </exception>
    public static BaseEntry<TValue> Create<TValue>()
    {
        var valueType =
            typeof(TValue);

        if (!_factories.TryGetValue(
                valueType,
                out var factory))
        {
            throw new NotSupportedException(
                $"Поле ввода для типа '{valueType.Name}' не зарегистрировано.");
        }

        var entry =
            factory();

        if (entry is BaseEntry<TValue> typedEntry)
        {
            return typedEntry;
        }

        /*
         * Такое состояние означает ошибку регистрации:
         * фабрика была сохранена под одним TValue,
         * но фактически вернула BaseEntry другого типа.
         */
        throw new InvalidOperationException(
            $"Фабрика для типа '{valueType.Name}' вернула " +
            $"несовместимый компонент '{entry.GetType().Name}'.");
    }

    #endregion

    #region Registration

    /// <summary>
    /// Регистрирует или заменяет фабрику поля ввода
    /// для указанного типа значения.
    /// </summary>
    /// <typeparam name="TValue">
    /// Тип значения поля ввода.
    /// </typeparam>
    /// <param name="factory">
    /// Фабрика, создающая соответствующий
    /// <see cref="BaseEntry{TValue}"/>.
    /// </param>
    public static void Register<TValue>(
        Func<BaseEntry<TValue>> factory)
    {
        ArgumentNullException.ThrowIfNull(
            factory);

        _factories[typeof(TValue)] =
            () => factory();
    }

    #endregion
}