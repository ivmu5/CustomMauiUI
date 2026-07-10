using System.ComponentModel;

namespace MauiUiComponents;

public class ValueSynchronizer<TValue> : IDisposable
{
    private readonly HashSet<IBindableValue<TValue>> _bindableValues = new();

    private bool _updating;

    public ValueSynchronizer(List<IBindableValue<TValue>>? bindableValues = null)
    {
        if (bindableValues == null)
            return;

        foreach (var value in bindableValues)
            Add(value);
    }

    public void Add(IBindableValue<TValue> bindableValue)
    {
        if (_bindableValues.Contains(bindableValue))
            return;

        _bindableValues.Add(bindableValue);

        if (bindableValue is INotifyPropertyChanged npc)
            npc.PropertyChanged += OnChanged;

        if (_bindableValues.Count > 0)
            bindableValue.BindableValue = _bindableValues.FirstOrDefault()!.BindableValue;
    }

    public void Remove(IBindableValue<TValue> bindableValue)
    {
        if (_bindableValues.Remove(bindableValue) &&
            bindableValue is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged -= OnChanged;
        }
    }

    private void OnChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_updating)
            return;

        if (sender is not IBindableValue<TValue> source)
            return;

        if (e.PropertyName != nameof(IBindableValue<TValue>.BindableValue))
            return;

        _updating = true;

        var value = source.BindableValue;

        try
        {
            foreach (var bindableValue in _bindableValues)
            {
                if (ReferenceEquals(bindableValue, source))
                    continue;

                bindableValue.BindableValue = value;
            }
        }
        finally
        {
            _updating = false;
        }
    }

    public void Dispose()
    {
        foreach (var bindableValue in _bindableValues)
        {
            if (bindableValue is INotifyPropertyChanged npc)
                npc.PropertyChanged -= OnChanged;
        }

        _bindableValues.Clear();
    }
}
