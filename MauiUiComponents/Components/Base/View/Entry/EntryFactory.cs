namespace MauiUiComponents;

public static class EntryFactory
{
    private static readonly Dictionary<Type, Func<BaseEntry>> Factories = new()
    {
        [typeof(int)] = () => new BaseIntEntry(),
        [typeof(double)] = () => new BaseDoubleEntry(),
    };

    public static BaseEntry<TValue> Create<TValue>()
    {
        if (!Factories.TryGetValue(typeof(TValue), out var factory))
        {
            throw new NotSupportedException(
                $"Entry for type '{typeof(TValue).Name}' is not registered.");
        }

        return (BaseEntry<TValue>)factory();
    }

    public static void Register<TValue>(
        Func<BaseEntry<TValue>> factory)
    {
        Factories[typeof(TValue)] = factory;
    }
}
