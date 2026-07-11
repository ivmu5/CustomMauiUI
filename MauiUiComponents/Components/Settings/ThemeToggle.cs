using MauiUiComponents.Resources.Localization;
using MauiUiSettings;

namespace MauiUiComponents;

public class ThemeToggle<TView> : ToggleGroup<FlexLayout>
    where TView : View, ITextComponent, new()
{
    private readonly UiServiceStore _uiServices;
    private readonly ComponentStore _componentStore;

    private readonly ToggleItem<TView> _viewSystemTheme;
    private readonly ToggleItem<TView> _viewDarkTheme;
    private readonly ToggleItem<TView> _viewLightTheme;

    private readonly Dictionary<TView, ThemeType> _themeMap = new();



    public ThemeToggle(
        UiServiceStore uiServices,
        ComponentStore componentStore)
        : base(componentStore)
    {
        _uiServices = uiServices;
        _componentStore = componentStore;

        UseCaption = true;
        CaptionLabel.TextBind(
            _componentStore.ResourcesStore.SettingsLocalization,
            nameof(Settings.ThemeSetting));

        _viewSystemTheme = CreateThemeView(ThemeType.System, nameof(Settings.ThemeSystem));
        _viewDarkTheme = CreateThemeView(ThemeType.Dark, nameof(Settings.ThemeDark));
        _viewLightTheme = CreateThemeView(ThemeType.Light, nameof(Settings.ThemeLight));

        SelectedItem = GetCurrentThemeView();

        _uiServices.ThemeService.PropertyChanged += OnThemeChanged;

        ToggleLayout.FlexEqualGrow();
    }

    private ToggleItem<TView> GetCurrentThemeView()
    {
        return _uiServices.ThemeService.CurrentTheme switch
        {
            ThemeType.System => _viewSystemTheme,
            ThemeType.Dark => _viewDarkTheme,
            ThemeType.Light => _viewLightTheme,
            _ => throw new NotImplementedException()
        };
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        var currentView = GetCurrentThemeView();
        if (currentView == SelectedItem)
            return;

        SelectedItem = currentView;
    }

    private ToggleItem<TView> CreateThemeView(
        ThemeType theme,
        string localizationKey)
    {
        var toggleStore = _componentStore.Custom.ToggleGroup;

        var toggleItem = toggleStore
            .BaseTextToggleView<TView>(
                _componentStore.ResourcesStore.SettingsLocalization,
                localizationKey);
        toggleItem.AddAction(
            toggleStore.Styles.ToggleBackgroundColor<TView>(toggleItem.View),
            new ToggleBehavior<TView>(
                toggleItem.View,
                (_) => _uiServices.ThemeService.SetTheme(theme),
                (_) => { },
                ToggleTrigger.BusinessAction));

        toggleItem.View.ViewFillHorizontal();

        _themeMap[toggleItem.View] = theme;
        AddItem(toggleItem);

        return toggleItem;
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent == null)
        {
            _uiServices.ThemeService.PropertyChanged -= OnThemeChanged;
        }
    }
}