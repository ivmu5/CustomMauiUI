using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MauiUiComponents;

public static class BindingExtensions
{
    private static readonly ConcurrentDictionary<string, BindableProperty>
        _propertyCache = new();

    public static T Bind<T, TObject, TSource, TTarget>(
        this T bindable,
        Expression<Func<T, TTarget>> viewExpression,
        TObject bindableObject,
        Expression<Func<TObject, TSource>> sourceExpression,
        BindingMode bindingMode = BindingMode.Default,
        Func<TSource, TTarget>? convertToTarget = null,
        Func<TTarget, TSource>? convertBackToSource = null)
        where T : BindableObject
        where TObject : BindableObject
    {
        Expression body = sourceExpression.Body;
        var sourceType = typeof(TSource);
        var targetType = typeof(TTarget);
        IValueConverter? converter = null;

        if (body is UnaryExpression unary)
            body = unary.Operand;

        if (body is not MemberExpression member)
            throw new InvalidOperationException(
                "Expression must be property access.");

        if (convertToTarget is not null || convertBackToSource is not null)
        {
            converter = new DelegateValueConverter<TSource, TTarget>(
                convertToTarget,
                convertBackToSource);
        }

        if (!targetType.IsAssignableFrom(sourceType) &&
            converter is null)
        {
            throw new InvalidOperationException(
                $"Cannot bind {sourceType.Name} to {targetType.Name} without converter.");
        }

        bindable.SetBinding(
            viewExpression.GetBindableProperty(),
            new Binding(
                member.Member.Name,
                bindingMode,
                converter,
                source: bindableObject));

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
