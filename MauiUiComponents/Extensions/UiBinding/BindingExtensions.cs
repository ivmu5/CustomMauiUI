using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MauiUiComponents;

public static class BindingExtensions
{
    private static readonly ConcurrentDictionary<string, BindableProperty>
        _propertyCache = new();

    public static TBindableObject Bind<TBindableObject, TSourceObject, TSourceValue, TBindableValue>(
        this TBindableObject bindable,
        Expression<Func<TBindableObject, TBindableValue>> bindablePropertyExpression,
        TSourceObject sourceObject,
        Expression<Func<TSourceObject, TSourceValue>> sourcePropertyExpression,
        BindingMode bindingMode = BindingMode.Default,
        Func<TSourceValue, TBindableValue>? sourceToBindable = null,
        Func<TBindableValue, TSourceValue>? bindableToSource = null)
        where TBindableObject : BindableObject
        where TSourceObject : BindableObject
    {
        Expression sourceBody = sourcePropertyExpression.Body;

        var sourceValueType = typeof(TSourceValue);
        var bindableValueType = typeof(TBindableValue);

        IValueConverter? converter = null;

        if (sourceBody is UnaryExpression unary)
            sourceBody = unary.Operand;

        if (sourceBody is not MemberExpression member)
        {
            throw new InvalidOperationException(
                "Expression must be property access.");
        }

        if (sourceToBindable is not null || bindableToSource is not null)
        {
            converter = new DelegateValueConverter<TSourceValue, TBindableValue>(
                sourceToBindable,
                bindableToSource);
        }

        if (!bindableValueType.IsAssignableFrom(sourceValueType) &&
            converter is null)
        {
            throw new InvalidOperationException(
                $"Cannot bind {sourceValueType.Name} to {bindableValueType.Name} without converter.");
        }

        bindable.SetBinding(
            bindablePropertyExpression.GetBindableProperty(),
            new Binding(
                member.Member.Name,
                bindingMode,
                converter,
                source: sourceObject));

        return bindable;
    }


    public static BindableProperty GetBindableProperty(
        this Type type,
        string propertyName)
    {
        var cacheKey = $"{type.FullName}.{propertyName}";

        return _propertyCache.GetOrAdd(
            cacheKey,
            _ =>
            {
                var field = type.GetField(
                    $"{propertyName}Property",
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy);

                if (field?.GetValue(null) is not BindableProperty property)
                    throw new InvalidOperationException(
                        $"BindableProperty for '{propertyName}' not found.");

                return property;
            });
    }

    public static BindableProperty GetBindableProperty<T, TValue>(
        this Expression<Func<T, TValue>> expression)
    {
        Expression body = expression.Body;

        if (body is UnaryExpression unary)
            body = unary.Operand;

        if (body is not MemberExpression member)
            throw new InvalidOperationException(
                "Expression must be property access.");

        return typeof(T)
            .GetBindableProperty(member.Member.Name);
    }
}
