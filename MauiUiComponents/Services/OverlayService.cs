using Microsoft.Maui.Layouts;

namespace MauiUiComponents;

/// <summary>
/// Управляет отображением временных overlay-компонентов
/// поверх содержимого страницы.
/// </summary>
public sealed class OverlayService : IOverlayService
{
    #region Fields

    private readonly Grid _rootLayout;

    private readonly Dictionary<View, AbsoluteLayout>
        _overlays = new();

    #endregion

    #region Constructor

    /// <summary>
    /// Создаёт сервис управления overlay-компонентами
    /// для указанного корневого контейнера.
    /// </summary>
    /// <param name="rootLayout">
    /// Корневой <see cref="Grid"/>, поверх которого
    /// будут отображаться overlay-компоненты.
    /// </param>
    /// <param name="componentStore">
    /// Хранилище компонентов.
    /// Временно сохраняется в сигнатуре конструктора
    /// для совместимости с существующим кодом.
    /// </param>
    public OverlayService(
        Grid rootLayout,
        ComponentStore componentStore)
    {
        ArgumentNullException.ThrowIfNull(rootLayout);
        ArgumentNullException.ThrowIfNull(componentStore);

        _rootLayout =
            rootLayout;
    }

    #endregion

    #region Overlay Management

    /// <summary>
    /// Добавляет представление поверх основного содержимого страницы.
    /// </summary>
    /// <param name="view">
    /// Представление, которое необходимо показать как overlay.
    /// </param>
    /// <param name="placement">
    /// Способ расположения overlay.
    /// </param>
    /// <param name="anchor">
    /// Элемент, относительно которого располагается overlay.
    /// Для <see cref="OverlayPlacement.Center"/> не требуется.
    /// </param>
    /// <param name="onOverlayTapped">
    /// Дополнительное действие, выполняемое после нажатия
    /// на overlay-слой.
    /// </param>
    public void AddOverlay(
        View view,
        OverlayPlacement placement,
        View? anchor = null,
        Action<View>? onOverlayTapped = null)
    {
        ArgumentNullException.ThrowIfNull(view);

        ValidatePlacement(
            placement,
            anchor);

        // Повторное добавление одного View не должно оставлять
        // предыдущий AbsoluteLayout внутри корневого контейнера.
        RemoveOverlay(
            view);

        if (view.Parent is not null)
        {
            throw new InvalidOperationException(
                "Overlay view уже принадлежит другому визуальному контейнеру.");
        }

        SetPosition(
            view,
            placement,
            anchor);

        view.ViewAddShadow();

        var overlayLayout =
            CreateOverlayLayout(
                view,
                onOverlayTapped);

        overlayLayout.Add(
            view);

        _rootLayout.Add(
            overlayLayout);

        _overlays.Add(
            view,
            overlayLayout);
    }

    /// <summary>
    /// Удаляет ранее добавленный overlay.
    /// Если указанное представление не зарегистрировано,
    /// метод ничего не делает.
    /// </summary>
    /// <param name="view">
    /// Представление overlay, которое необходимо удалить.
    /// </param>
    public void RemoveOverlay(
        View view)
    {
        ArgumentNullException.ThrowIfNull(view);

        if (!_overlays.TryGetValue(
                view,
                out var overlayLayout))
        {
            return;
        }

        overlayLayout.Remove(
            view);

        _rootLayout.Remove(
            overlayLayout);

        _overlays.Remove(
            view);
    }

    #endregion

    #region Overlay Creation

    /// <summary>
    /// Создаёт полноэкранный контейнер,
    /// используемый как слой для конкретного overlay.
    /// </summary>
    /// <param name="view">
    /// Представление, связанное с создаваемым overlay.
    /// </param>
    /// <param name="onOverlayTapped">
    /// Дополнительное действие после нажатия на overlay.
    /// </param>
    private AbsoluteLayout CreateOverlayLayout(
        View view,
        Action<View>? onOverlayTapped)
    {
        var overlayLayout =
            new AbsoluteLayout
            {
                HorizontalOptions =
                    LayoutOptions.Fill,

                VerticalOptions =
                    LayoutOptions.Fill,

                ZIndex =
                    int.MaxValue
            };

        /*
         * Overlay должен перекрывать весь корневой Grid,
         * даже если позже у него появится несколько строк
         * или колонок.
         */
        Grid.SetRowSpan(
            overlayLayout,
            Math.Max(
                1,
                _rootLayout.RowDefinitions.Count));

        Grid.SetColumnSpan(
            overlayLayout,
            Math.Max(
                1,
                _rootLayout.ColumnDefinitions.Count));

        overlayLayout.ViewOnTapped(
            _ => RemoveOverlay(view),
            onOverlayTapped);

        return overlayLayout;
    }

    #endregion

    #region Positioning

    /// <summary>
    /// Устанавливает положение overlay внутри
    /// его <see cref="AbsoluteLayout"/>.
    /// </summary>
    private void SetPosition(
        View view,
        OverlayPlacement placement,
        View? anchor)
    {
        switch (placement)
        {
            case OverlayPlacement.Center:
                SetCenterPosition(
                    view);
                break;

            case OverlayPlacement.BelowAnchor:
                SetBelowAnchorPosition(
                    view,
                    anchor!);
                break;

            default:
                throw new NotSupportedException(
                    $"Расположение overlay '{placement}' пока не реализовано.");
        }
    }

    /// <summary>
    /// Центрирует overlay относительно всего доступного пространства.
    /// </summary>
    private static void SetCenterPosition(
        View view)
    {
        AbsoluteLayout.SetLayoutFlags(
            view,
            AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(
            view,
            new Rect(
                0.5,
                0.5,
                AbsoluteLayout.AutoSize,
                AbsoluteLayout.AutoSize));
    }

    /// <summary>
    /// Располагает overlay непосредственно под указанным элементом.
    /// </summary>
    private void SetBelowAnchorPosition(
        View view,
        View anchor)
    {
        var anchorPosition =
            GetPositionRelativeToRoot(
                anchor);

        AbsoluteLayout.SetLayoutFlags(
            view,
            AbsoluteLayoutFlags.None);

        AbsoluteLayout.SetLayoutBounds(
            view,
            new Rect(
                anchorPosition.X,
                anchorPosition.Y + anchor.Height,
                AbsoluteLayout.AutoSize,
                AbsoluteLayout.AutoSize));
    }

    /// <summary>
    /// Вычисляет положение элемента относительно
    /// корневого контейнера overlay-системы.
    /// </summary>
    /// <remarks>
    /// Координаты последовательно накапливаются по визуальному дереву.
    /// Для <see cref="ScrollView"/> дополнительно учитывается
    /// текущее смещение прокрутки.
    /// </remarks>
    private Point GetPositionRelativeToRoot(
        View view)
    {
        double x = 0;
        double y = 0;

        Element? current =
            view;

        while (current is not null &&
               !ReferenceEquals(
                   current,
                   _rootLayout))
        {
            if (current is View currentView)
            {
                x +=
                    currentView.X +
                    currentView.TranslationX;

                y +=
                    currentView.Y +
                    currentView.TranslationY;
            }

            /*
             * Layout-координаты содержимого ScrollView
             * не отражают его текущее визуальное смещение,
             * поэтому компенсируем прокрутку вручную.
             */
            if (current.Parent is ScrollView scrollView)
            {
                x -=
                    scrollView.ScrollX;

                y -=
                    scrollView.ScrollY;
            }

            current =
                current.Parent;
        }

        if (current is null)
        {
            throw new InvalidOperationException(
                "Anchor должен находиться внутри визуального дерева текущей страницы.");
        }

        return new Point(
            x,
            y);
    }

    #endregion

    #region Validation

    /// <summary>
    /// Проверяет корректность параметров
    /// выбранного способа размещения overlay.
    /// </summary>
    private static void ValidatePlacement(
        OverlayPlacement placement,
        View? anchor)
    {
        if (placement != OverlayPlacement.Center &&
            anchor is null)
        {
            throw new ArgumentNullException(
                nameof(anchor),
                "Для размещения относительно элемента необходимо указать anchor.");
        }

        if (placement is not
            OverlayPlacement.Center and not
            OverlayPlacement.BelowAnchor)
        {
            throw new NotSupportedException(
                $"Расположение overlay '{placement}' пока не реализовано.");
        }
    }

    #endregion
}