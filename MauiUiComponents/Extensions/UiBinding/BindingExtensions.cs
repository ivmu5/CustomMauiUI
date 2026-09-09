using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MauiUiComponents;

/// <summary>
/// Содержит методы расширения для создания и удаления
/// типизированных MAUI-привязок через lambda-выражения.
/// </summary>
public static class BindingExtensions
{
    #region Fields

    private static readonly ConcurrentDictionary<
        (Type Type, string PropertyName),
        BindableProperty> _bindablePropertyCache = new();

    #endregion

    #region Binding

    /// <summary>
    /// Создаёт привязку между свойством целевого объекта
    /// и свойством объекта-источника.
    /// </summary>
    /// <typeparam name="TTargetObject">
    /// Тип целевого объекта.
    /// </typeparam>
    /// <typeparam name="TTargetValue">
    /// Тип целевого свойства.
    /// </typeparam>
    /// <typeparam name="TSourceObject">
    /// Тип объекта-источника.
    /// </typeparam>
    /// <typeparam name="TSourceValue">
    /// Тип свойства источника.
    /// </typeparam>
    /// <param name="target">
    /// Объект, на который устанавливается привязка.
    /// </param>
    /// <param name="targetPropertyExpression">
    /// Выражение, указывающее целевое bindable-свойство.
    /// </param>
    /// <param name="source">
    /// Объект-источник данных.
    /// </param>
    /// <param name="sourcePropertyExpression">
    /// Выражение, указывающее свойство источника.
    /// Поддерживаются вложенные пути.
    /// </param>
    /// <param name="mode">
    /// Режим привязки.
    /// </param>
    /// <param name="convert">
    /// Необязательное преобразование значения
    /// от источника к целевому свойству.
    /// </param>
    /// <param name="convertBack">
    /// Необязательное обратное преобразование значения.
    /// </param>
    /// <returns>
    /// Исходный целевой объект для продолжения fluent-цепочки.
    /// </returns>
    public static TTargetObject Bind<
        TTargetObject,
        TTargetValue,
        TSourceObject,
        TSourceValue>(
        this TTargetObject target,
        Expression<Func<TTargetObject, TTargetValue>>
            targetPropertyExpression,
        TSourceObject source,
        Expression<Func<TSourceObject, TSourceValue>>
            sourcePropertyExpression,
        BindingMode mode = BindingMode.Default,
        Func<TSourceValue, TTargetValue>? convert = null,
        Func<TTargetValue, TSourceValue>? convertBack = null)
        where TTargetObject : BindableObject
        where TSourceObject : BindableObject
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(targetPropertyExpression);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sourcePropertyExpression);

        var targetProperty =
            targetPropertyExpression.GetBindableProperty();

        var sourcePath =
            sourcePropertyExpression.GetPropertyPath();

        ValidateBindingTypes(
            targetPropertyExpression,
            sourcePropertyExpression,
            convert);

        var converter =
            CreateConverter(
                convert,
                convertBack);

        var binding =
            new Binding
            {
                Path =
                    sourcePath,

                Source =
                    source,

                Mode =
                    mode,

                Converter =
                    converter
            };

        target.SetBinding(
            targetProperty,
            binding);

        return target;
    }

    /// <summary>
    /// Удаляет привязку с указанного bindable-свойства.
    /// </summary>
    /// <typeparam name="TTargetObject">
    /// Тип целевого объекта.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Тип свойства.
    /// </typeparam>
    /// <param name="target">
    /// Объект, с которого необходимо удалить привязку.
    /// </param>
    /// <param name="propertyExpression">
    /// Выражение, указывающее bindable-свойство.
    /// </param>
    /// <returns>
    /// Исходный объект для продолжения fluent-цепочки.
    /// </returns>
    public static TTargetObject Unbind<
        TTargetObject,
        TValue>(
        this TTargetObject target,
        Expression<Func<TTargetObject, TValue>>
            propertyExpression)
        where TTargetObject : BindableObject
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        var bindableProperty =
            propertyExpression.GetBindableProperty();

        target.RemoveBinding(
            bindableProperty);

        return target;
    }

    #endregion

    #region Converter

    /// <summary>
    /// Создаёт конвертер для привязки,
    /// если указано хотя бы одно направление преобразования.
    /// </summary>
    private static IValueConverter? CreateConverter<
        TSourceValue,
        TTargetValue>(
        Func<TSourceValue, TTargetValue>? convert,
        Func<TTargetValue, TSourceValue>? convertBack)
    {
        if (convert is null &&
            convertBack is null)
        {
            return null;
        }

        return new DelegateValueConverter<
            TSourceValue,
            TTargetValue>(
                convert,
                convertBack);
    }

    #endregion

    #region Validation

    /// <summary>
    /// Проверяет совместимость типов исходного
    /// и целевого свойств привязки.
    /// </summary>
    private static void ValidateBindingTypes<
        TTargetObject,
        TTargetValue,
        TSourceObject,
        TSourceValue>(
        Expression<Func<TTargetObject, TTargetValue>>
            targetExpression,
        Expression<Func<TSourceObject, TSourceValue>>
            sourceExpression,
        Func<TSourceValue, TTargetValue>? converter)
    {
        /*
         * Явный converter сам отвечает за преобразование типов,
         * поэтому дополнительная проверка в этом случае не требуется.
         */
        if (converter is not null)
            return;

        var targetType =
            GetMemberType(
                targetExpression);

        var sourceType =
            GetMemberType(
                sourceExpression);

        if (AreTypesCompatible(
                sourceType,
                targetType))
        {
            return;
        }

        throw new InvalidOperationException(
            $"Невозможно создать привязку из '{sourceType.Name}' " +
            $"в '{targetType.Name}' без конвертера.");
    }

    /// <summary>
    /// Проверяет, может ли значение исходного типа
    /// быть передано в целевое свойство без явного конвертера.
    /// </summary>
    private static bool AreTypesCompatible(
        Type sourceType,
        Type targetType)
    {
        sourceType =
            Nullable.GetUnderlyingType(sourceType)
            ?? sourceType;

        targetType =
            Nullable.GetUnderlyingType(targetType)
            ?? targetType;

        if (targetType.IsAssignableFrom(
                sourceType))
        {
            return true;
        }

        /*
         * Некоторые MAUI-типы используют операторы неявного
         * преобразования, например Color -> Brush.
         */
        return HasConversionOperator(
            sourceType,
            targetType);
    }

    /// <summary>
    /// Проверяет наличие пользовательского оператора
    /// неявного или явного преобразования между типами.
    /// </summary>
    private static bool HasConversionOperator(
        Type sourceType,
        Type targetType)
    {
        return HasConversionOperator(
                   sourceType,
                   sourceType,
                   targetType)
               ||
               HasConversionOperator(
                   targetType,
                   sourceType,
                   targetType);
    }

    /// <summary>
    /// Ищет оператор преобразования
    /// в указанном типе-владельце.
    /// </summary>
    private static bool HasConversionOperator(
        Type ownerType,
        Type sourceType,
        Type targetType)
    {
        return ownerType
            .GetMethods(
                BindingFlags.Public |
                BindingFlags.Static)
            .Any(
                method =>
                    method.Name is
                        "op_Implicit" or
                        "op_Explicit"
                    &&
                    method.ReturnType == targetType
                    &&
                    method.GetParameters() is
                    [
                        var parameter
                    ]
                    &&
                    parameter.ParameterType == sourceType);
    }

    #endregion

    #region Bindable Property

    /// <summary>
    /// Возвращает <see cref="BindableProperty"/>,
    /// соответствующее указанному свойству объекта.
    /// </summary>
    public static BindableProperty GetBindableProperty<
        TObject,
        TValue>(
        this Expression<Func<TObject, TValue>>
            propertyExpression)
        where TObject : BindableObject
    {
        ArgumentNullException.ThrowIfNull(
            propertyExpression);

        var propertyName =
            propertyExpression.GetPropertyName();

        var targetType =
            typeof(TObject);

        return _bindablePropertyCache.GetOrAdd(
            (
                targetType,
                propertyName
            ),
            static key =>
                FindBindableProperty(
                    key.Type,
                    key.PropertyName));
    }

    /// <summary>
    /// Ищет статическое поле <see cref="BindableProperty"/>
    /// для указанного свойства в типе и его базовых классах.
    /// </summary>
    private static BindableProperty FindBindableProperty(
        Type targetType,
        string propertyName)
    {
        var fieldName =
            $"{propertyName}Property";

        for (var type = targetType;
             type is not null;
             type = type.BaseType)
        {
            var field =
                type.GetField(
                    fieldName,
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly);

            if (field?.GetValue(null)
                is BindableProperty bindableProperty)
            {
                return bindableProperty;
            }
        }

        throw new InvalidOperationException(
            $"BindableProperty '{fieldName}' не найден " +
            $"для типа '{targetType.Name}'.");
    }

    #endregion

    #region Expression

    /// <summary>
    /// Возвращает имя последнего свойства,
    /// указанного в lambda-выражении.
    /// </summary>
    public static string GetPropertyName<
        TObject,
        TValue>(
        this Expression<Func<TObject, TValue>>
            propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(
            propertyExpression);

        var memberExpression =
            GetMemberExpression(
                propertyExpression.Body);

        if (memberExpression.Member
            is not PropertyInfo property)
        {
            throw new ArgumentException(
                "Выражение должно указывать на свойство.",
                nameof(propertyExpression));
        }

        return property.Name;
    }

    /// <summary>
    /// Возвращает полный путь к свойству,
    /// указанному в lambda-выражении.
    /// </summary>
    /// <example>
    /// Выражение <c>x => x.Settings.Theme</c>
    /// преобразуется в строку <c>Settings.Theme</c>.
    /// </example>
    public static string GetPropertyPath<
        TObject,
        TValue>(
        this Expression<Func<TObject, TValue>>
            propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(
            propertyExpression);

        var members =
            new Stack<string>();

        Expression? expression =
            UnwrapConversion(
                propertyExpression.Body);

        while (expression
               is MemberExpression memberExpression)
        {
            if (memberExpression.Member
                is not PropertyInfo property)
            {
                throw new ArgumentException(
                    "Путь привязки может содержать только свойства.",
                    nameof(propertyExpression));
            }

            members.Push(
                property.Name);

            expression =
                UnwrapConversion(
                    memberExpression.Expression);
        }

        if (expression
            is not ParameterExpression)
        {
            throw new ArgumentException(
                "Выражение должно начинаться с параметра lambda.",
                nameof(propertyExpression));
        }

        if (members.Count == 0)
        {
            throw new ArgumentException(
                "Выражение не содержит свойства.",
                nameof(propertyExpression));
        }

        return string.Join(
            ".",
            members);
    }

    /// <summary>
    /// Возвращает фактический тип свойства,
    /// указанного в lambda-выражении.
    /// </summary>
    private static Type GetMemberType(
        LambdaExpression expression)
    {
        var memberExpression =
            GetMemberExpression(
                expression.Body);

        if (memberExpression.Member
            is not PropertyInfo property)
        {
            throw new ArgumentException(
                "Выражение должно указывать на свойство.",
                nameof(expression));
        }

        return property.PropertyType;
    }

    /// <summary>
    /// Извлекает <see cref="MemberExpression"/>
    /// из lambda-выражения, удаляя автоматические Convert-операции.
    /// </summary>
    private static MemberExpression GetMemberExpression(
        Expression expression)
    {
        expression =
            UnwrapConversion(
                expression);

        if (expression
            is MemberExpression memberExpression)
        {
            return memberExpression;
        }

        throw new ArgumentException(
            "Выражение должно указывать на свойство.");
    }

    /// <summary>
    /// Удаляет операции приведения типов,
    /// автоматически добавленные компилятором в Expression Tree.
    /// </summary>
    private static Expression? UnwrapConversion(
        Expression? expression)
    {
        while (expression is UnaryExpression
            {
                NodeType:
                       ExpressionType.Convert
                       or ExpressionType.ConvertChecked
            } unaryExpression)
        {
            expression =
                unaryExpression.Operand;
        }

        return expression;
    }

    #endregion
}