using System.ComponentModel;

namespace MauiUiComponents;

/// <summary>
/// Синхронизирует значение между несколькими компонентами,
/// реализующими <see cref="IBindableValue{T}"/>.
/// </summary>
/// <typeparam name="TValue">
/// Тип синхронизируемого значения.
/// </typeparam>
public sealed class ValueSynchronizer<TValue> :
    IDisposable
{
    #region Fields

    private readonly List<IBindableValue<TValue>>
        _values = new();

    private bool _isUpdating;
    private bool _disposed;

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт синхронизатор значений
    /// и при необходимости сразу добавляет набор компонентов.
    /// </summary>
    /// <param name="values">
    /// Компоненты, значения которых необходимо синхронизировать.
    /// Первый элемент используется как источник начального значения.
    /// </param>
    public ValueSynchronizer(
        IEnumerable<IBindableValue<TValue>>? values = null)
    {
        if (values is null)
            return;

        foreach (var value in values)
        {
            Add(
                value);
        }
    }

    #endregion

    #region Values

    /// <summary>
    /// Добавляет компонент в синхронизацию.
    /// </summary>
    /// <param name="value">
    /// Компонент, значение которого необходимо синхронизировать.
    /// </param>
    /// <remarks>
    /// Первый добавленный компонент становится источником
    /// начального значения. Все последующие компоненты
    /// при добавлении получают его текущее значение.
    /// </remarks>
    public void Add(
        IBindableValue<TValue> value)
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);

        ArgumentNullException.ThrowIfNull(
            value);

        if (_values.Contains(value))
            return;

        if (_values.Count > 0)
        {
            value.BindableValue =
                _values[0].BindableValue;
        }

        _values.Add(
            value);

        Subscribe(
            value);
    }

    /// <summary>
    /// Удаляет компонент из синхронизации.
    /// </summary>
    /// <param name="value">
    /// Компонент, который необходимо удалить.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, если компонент был найден
    /// и удалён; иначе <see langword="false"/>.
    /// </returns>
    public bool Remove(
        IBindableValue<TValue> value)
    {
        ObjectDisposedException.ThrowIf(
            _disposed,
            this);

        ArgumentNullException.ThrowIfNull(
            value);

        if (!_values.Remove(value))
            return false;

        Unsubscribe(
            value);

        return true;
    }

    #endregion

    #region Synchronization

    /// <summary>
    /// Обрабатывает изменение свойства одного
    /// из синхронизируемых компонентов.
    /// </summary>
    private void OnValuePropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (_isUpdating)
            return;

        if (e.PropertyName !=
            nameof(IBindableValue<TValue>.BindableValue))
        {
            return;
        }

        if (sender is not IBindableValue<TValue> source)
            return;

        SynchronizeFrom(
            source);
    }

    /// <summary>
    /// Копирует текущее значение указанного источника
    /// во все остальные синхронизируемые компоненты.
    /// </summary>
    private void SynchronizeFrom(
        IBindableValue<TValue> source)
    {
        try
        {
            _isUpdating =
                true;

            var newValue =
                source.BindableValue;

            foreach (var target in _values)
            {
                if (ReferenceEquals(
                        target,
                        source))
                {
                    continue;
                }

                if (EqualityComparer<TValue>.Default.Equals(
                        target.BindableValue,
                        newValue))
                {
                    continue;
                }

                target.BindableValue =
                    newValue;
            }
        }
        finally
        {
            _isUpdating =
                false;
        }
    }

    #endregion

    #region Subscriptions

    /// <summary>
    /// Подписывается на изменение свойств указанного компонента,
    /// если он поддерживает <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    private void Subscribe(
        IBindableValue<TValue> value)
    {
        if (value is not INotifyPropertyChanged notifyPropertyChanged)
        {
            throw new InvalidOperationException(
                $"Тип '{value.GetType().Name}' должен реализовывать " +
                $"{nameof(INotifyPropertyChanged)} для участия в синхронизации.");
        }

        notifyPropertyChanged.PropertyChanged +=
            OnValuePropertyChanged;
    }

    /// <summary>
    /// Снимает подписку с указанного компонента.
    /// </summary>
    private void Unsubscribe(
        IBindableValue<TValue> value)
    {
        if (value is not INotifyPropertyChanged notifyPropertyChanged)
            return;

        notifyPropertyChanged.PropertyChanged -=
            OnValuePropertyChanged;
    }

    #endregion

    #region Dispose

    /// <summary>
    /// Снимает все зарегистрированные обработчики событий
    /// и завершает работу синхронизатора.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (var value in _values)
        {
            Unsubscribe(
                value);
        }

        _values.Clear();

        _disposed =
            true;

        GC.SuppressFinalize(
            this);
    }

    #endregion
}